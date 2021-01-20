using player.skill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hud.selectionwheel
{
    public class SkillWheelSlot : SelectionWheelSlot
    {
        public PlayerSkill PlayerSkill
        {
            get; private set;
        }

        public void Load(PlayerSkill skill)
        {
            PlayerSkill = skill;
            _canBeSelected = true;
            _itemIcon.gameObject.SetActive(true);
            _itemIcon.sprite = PlayerSkill.Icon;
        }

        public void SetDefault()
        {
            PlayerSkill = null;
            _canBeSelected = false;
            _itemIcon.gameObject.SetActive(false);
        }
    }
}
