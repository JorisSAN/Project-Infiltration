using game.manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player.item
{
    public class PlayerItem
    {
        public string Uuid { get; }

        public bool Unlocked { get; }

        public Sprite Icon { get; }

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

        public Sprite ConvertStringToSprite(string spriteName)
        {
            return SpriteManager.Instance.GetSprite(spriteName);
        }
    }
}
