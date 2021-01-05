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

        public PlayerSkill(string uuid, bool unlocked, bool usable)
        {
            Uuid = uuid;
            Unlocked = unlocked;
            Usable = usable;
        }
    }
}
