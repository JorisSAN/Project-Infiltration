using player.obj;
using player.skill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player.inventory
{
    public class PlayerInventory
    {
        private SkillSlot _skillSlot;
        private ObjectSlot _objectSlot;

        public void Initialize()
        {
            _skillSlot = new SkillSlot();
            _objectSlot = new ObjectSlot();
        }

        public void SelectSkill(PlayerSkill skill)
        {
            _skillSlot.LoadSkill(skill);
        }

        public void UseSkill()
        {
            _skillSlot.Use();
        }

        public void SelectObject(PlayerObject obj)
        {
            _objectSlot.LoadObject(obj);
        }

        public void UseObject()
        {
            _objectSlot.Use();
        }
    }
}
