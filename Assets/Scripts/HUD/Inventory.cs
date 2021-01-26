using player.item;
using player.skill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hud
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private HudSkillSlot _skillSlot = default;
        [SerializeField] private HudItemSlot _itemSlot = default;

        public void SelectSkill(PlayerSkill skill)
        {
            if (skill is null)
            {
                _skillSlot.LoadSkill(null);
                _skillSlot.LoadImage(null);
                return;
            }
            _skillSlot.LoadSkill(skill);
            _skillSlot.LoadImage(skill.Icon);
        }

        public void UseSkill()
        {
            // Start timer for the next possible use
            _skillSlot.StartCooldown();
        }

        public bool CanUseSkill()
        {
            return !_skillSlot.IsCooldown;
        }

        public void SelectItem(PlayerItem item)
        {
            if (item is null)
            {
                _itemSlot.LoadImage(null);
                return;
            }
            _itemSlot.LoadImage(item.Icon);
        }

        public void UseItem()
        {
            // Start timer for the next possible use
        }
    }
}
