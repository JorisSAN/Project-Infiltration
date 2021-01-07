using player.item;
using player.skill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player.inventory
{
    public class PlayerInventory
    {
        private SkillSlot _skillSlot;
        private ItemSlot _itemSlot;

        public void Initialize()
        {
            _skillSlot = new SkillSlot();
            _itemSlot = new ItemSlot();
        }

        public void SelectSkill(PlayerSkill skill)
        {
            _skillSlot.LoadSkill(skill);
        }

        public void UseSkill()
        {
            _skillSlot.Use();
        }

        public void SelectItem(PlayerItem item)
        {
            _itemSlot.LoadObject(item);
        }

        public void UseItem()
        {
            _itemSlot.Use();
        }
    }
}
