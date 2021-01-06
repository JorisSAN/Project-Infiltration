using game.manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player.obj
{
    public class PlayerObject
    {
        public string Uuid { get; }

        public bool Unlocked { get; }

        public Sprite Icon { get; }

        public PlayerObject(string uuid, bool unlocked)
        {
            Uuid = uuid;
            Unlocked = unlocked;
        }

        public PlayerObject(string uuid, bool unlocked, string iconName)
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
