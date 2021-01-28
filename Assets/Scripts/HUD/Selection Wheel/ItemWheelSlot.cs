using player.item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace hud.selectionwheel
{
    public class ItemWheelSlot : SelectionWheelSlot
    {
        [SerializeField] private Text _stock = default;

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

            SetStock();
        }

        public void SetDefault()
        {
            PlayerItem = null;
            _canBeSelected = false;
            _itemIcon.gameObject.SetActive(false);
            _stock.gameObject.SetActive(false);
        }

        public void SetStock()
        {
            _stock.gameObject.SetActive(false);
            if (PlayerItem != null && PlayerItem.Consommable)
            {
                if (PlayerItem.Stock > 0)
                {
                    _stock.text = PlayerItem.Stock.ToString();
                    _stock.gameObject.SetActive(true);
                }
                else
                {
                    SetDefault();
                }
            }
        }
    }
}
