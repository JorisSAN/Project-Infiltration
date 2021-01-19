using player.item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hud.itemwheel
{
    public class ItemWheel : MonoBehaviour
    {
        private Vector2 _normalizedMousePosition;
        private float _currentAngle;
        private int _currentSelectedSlot = -1;
        private int _previousSelectedSlot = -1;

        [SerializeField] private ItemWheelSlot[] _slots = default;
        [SerializeField] private Hud _hud = default;

        public ItemWheelSlot CurrentSlot
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

        public ItemWheelSlot PreviousSlot
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
                if (CurrentSlot != null && CurrentSlot.PlayerItem != null)
                {
                    _hud.SelectItem(CurrentSlot.PlayerItem.Uuid);
                }
            }
        }

        public void Show()
        {
            this.gameObject.SetActive(true);
        }

        public void Hide()
        {
            this.gameObject.SetActive(false);
        }

        public void LoadWheel(List<PlayerItem> items)
        {
            int index = 0;
            foreach (PlayerItem item in items)
            {
                if (index < _slots.Length)
                {
                    _slots[index].Load(item);
                }
                index++;
            }

            for (int i=index; i<_slots.Length; i++)
            {
                _slots[i].SetDefault();
            }
        }
    }
}
