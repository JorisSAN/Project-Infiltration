using player.item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hud.selectionwheel
{
    public class ItemWheel : SelectionWheel
    {
        public new ItemWheelSlot CurrentSlot
        {
            get
            {
                if (_currentSelectedSlot >= 0 && _currentSelectedSlot < _slots.Length)
                {
                    return (ItemWheelSlot)_slots[_currentSelectedSlot];
                }
                return null;
            }
        }

        public new ItemWheelSlot PreviousSlot
        {
            get
            {
                if (_previousSelectedSlot >= 0 && _previousSelectedSlot < _slots.Length)
                {
                    return (ItemWheelSlot)_slots[_previousSelectedSlot];
                }
                return null;
            }
        }

        public void LoadWheel(List<PlayerItem> items)
        {
            int index = 0;
            foreach (PlayerItem item in items)
            {
                if (index < _slots.Length)
                {
                    ((ItemWheelSlot)_slots[index]).Load(item);
                }
                index++;
            }

            for (int i=index; i<_slots.Length; i++)
            {
                ((ItemWheelSlot)_slots[i]).SetDefault();
            }
        }

        public override void SelectSlot()
        {
            if (CurrentSlot != null && CurrentSlot.PlayerItem != null)
            {
                _hud.SelectItem(CurrentSlot.PlayerItem.Uuid);
            }
        }
    }
}
