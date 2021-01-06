using player.skill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player.inventory
{
    public class PlayerInventory
    {
        private SkillSlot _skillSlot;

        public void Initialize()
        {
            _skillSlot = new SkillSlot();
        }

        public void SelectSkill(PlayerSkill skill)
        {
            _skillSlot.LoadSkill(skill);
        }

        public void UseSkill()
        {
            _skillSlot.Use();
        }
    }
}
