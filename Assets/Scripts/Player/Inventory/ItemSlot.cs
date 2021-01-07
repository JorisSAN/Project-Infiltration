using player.inventory;
using player.item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player.inventory
{
    public class ItemSlot : IInventorySlot
    {
        private PlayerItem _item;

        public void LoadObject(PlayerItem item)
        {
            if (_item == null)
            {
                _item = item;
                return;
            }

            if (!_item.Uuid.Equals(item.Uuid))
            {
                _item = item;
            }
        }

        public void Use()
        {
            if (!(_item is null))
            {
                ItemManager.Instance.UseItem(_item.Uuid);
            }
        }
    }
}
