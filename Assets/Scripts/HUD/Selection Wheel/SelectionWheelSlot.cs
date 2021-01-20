using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace hud.selectionwheel
{
    public abstract class SelectionWheelSlot : MonoBehaviour
    {
        [SerializeField] protected Color _hoverColor = default;
        [SerializeField] protected Color _baseColor = default;
        [SerializeField] protected Image _background = default;
        [SerializeField] protected Image _itemIcon = default;

        protected bool _canBeSelected = false;

        public void Start()
        {
            _background.color = _baseColor;
        }

        public void SelectItem()
        {
            if (_canBeSelected)
            {
                _background.color = _hoverColor;
            }
        }

        public void UnselectItem()
        {
            _background.color = _baseColor;
        }
    }
}
