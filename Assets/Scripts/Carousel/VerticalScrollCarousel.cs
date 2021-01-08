using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace carousel
{
    /// <summary>
    ///     Carousel for a vertical scroll :
    ///         we won't have 1 ui element for 1 element but
    ///         a defined number of ui element that will 
    ///         display the elements in function of where
    ///         we are in the scroll. It works because
    ///         we have onscreen only a part of every elements
    ///         we want to display.
    ///         
    ///         There will be 3 displayers above and 3 displayers
    ///         under the mask view so we won't see any flickering
    ///         while pooling the displayers.
    ///         
    /// 
    /// </summary>
    /// <typeparam name="T">The ui element we want to use the carousel on</typeparam>
    /// <typeparam name="U">The elements that will be shown inside the T elements</typeparam>
    public class VerticalScrollCarousel<T, U> : MonoBehaviour where T : ACarouselElementDisplayer<U>
    {
        [SerializeField] protected T[] _displayers = default(T[]);
        public U[] _elementsToDisplay = new U[0];
        [SerializeField] protected ScrollRect _scrollDriver = default(ScrollRect);
        [SerializeField] protected bool _isUsingScrollbar = default;
        [SerializeField] protected Scrollbar _scrollbar = default;

        protected float _height;
        protected float _minScroll;
        protected float _maxScroll;
        protected float _range;
        protected bool _ready;
        protected float _spacing = 1.1f;
        protected float _startScrollPosition = 1f;
        protected float _elementDisplayedAtTheSameTime;
        protected float StartPositionOffsetPercent { get; set; } = .6f;
        protected float _startPositionOffset;
        protected float _contentHeight;

        public float MaxScroll
        {
            get
            {
                return _maxScroll;
            }
        }

        protected int _firstElementShownPosition;
        private float _elementHeightScrollPercentage;
        protected const int NUMBER_OF_ELEMENTS_NON_DISPLAYED_ABOVE = 3;
        protected const int NUMBER_OF_ELEMENTS_NON_DISPLAYED_UNDER = 3;

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (_scrollDriver == null)
            {
                _scrollDriver = GetComponent<ScrollRect>();
                if (_scrollDriver == null)
                {
                    Debug.LogWarning("No scroll rect setted in the carousel.", this);
                }
            }
            if (_displayers == null)
            {
                Debug.LogWarning("No displayers setted in the editor. The carousel won't work without them.", this);
            }
        }
#endif
        public void SetupCarousel()
        {
            SetValues();
            if (_elementsToDisplay.Length >= _elementDisplayedAtTheSameTime)
            {
                _minScroll = CalculateScrollPositionFromElementPosition(_elementsToDisplay.Length - _elementDisplayedAtTheSameTime);
            }
            SetupScrollbar(_isUsingScrollbar);
            ReplenishDisplayers();
            LoopCarouselAccordingToPosition(_maxScroll);
            gameObject.SetActive(true);
        }
        protected virtual void SetValues()
        {
            _scrollDriver.normalizedPosition = new Vector2(0, 1);
            _height = _displayers[0].RectTransform.rect.height;
            _contentHeight = (_displayers.Length * _height) - _scrollDriver.viewport.rect.height;
            _elementHeightScrollPercentage = _height / _contentHeight;
            _firstElementShownPosition = 0;
            _maxScroll = 1f;
            _startPositionOffset = _elementHeightScrollPercentage * StartPositionOffsetPercent;
            _minScroll = 1f;
            _startScrollPosition = 1f;
            _elementDisplayedAtTheSameTime = _scrollDriver.viewport.rect.height / (_height * _spacing);
        }
        /// <summary>
        /// if isUsingScrollbar is true Resize the scrollbar according the number of elements to display else it will hide it
        /// </summary>
        private void SetupScrollbar(bool isUsingScrollbar)
        {
            if (!isUsingScrollbar)
            {
                if (_scrollbar != null)
                {
                    _scrollbar.gameObject.SetActive(false);
                }
                return;
            }
            if (_elementsToDisplay.Length == 0)
            {
                _scrollbar.gameObject.SetActive(false);
                return;
            }
            _scrollbar.gameObject.SetActive(true);
            _range = MaxScroll - _minScroll;
            ResizeScrollbarHandle();
            _scrollbar.value = _maxScroll;
        }
        /// <summary>
        /// Resize the scrollbar handle to match the number of element to display
        /// </summary>
        private void ResizeScrollbarHandle()
        {
            float scrollsToDo = _elementsToDisplay.Length / _elementDisplayedAtTheSameTime;
            _scrollbar.size = Mathf.Clamp(1 / scrollsToDo, 0, 1);

        }
        /// <summary>
        ///     Call it to simulate a scroll change on the carousel
        ///     so it will move according to it.
        /// </summary>
        /// <param name="scrollPosition">the new scroll position</param>
        public void ChangeCarouselPositionWithoutScrollbar(Vector2 scrollPosition)
        {
            if (_isUsingScrollbar)
            {
                return;
            }
            LoopCarouselAccordingToPosition(scrollPosition.y);
        }
        /// <summary>
        /// Call it to simulate a scroll change on the scrollbar
        /// si it will move the carousel according to the scrollbarposition
        /// </summary>
        /// <param name="scrollbarPosition"></param>
        public void ChangeCarouselPositionWithScrollbar(float scrollbarPosition)
        {
            if (_isUsingScrollbar)
            {
                float convertedScrollPosition = _minScroll + (_range * scrollbarPosition);
                LoopCarouselAccordingToPosition(convertedScrollPosition);
            }
        }

        /// <summary>
        ///     Update the current elements to display
        ///     by setting them to the array passed in parameter
        /// </summary>
        /// <param name="elementsToDisplay">the data elements that will be displayed</param>
        public void SetElementsToDisplay(U[] elementsToDisplay)
        {
            _elementsToDisplay = elementsToDisplay;
        }

        protected void ReplenishDisplayers()
        {
            int minLength = _displayers.Length;
            if (_displayers.Length > (_elementsToDisplay.Length + NUMBER_OF_ELEMENTS_NON_DISPLAYED_ABOVE))
            {
                minLength = _elementsToDisplay.Length + NUMBER_OF_ELEMENTS_NON_DISPLAYED_ABOVE;
            }

            for (int i = NUMBER_OF_ELEMENTS_NON_DISPLAYED_ABOVE; i < minLength; i++)
            {
                _displayers[i].Replenish(_elementsToDisplay[i - NUMBER_OF_ELEMENTS_NON_DISPLAYED_ABOVE]);
            }
            for (int i = minLength; i < _displayers.Length; i++)
            {
                _displayers[i].Replenish(default(U));
            }
        }

        protected void LoopCarouselAccordingToPosition(float yPosition)
        {
            if (yPosition > _maxScroll)
            {
                _scrollDriver.verticalNormalizedPosition = _maxScroll;
                yPosition = _maxScroll;
            }
            else if (yPosition < _minScroll)
            {
                _scrollDriver.verticalNormalizedPosition = _minScroll;
                yPosition = _minScroll;
            }
            SortDisplayersAccordingToPosition(yPosition);
            MoveDisplayersAccordingToPosition(yPosition);
        }

        protected void SortDisplayersAccordingToPosition(float currentYPosition)
        {
            bool isDisappearingAbove;
            bool isDisappearingUnder;

            //while loop beacause, in case of a big scroll difference, we might 
            //have to move the carousel more than one time
            do
            {
                float scrollPositionFirstElementShown = CalculateScrollPositionFromElementPosition(_firstElementShownPosition);
                float scrollPositionSecondElementShown = CalculateScrollPositionFromElementPosition(_firstElementShownPosition + 1);
                isDisappearingAbove = currentYPosition <= scrollPositionSecondElementShown
                    && _firstElementShownPosition < (NUMBER_OF_ELEMENTS_NON_DISPLAYED_ABOVE + _elementsToDisplay.Length - _displayers.Length);
                isDisappearingUnder = currentYPosition > scrollPositionFirstElementShown && _firstElementShownPosition > 0;
                if (isDisappearingAbove)
                {
                    DisappearsAboveViewport();
                }
                else if (isDisappearingUnder)
                {
                    DisappearsUnderViewport();
                }
            } while (isDisappearingAbove || isDisappearingUnder);
        }

        private void DisappearsAboveViewport()
        {
            _firstElementShownPosition++;
            MoveUIElementsUpLooping();
            int positionLastElementDisplayedOnScreen = _firstElementShownPosition + _displayers.Length - 1 - NUMBER_OF_ELEMENTS_NON_DISPLAYED_UNDER;
            U elementToDisplay = default(U);
            if (positionLastElementDisplayedOnScreen < _elementsToDisplay.Length)
            {
                elementToDisplay = _elementsToDisplay[positionLastElementDisplayedOnScreen];
            }
            _displayers[_displayers.Length - 1].Replenish(elementToDisplay);
        }

        private void DisappearsUnderViewport()
        {
            _firstElementShownPosition--;
            MoveUIElementsDownLooping();
            if (_firstElementShownPosition >= NUMBER_OF_ELEMENTS_NON_DISPLAYED_ABOVE)
            {
                _displayers[0].Replenish(_elementsToDisplay[_firstElementShownPosition - NUMBER_OF_ELEMENTS_NON_DISPLAYED_ABOVE]);
            }
        }

        protected void MoveDisplayersAccordingToPosition(float yPosition)
        {
            float elementPosition = CalculateElementPositionFromScrollPosition(yPosition);
            float offsetPercent = CalculateOffsetPercentFromElementPosition(elementPosition);
            for (int i = 0; i < _displayers.Length; i++)
            {
                Vector3 newDisplayerLocalPosition = _displayers[i].RectTransform.localPosition;
                newDisplayerLocalPosition.y = -(i - NUMBER_OF_ELEMENTS_NON_DISPLAYED_ABOVE - offsetPercent) * _height * _spacing;
                _displayers[i].RectTransform.localPosition = newDisplayerLocalPosition;
            }
        }

        private float CalculateOffsetPercentFromElementPosition(float elementPosition)
        {
            float offsetPercent = 0;

            int positionLastElementToShow = (_elementsToDisplay.Length - _displayers.Length) + NUMBER_OF_ELEMENTS_NON_DISPLAYED_ABOVE;
            positionLastElementToShow = Mathf.Max(positionLastElementToShow, 0);

            if (elementPosition > positionLastElementToShow)
            {
                offsetPercent = elementPosition - positionLastElementToShow;
            }
            else if (elementPosition >= 0)
            {
                offsetPercent = Mathf.Repeat(elementPosition, 1.0f);
            }

            return offsetPercent;
        }

        private float CalculateElementPositionFromScrollPosition(float scrollPosition)
        {
            return (_contentHeight * (1 - scrollPosition)) / _height;
        }

        protected float CalculateScrollPositionFromElementPosition(float elementNumber)
        {
            return 1 - _elementHeightScrollPercentage * elementNumber;
        }

        private void MoveUIElementsUpLooping()
        {
            T displayer = _displayers[0];
            for (int i = 1; i < _displayers.Length; i++)
            {
                _displayers[i - 1] = _displayers[i];
            }
            _displayers[_displayers.Length - 1] = displayer;
        }

        private void MoveUIElementsDownLooping()
        {
            T displayer = _displayers[_displayers.Length - 1];
            for (int i = _displayers.Length - 2; i >= 0; i--)
            {
                _displayers[i + 1] = _displayers[i];
            }
            _displayers[0] = displayer;
        }

        public IEnumerator SetVerticalScrollRectToPosition(float position)
        {
            //Need to use a Coroutine to set the position of the scrollDriver else it overwritten its changes.
            yield return null;
            _scrollDriver.normalizedPosition = new Vector2(0, position);
        }

#if UNITY_EDITOR
        public float EDITOR_ONLY_CalculateCurrentHeight()
        {
            float height = 0f;
            foreach (T displayer in _displayers)
            {
                height += Mathf.Abs(displayer.RectTransform.rect.height);
            }
            return height;
        }
#endif
    }
}
