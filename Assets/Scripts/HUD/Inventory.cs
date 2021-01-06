using player.skill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hud
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private Slot _skillSlot = default;

        public void SelectSkill(PlayerSkill skill)
        {
            if (skill is null)
            {
                _skillSlot.LoadImage(null);
                return;
            }
            _skillSlot.LoadImage(skill.Icon);
        }

        public void UseSkill()
        {
            // Start timer for the next possible use
        }
    }
}
