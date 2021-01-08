using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace carousel
{
    [ExecuteInEditMode]
    public class HorizontalSlotList<T, U> : MonoBehaviour, IEndDragHandler, IBeginDragHandler, IDragHandler where T : ASlotListElementDisplayer<U>
    {
        [SerializeField] protected T[] _displayers = default(T[]);
        [SerializeField] private GameObject _slotsContainer = default;
        [SerializeField] protected GameObject[] _slotsGO = default;
        [SerializeField] protected GameObject _leftWaitingSlotGO = default;
        [SerializeField] protected GameObject _rightWaitingSlotGO = default;
        private List<Slot<T, U>> _slots = new List<Slot<T, U>>();
        private Slot<T, U> _leftWaitingSlot = default;
        private Slot<T, U> _rightWaitingSlot = default;
        private U[] _elementsToDisplay = new U[0];
        private float _startDragPosition = default;
        private bool _hasPointerDown = false;
        private int _firstElementShownPosition = default;
        [SerializeField] private int _startingSlot = default; //work with the slot 0 and 1 (for list with 3 slots it will be the left and middle one) wont work with the rest
        [SerializeField] protected int _startingElementPosition = default;
        private const int NUMBER_OF_ELEMENTS_NON_DISPLAYED_LEFT = 1;
        private const int NUMBER_OF_ELEMENTS_NON_DISPLAYED_RIGHT = 1;
        internal enum Direction { LEFT = 1, RIGHT = -1 };
        /// <summary>
        /// Need to call the setup once the gameObject is active else the canvas will not be set, all transform will be equals to zero and the slots wont place the displayer at the right position
        /// </summary>
        private void OnEnable()
        {
            ReplenishDisplayers();
            SetupCarousel();
        }
        public void SetupCarousel()
        {
            gameObject.SetActive(true);
            GetSlotsComponents();
            SetValues();
            _firstElementShownPosition = -_startingSlot;
            FillCarousel(_startingSlot, _startingElementPosition);

        }
        private void GetSlotsComponents()
        {
            _slots.Clear();
            foreach (GameObject slotGO in _slotsGO)
            {
                _slots.Add(slotGO.GetComponent<Slot<T, U>>());
            }
            _leftWaitingSlot = _leftWaitingSlotGO.GetComponent<Slot<T, U>>();
            _rightWaitingSlot = _rightWaitingSlotGO.GetComponent<Slot<T, U>>();
        }
        protected virtual void SetValues()
        {
            _firstElementShownPosition = 0;
            _slotsContainer.transform.SetAsLastSibling();       //To ensure the buttons on the slots wont be covered by the displayers
            foreach (Slot<T, U> slot in _slots)
            {
                slot.Displayer.Clear();
            }
            _rightWaitingSlot.Displayer.Clear();
            _leftWaitingSlot.Displayer.Clear();
        }
        private void FillCarousel(int startingSlot, int startingElementNumber)
        {
            for (int i = 0; i < _slots.Count - startingSlot; i++)
            {
                _slots[i + startingSlot].AddDisplayer(_displayers[i + 1]);
            }
            _leftWaitingSlot.AddDisplayer(_displayers[0]);

            for (int j = _slots.Count - startingSlot; j < _slots.Count; j++)
            {
                _rightWaitingSlot.AddDisplayer(_displayers[j + 1]);
            }
            _rightWaitingSlot.AddDisplayer(_displayers[_displayers.Length - 1]);
            for (int i = 0; i < startingElementNumber; i++)
            {
                LoopCarouselAccordingToDirection(1, false);
            }
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
            if (_hasPointerDown)
            {
                return;
            }
            if (_startDragPosition == scrollPosition.x)
            {
                return;
            }
            _hasPointerDown = true;
            int direction;
            if (scrollPosition.x > _startDragPosition)
            {
                direction = -1;
            }
            else
            {
                direction = 1;
            }
            bool isDisplayersMoving = CheckIsDisplayersMoving();
            if (isDisplayersMoving)
            {
                return;
            }
            LoopCarouselAccordingToDirection(direction);
        }

        private bool CheckIsDisplayersMoving()
        {
            bool isDisplayersMoving = false;
            foreach (Slot<T, U> slot in _slots)
            {
                isDisplayersMoving |= slot.isAnimating;
            }
            isDisplayersMoving |= _leftWaitingSlot.isAnimating;
            isDisplayersMoving |= _rightWaitingSlot.isAnimating;
            return isDisplayersMoving;
        }

        protected void LoopCarouselAccordingToDirection(int direction, bool isAnimated = true)
        {
            if (_firstElementShownPosition + direction < -_startingSlot
               || _firstElementShownPosition + direction > _elementsToDisplay.Length - _slots.Count + _startingSlot)
            {
                return;
            }
            if (direction == 1)
            {
                _firstElementShownPosition++;
            }
            else
            {
                _firstElementShownPosition--;
            }
            SortDisplayersAccordingToDirection(direction);
            MoveDisplayersAccordingToDirection(direction, isAnimated);
        }
        protected void SortDisplayersAccordingToDirection(float direction)
        {
            bool isDisappearingLeft;
            bool isDisappearingRight;


            isDisappearingLeft = direction == 1 && _slots[0].GetComponent<Slot<T, U>>().Displayer.Count > 0;

            isDisappearingRight = direction == -1 && _slots[_slots.Count - 1].GetComponent<Slot<T, U>>().Displayer.Count > 0;

            if (isDisappearingLeft)
            {
                DisappearsLeftViewport();
            }
            else if (isDisappearingRight)
            {
                DisappearsRightViewport();
            }
        }

        private void DisappearsLeftViewport()
        {
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
            MoveUIElementsRightLooping();

            U elementToDisplay = default(U);
            if (_firstElementShownPosition - NUMBER_OF_ELEMENTS_NON_DISPLAYED_LEFT >= 0)
            {
                elementToDisplay = _elementsToDisplay[_firstElementShownPosition - NUMBER_OF_ELEMENTS_NON_DISPLAYED_LEFT];
                _displayers[0].Replenish(elementToDisplay);
            }
            else
            {
                for (int i = 0; i <= -_firstElementShownPosition; i++)
                {
                    _displayers[i].Replenish(elementToDisplay);
                }
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

        protected void MoveDisplayersAccordingToDirection(int direction, bool isAnimated)
        {
            if (direction == (int)Direction.LEFT)
            {
                if (isAnimated)
                {
                    MoveDisplayersToTheLeftWithAnimation();
                }
                else
                {
                    MoveDisplayersToTheLeftWithoutAnimation();
                }
            }
            else if (direction == (int)Direction.RIGHT)
            {
                MoveDisplayersToTheRight();
            }
        }
        private void MoveDisplayersToTheLeftWithoutAnimation()
        {
            if (_slots[0].Displayer.Count > 0)
            {
                _leftWaitingSlot.ChangeDisplayerOfSlotWithoutAnimation(_slots[0]);
            }
            for (int i = 1; i < _slots.Count; i++)
            {
                if (_slots[i].Displayer.Count > 0)
                {
                    _slots[i - 1].ChangeDisplayerOfSlotWithoutAnimation(_slots[i]);
                }
            }
            if (_leftWaitingSlot.Displayer.Count > 1)
            {
                _rightWaitingSlot.ChangeDisplayerOfSlotWithoutAnimation(_leftWaitingSlot);
            }
            if (_rightWaitingSlot.Displayer.Count > 1)
            {
                _slots[_slots.Count - 1].ChangeDisplayerOfSlotWithoutAnimation(_rightWaitingSlot);
            }
        }
        private void MoveDisplayersToTheLeftWithAnimation()
        {
            if (_slots[0].Displayer.Count > 0)
            {
                _leftWaitingSlot.ChangeDisplayerOfSlotWithAnimation(_slots[0]);
            }
            for (int i = 1; i < _slots.Count; i++)
            {
                if (_slots[i].Displayer.Count > 0)
                {
                    _slots[i - 1].ChangeDisplayerOfSlotWithAnimation(_slots[i]);
                }
            }
            if (_leftWaitingSlot.Displayer.Count > 1)
            {
                _rightWaitingSlot.ChangeDisplayerOfSlotWithoutAnimation(_leftWaitingSlot);
            }
            if (_rightWaitingSlot.Displayer.Count > 1)
            {
                _slots[_slots.Count - 1].ChangeDisplayerOfSlotWithAnimation(_rightWaitingSlot);
            }
        }
        private void MoveDisplayersToTheRight()
        {
            if (_slots[_slots.Count - 1].Displayer.Count > 0)
            {
                _rightWaitingSlot.ChangeDisplayerOfSlotWithAnimation(_slots[_slots.Count - 1]);
            }
            for (int i = _slots.Count - 1; i > 0; i--)
            {
                if (_slots[i - 1].Displayer.Count > 0)
                {
                    _slots[i].ChangeDisplayerOfSlotWithAnimation(_slots[i - 1]);
                }
            }
            if (_rightWaitingSlot.Displayer.Count > 1)
            {
                _leftWaitingSlot.ChangeDisplayerOfSlotWithoutAnimation(_rightWaitingSlot);
            }
            if (_leftWaitingSlot.Displayer.Count > 1)
            {
                _slots[0].ChangeDisplayerOfSlotWithAnimation(_leftWaitingSlot);
            }
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            _hasPointerDown = false;
        }
        /// <summary>
        /// Needed for OnPointerUp to be called
        /// </summary>
        public void OnPointerDown(PointerEventData eventData)
        {

        }
        public void MoveToTheRight()
        {
            bool isDisplayersMoving = CheckIsDisplayersMoving();
            if (isDisplayersMoving)
            {
                return;
            }
            LoopCarouselAccordingToDirection(1);
        }
        public void MoveToTheLeft()
        {
            bool isDisplayersMoving = CheckIsDisplayersMoving();
            if (isDisplayersMoving)
            {
                return;
            }
            LoopCarouselAccordingToDirection(-1);
        }
        public virtual void OpenMiddle()
        {
            Debug.LogWarning("Middle displayer clicked");
        }



        public void OnEndDrag(PointerEventData eventData)
        {
            _hasPointerDown = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            ChangeCarouselPositionWithoutScrollbar(eventData.position);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _startDragPosition = eventData.position.x;
        }
    }
}