using player.item;
using player.skill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hud
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private Slot _skillSlot = default;
        [SerializeField] private Slot _itemSlot = default;

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
