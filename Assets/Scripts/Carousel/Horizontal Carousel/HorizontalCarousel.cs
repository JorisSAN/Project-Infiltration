using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using utils;

namespace carousel
{
    /// <summary>
    ///     Carousel for a horizontal scroll :
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
    public class HorizontalCarousel<T, U> : MonoBehaviour where T : ACarouselElementDisplayer<U>
    {
        [SerializeField] protected T[] _displayers = default(T[]);
        public U[] _elementsToDisplay = new U[0];
        [SerializeField] protected bool _isUsingScrollbar = default;
        [SerializeField] protected Scrollbar _scrollbar = default;
        [SerializeField] protected ScrollRect _scrollDriver = default(ScrollRect);
        [SerializeField] protected CoroutineStarter _coroutineStarter;

        protected float _displayerWidth;
        protected float _minScroll;
        protected float _maxScroll;
        protected float _currentScroll;
        protected float _range;
        protected float _elementDisplayedAtTheSameTime;
        protected int _ceilOfElementDisplayedAtTheSameTime;
        protected int _floorOfElementDisplayedAtTheSameTime;
        protected bool _ready;
        protected float _spacing = 1f;
        protected float _startScrollPosition = 1f;
        protected float StartPositionOffsetPercent { get; set; } = 0f;
        protected float _startPositionOffset;
        protected float _contentWidth;
        public float MaxScroll
        {
            get
            {
                return _maxScroll;
            }
        }
        public float MinScroll
        {
            get
            {
                return _minScroll;
            }
        }
        protected float ElementDisplayedAtTheSameTime
        {
            get
            {
                return _elementDisplayedAtTheSameTime;
            }
        }
        protected int _firstElementShownPosition;
        private float _elementWidthScrollPercentage;
        protected const int NUMBER_OF_ELEMENTS_NON_DISPLAYED_LEFT = 3;
        protected const int NUMBER_OF_ELEMENTS_NON_DISPLAYED_RIGHT = 3;

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
            _coroutineStarter = CoroutineStarter.Instance;
            _coroutineStarter.StartCoroutine(SetHorizontalScrollRectToPosition(_minScroll));
            LoopCarouselAccordingToPosition(_minScroll);
            gameObject.SetActive(true);
        }
        protected virtual void SetValues()
        {
            _scrollDriver.normalizedPosition = new Vector2(1, 0);
            _displayerWidth = _displayers[0].RectTransform.rect.width;
            _contentWidth = (_displayers.Length * _displayerWidth) - _scrollDriver.viewport.rect.width;
            _elementWidthScrollPercentage = _displayerWidth / _contentWidth;
            _firstElementShownPosition = 0;
            _maxScroll = 1f;
            _startPositionOffset = _elementWidthScrollPercentage * StartPositionOffsetPercent;
            _minScroll = 1f;
            _startScrollPosition = 1f;
            _elementDisplayedAtTheSameTime = _scrollDriver.viewport.rect.width / (_displayerWidth * _spacing);
            _ceilOfElementDisplayedAtTheSameTime = Mathf.CeilToInt(_elementDisplayedAtTheSameTime);
            _floorOfElementDisplayedAtTheSameTime = Mathf.FloorToInt(_elementDisplayedAtTheSameTime);
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
            _scrollbar.value = 0;
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
        /// If there's enough space to display all the elements without the need of scrolling, we need to fake more element so the elements dont align on the right but stay aligned on the left
        /// </summary>
        protected void SetMinScroll()
        {
            if (_elementsToDisplay.Length >= (_displayers.Length - NUMBER_OF_ELEMENTS_NON_DISPLAYED_LEFT - NUMBER_OF_ELEMENTS_NON_DISPLAYED_RIGHT))
            {

                _minScroll = CalculateScrollPositionFromElementPosition(_elementsToDisplay.Length - _elementDisplayedAtTheSameTime);
            }
            else if (_elementsToDisplay.Length > 0)
            {
                _minScroll = CalculateScrollPositionFromElementPosition(Mathf.Ceil(_elementDisplayedAtTheSameTime) - _elementDisplayedAtTheSameTime);
            }

        }
        /// <summary>
        /// Need to use a Coroutine to set the position of the scrollDriver else it overwritten its changes.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        protected IEnumerator SetHorizontalScrollRectToPosition(float position)
        {

            yield return null;
            _scrollDriver.normalizedPosition = new Vector2(position, 0);
        }

        protected void ReplenishDisplayers()
        {
            int minLength = _displayers.Length;
            if (_displayers.Length > (_elementsToDisplay.Length + NUMBER_OF_ELEMENTS_NON_DISPLAYED_LEFT))
            {
                minLength = _elementsToDisplay.Length + NUMBER_OF_ELEMENTS_NON_DISPLAYED_LEFT;
            }

            for (int i = NUMBER_OF_ELEMENTS_NON_DISPLAYED_LEFT; i < minLength; i++)
            {
                _displayers[i].Replenish(_elementsToDisplay[i - NUMBER_OF_ELEMENTS_NON_DISPLAYED_LEFT]);
                _displayers[i].transform.SetSiblingIndex(i - NUMBER_OF_ELEMENTS_NON_DISPLAYED_LEFT);
            }
            for (int i = minLength; i < _displayers.Length; i++)
            {
                _displayers[i].Replenish(default(U));
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
            ReplenishDisplayers();
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
            _currentScroll = scrollPosition.x;
            LoopCarouselAccordingToPosition(scrollPosition.x);
        }
        /// <summary>
        /// Call it to simulate a scroll change on the scrollbar
        /// si it will move the carousel according to the scrollbarposition
        /// </summary>
        /// <param name="scrollbarPosition"></param>
        public virtual void ChangeCarouselPositionWithScrollbar(float scrollbarPosition)
        {
            if (_isUsingScrollbar)
            {
                float convertedScrollPosition = _minScroll + (_range * scrollbarPosition);
                LoopCarouselAccordingToPosition(convertedScrollPosition);
            }
        }
        protected void LoopCarouselAccordingToPosition(float xPosition)
        {
            if (xPosition > _maxScroll)
            {
                _scrollDriver.horizontalNormalizedPosition = _maxScroll;
                xPosition = _maxScroll;
            }
            else if (xPosition < _minScroll)
            {
                _scrollDriver.horizontalNormalizedPosition = _minScroll;
                xPosition = _minScroll;
            }
            SortDisplayersAccordingToPosition(xPosition);
            MoveDisplayersAccordingToPosition(xPosition);
        }
        protected void SortDisplayersAccordingToPosition(float currentXPosition)
        {
            bool isDisappearingLeft;
            bool isDisappearingRight;

            //while loop because, in case of a big scroll difference, we might 
            //have to move the carousel more than one time
            do
            {
                float scrollPositionFirstElementShown = CalculateScrollPositionFromElementPosition(_firstElementShownPosition);
                float scrollPositionSecondElementShown = CalculateScrollPositionFromElementPosition(_firstElementShownPosition + 1);
                isDisappearingLeft = currentXPosition <= scrollPositionSecondElementShown
                    && _firstElementShownPosition < (NUMBER_OF_ELEMENTS_NON_DISPLAYED_LEFT + _elementsToDisplay.Length - _displayers.Length);
                isDisappearingRight = currentXPosition > scrollPositionFirstElementShown && _firstElementShownPosition > 0;
                if (isDisappearingLeft)
                {
                    DisappearsLeftViewport();
                }
                else if (isDisappearingRight)
                {
                    DisappearsRightViewport();
                }
            } while (isDisappearingLeft || isDisappearingRight);
        }

        private void DisappearsLeftViewport()
        {
            _firstElementShownPosition++;
            MoveUIElementsLeftLooping();
            int positionLastElementDisplayedOnScreen = _firstElementShownPosition + _displayers.Length - 1 - NUMBER_OF_ELEMENTS_NON_DISPLAYED_RIGHT;
            U elementToDisplay = default(U);
            if (positionLastElementDisplayedOnScreen < _elementsToDisplay.Length)
            {
                elementToDisplay = _elementsToDisplay[positionLastElementDisplayedOnScreen];
            }
            _displayers[_displayers.Length - 1].Replenish(elementToDisplay);
        }
        private void MoveUIElementsLeftLooping()
        {
            T displayer = _displayers[0];
            for (int i = 1; i < _displayers.Length; i++)
            {
                _displayers[i - 1] = _displayers[i];
            }
            _displayers[_displayers.Length - 1] = displayer;
        }
        private void DisappearsRightViewport()
        {
            _firstElementShownPosition--;
            MoveUIElementsRightLooping();
            if (_firstElementShownPosition >= NUMBER_OF_ELEMENTS_NON_DISPLAYED_LEFT)
            {
                _displayers[0].Replenish(_elementsToDisplay[_firstElementShownPosition - NUMBER_OF_ELEMENTS_NON_DISPLAYED_LEFT]);
            }
        }
        private void MoveUIElementsRightLooping()
        {
            T displayer = _displayers[_displayers.Length - 1];
            for (int i = _displayers.Length - 2; i >= 0; i--)
            {
                _displayers[i + 1] = _displayers[i];
            }
            _displayers[0] = displayer;
        }
        protected void MoveDisplayersAccordingToPosition(float xPosition)
        {
            float elementPosition = CalculateElementPositionFromScrollPosition(xPosition);
            float offsetPercent = CalculateOffsetPercentFromElementPosition(elementPosition);
            if (_elementsToDisplay.Length > _elementDisplayedAtTheSameTime)
            {

                for (int i = 0; i < _displayers.Length; i++)
                {
                    Vector3 newDisplayerLocalPosition = _displayers[i].RectTransform.localPosition;
                    newDisplayerLocalPosition.x = -(i - NUMBER_OF_ELEMENTS_NON_DISPLAYED_LEFT - offsetPercent) * _displayerWidth * _spacing;
                    _displayers[i].RectTransform.localPosition = newDisplayerLocalPosition;
                }
            }
            //If there's less elements to display than the number of elements we can display at the same time without scrolling we will correctely align the elements of the left and hide the displayers at the fake positions we created earlier
            else
            {
                for (int i = 0; i < _displayers.Length; i++)
                {
                    Vector3 newDisplayerLocalPosition = _displayers[i].RectTransform.localPosition;
                    float fakeElements = (_ceilOfElementDisplayedAtTheSameTime - _elementsToDisplay.Length);
                    newDisplayerLocalPosition.x = -(i + fakeElements - NUMBER_OF_ELEMENTS_NON_DISPLAYED_LEFT - offsetPercent) * _displayerWidth * _spacing;
                    _displayers[i].RectTransform.localPosition = newDisplayerLocalPosition;
                }
                for (int i = 0; i < NUMBER_OF_ELEMENTS_NON_DISPLAYED_LEFT; i++)
                {
                    _displayers[i].HideDisplayer();
                }
                for (int i = NUMBER_OF_ELEMENTS_NON_DISPLAYED_LEFT + _ceilOfElementDisplayedAtTheSameTime; i < _displayers.Length; i++)
                {
                    _displayers[i].HideDisplayer();
                }
            }
        }
        private float CalculateElementPositionFromScrollPosition(float scrollPosition)
        {
            return (_contentWidth * (1 - scrollPosition)) / _displayerWidth;
        }
        private float CalculateOffsetPercentFromElementPosition(float elementPosition)
        {
            float offsetPercent = 0;

            int positionLastElementToShow = (_elementsToDisplay.Length - _displayers.Length) + NUMBER_OF_ELEMENTS_NON_DISPLAYED_LEFT;
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
        protected float CalculateScrollPositionFromElementPosition(float elementNumber)
        {
            return 1 - _elementWidthScrollPercentage * elementNumber;
        }

        



#if UNITY_EDITOR
        public float EDITOR_ONLY_CalculateCurrentWidth()
        {
            float width = 0f;
            foreach (T displayer in _displayers)
            {
                width += Mathf.Abs(displayer.RectTransform.rect.width);
            }
            return width;
        }
#endif
    }
}
