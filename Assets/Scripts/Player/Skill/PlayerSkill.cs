using game.manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player.skill
{
    public class PlayerSkill
    {
        public string Uuid { get; }

        public bool Unlocked { get; }

        public bool Usable { get; }

        public Sprite Icon { get; }

        public PlayerSkill(string uuid, bool unlocked, bool usable)
        {
            Uuid = uuid;
            Unlocked = unlocked;
            Usable = usable;
        }

        public PlayerSkill(string uuid, bool unlocked, bool usable, string iconName)
        {
            Uuid = uuid;
            Unlocked = unlocked;
            Usable = usable;
            Icon = ConvertStringToSprite(iconName);
        }

        public Sprite ConvertStringToSprite(string spriteName)
        {
            return SpriteManager.Instance.GetSprite(spriteName);
        }
    }
}
