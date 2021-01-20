using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hud.selectionwheel
{
    public abstract class SelectionWheel : MonoBehaviour
    {
        protected Vector2 _normalizedMousePosition;
        protected float _currentAngle;
        protected int _currentSelectedSlot = -1;
        protected int _previousSelectedSlot = -1;

        [SerializeField] protected SelectionWheelSlot[] _slots = default;
        [SerializeField] protected Hud _hud = default;

        public SelectionWheelSlot CurrentSlot
        {
            get
            {
                if (_currentSelectedSlot >= 0 && _currentSelectedSlot < _slots.Length)
                {
                    return _slots[_currentSelectedSlot];
                }
                return null;
            }
        }

        public SelectionWheelSlot PreviousSlot
        {
            get
            {
                if (_previousSelectedSlot >= 0 && _previousSelectedSlot < _slots.Length)
                {
                    return _slots[_previousSelectedSlot];
                }
                return null;
            }
        }

        public void Start()
        {
            Hide();
        }

        public void Update()
        {
            _normalizedMousePosition = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2);
            _currentAngle = Mathf.Atan2(_normalizedMousePosition.y, _normalizedMousePosition.x) * Mathf.Rad2Deg;
            _currentAngle = (_currentAngle + 360) % 360;

            _currentSelectedSlot = (int)_currentAngle / 45;

            if (_currentSelectedSlot != _previousSelectedSlot)
            {
                if (PreviousSlot != null)
                {
                    PreviousSlot.UnselectItem();
                }

                _previousSelectedSlot = _currentSelectedSlot;

                if (CurrentSlot != null)
                {
                    CurrentSlot.SelectItem();
                }

            }

            /* Select item */
            if (Input.GetMouseButtonDown(0)) // LEFT CLIC
            {
                SelectSlot();
            }
        }

        public abstract void SelectSlot();

        public void Show()
        {
            this.gameObject.SetActive(true);
        }

        public void Hide()
        {
            this.gameObject.SetActive(false);
        }
    }
}
