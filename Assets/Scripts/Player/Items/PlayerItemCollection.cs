using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player.item
{
    public class PlayerItemCollection
    {
        private List<PlayerItem> _items = new List<PlayerItem>();

        public PlayerItem CurrentItem
        {
            get; set;
        }

        public void SelectItem(string itemUuid)
        {
            if (ContainItem(itemUuid) && IsItemUnlocked(itemUuid))
            {
                Debug.Log("Item " + itemUuid + " selected !");
                CurrentItem = GetItem(itemUuid);
            }
        }

        public bool ContainItem(string itemUuid)
        {
            foreach (PlayerItem item in _items)
            {
                if (item.Uuid.Equals(itemUuid))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsItemUnlocked(string itemUuid)
        {
            return GetItem(itemUuid).Unlocked;
        }

        public PlayerItem GetItem(string itemUuid)
        {
            foreach (PlayerItem item in _items)
            {
                if (item.Uuid.Equals(itemUuid))
                {
                    return item;
                }
            }
            return null;
        }

        public void Load()
        {
            // Fake load for the moment
            Debug.Log("Load items !");
            _items.Add(new PlayerItem("Item_1", true, "pistol-gun"));
            _items.Add(new PlayerItem("Item_2", false, "crossbow"));
        }
    }
}
