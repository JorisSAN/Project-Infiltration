using player.item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace hud.itemwheel
{
    public class ItemWheelSlot : MonoBehaviour
    {
        [SerializeField] private Color _hoverColor = default;
        [SerializeField] private Color _baseColor = default;
        [SerializeField] private Image _background = default;
        [SerializeField] private Image _itemIcon = default;

        private bool _canBeSelected = false;

        public PlayerItem PlayerItem
        {
            get; private set;
        }

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

        public void Load(PlayerItem item)
        {
            PlayerItem = item;
            _canBeSelected = true;
            _itemIcon.gameObject.SetActive(true);
            _itemIcon.sprite = PlayerItem.Icon;
        }

        public void SetDefault()
        {
            PlayerItem = null;
            _canBeSelected = false;
            _itemIcon.gameObject.SetActive(false);
        }
    }
}
