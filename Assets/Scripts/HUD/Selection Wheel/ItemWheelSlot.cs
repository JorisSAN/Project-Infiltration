using player.item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace hud.selectionwheel
{
    public class ItemWheelSlot : SelectionWheelSlot
    {
        public PlayerItem PlayerItem
        {
            get; private set;
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
