using player.skill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player.inventory
{
    public class SkillSlot : IInventorySlot
    {
        private PlayerSkill _skill;

        public void LoadSkill(PlayerSkill skill)
        {
            if (_skill == null)
            {
                _skill = skill;
                return;
            }

            if (!_skill.Uuid.Equals(skill.Uuid))
            {
                _skill = skill;
            }
        }

        public void Use()
        {
            if (!(_skill is null) && _skill.Usable)
            {
                SkillManager.Instance.UseSkill(_skill.Uuid);
            }
        }
    }
}
