using itemshop.save;
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

        public void Load(List<SaveItem> itemsSnapshot)
        {
            if (itemsSnapshot != null)
            {
                _items = new List<PlayerItem>(itemsSnapshot.Count);

                foreach (SaveItem item in itemsSnapshot)
                {
                    _items.Add(new PlayerItem(item._uuid, item._unlocked, item._cost, item._icon, item._rarity));
                }
            }
        }
    }
}
