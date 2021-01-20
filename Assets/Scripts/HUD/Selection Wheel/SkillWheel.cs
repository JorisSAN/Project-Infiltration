using player.skill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hud.selectionwheel
{
    public class SkillWheel : SelectionWheel
    {
        public new SkillWheelSlot CurrentSlot
        {
            get
            {
                if (_currentSelectedSlot >= 0 && _currentSelectedSlot < _slots.Length)
                {
                    return (SkillWheelSlot)_slots[_currentSelectedSlot];
                }
                return null;
            }
        }

        public new SkillWheelSlot PreviousSlot
        {
            get
            {
                if (_previousSelectedSlot >= 0 && _previousSelectedSlot < _slots.Length)
                {
                    return (SkillWheelSlot)_slots[_previousSelectedSlot];
                }
                return null;
            }
        }

        public void LoadWheel(List<PlayerSkill> skills)
        {
            int index = 0;
            foreach (PlayerSkill skill in skills)
            {
                if (index < _slots.Length)
                {
                    ((SkillWheelSlot)_slots[index]).Load(skill);
                }
                index++;
            }

            for (int i = index; i < _slots.Length; i++)
            {
                ((SkillWheelSlot)_slots[i]).SetDefault();
            }
        }

        public override void SelectSlot()
        {
            if (CurrentSlot != null && CurrentSlot.PlayerSkill != null)
            {
                _hud.SelectSkill(CurrentSlot.PlayerSkill.Uuid);
            }
        }
    }
}
