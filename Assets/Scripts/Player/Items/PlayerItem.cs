using game.manager;
using itemshop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player.item
{
    public class PlayerItem
    {
        public string Uuid { get; }

        public bool Unlocked { get; }

        public bool Consommable { get; }

        public int Cost { get; }

        public Rarity Rarity { get; }

        public Sprite Icon { get; }

        public int Stock { get; set; }

        public PlayerItem(string uuid, bool unlocked)
        {
            Uuid = uuid;
            Unlocked = unlocked;
        }

        public PlayerItem(string uuid, bool unlocked, string iconName)
        {
            Uuid = uuid;
            Unlocked = unlocked;
            Icon = ConvertStringToSprite(iconName);
        }

        public PlayerItem(string uuid, bool unlocked, bool consommable, int cost, string iconName, Rarity rarity, int stock)
        {
            Uuid = uuid;
            Unlocked = unlocked;
            Consommable = consommable;
            Cost = cost;
            Icon = ConvertStringToSprite(iconName);
            Rarity = rarity;
            Stock = stock;
        }

        public Sprite ConvertStringToSprite(string spriteName)
        {
            return SpriteManager.Instance.GetSprite(spriteName);
        }
    }
}
