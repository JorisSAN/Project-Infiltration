using player.obj;
using player.skill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hud
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private Slot _skillSlot = default;
        [SerializeField] private Slot _objectSlot = default;

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

        public void SelectObject(PlayerObject obj)
        {
            if (obj is null)
            {
                _objectSlot.LoadImage(null);
                return;
            }
            _objectSlot.LoadImage(obj.Icon);
        }

        public void UseObject()
        {
            // Start timer for the next possible use
        }
    }
}
