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

        public List<PlayerItem> Items
        {
            get
            {
                return _items;
            }
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

        public bool IsItemConsommable(string itemUuid)
        {
            return GetItem(itemUuid).Consommable;
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

        public void UpdateStockItem()
        {
            if (CurrentItem.Consommable)
            {
                CurrentItem.Stock -= 1;
                if (CurrentItem.Stock < 0)
                {
                    CurrentItem.Stock = 0;
                    CurrentItem = null;
                }
            }
        }

        public List<SaveItem> GetSnapshot()
        {
            List<SaveItem> snapshot = new List<SaveItem>(_items.Count);

            foreach (PlayerItem item in _items)
            {
                string itemName = "";
                if (item.Icon != null)
                {
                    itemName = item.Icon.name;

                }

                snapshot.Add(new SaveItem
                {
                    _uuid = item.Uuid,
                    _unlocked = item.Unlocked,
                    _consommable = item.Consommable,
                    _cost = item.Cost,
                    _icon = itemName,
                    _rarity = item.Rarity,
                    _cooldown = item.Cooldown,
                    _stock = item.Stock
                });
            }

            return snapshot;
        }

        public void Load(List<SaveItem> itemsSnapshot)
        {
            if (itemsSnapshot != null)
            {
                _items = new List<PlayerItem>(itemsSnapshot.Count);

                foreach (SaveItem item in itemsSnapshot)
                {
                    _items.Add(new PlayerItem(item._uuid, item._unlocked, item._consommable, item._cost, item._icon, item._rarity, item._cooldown, item._stock));
                }
            }
        }
    }
}
