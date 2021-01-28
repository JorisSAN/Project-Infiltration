using player.item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hud
{
    public class HudItemSlot : HudSlot
    {
        private PlayerItem _item;
        private float _cooldown = 0f;

        public bool IsCooldown { get; private set; } = false;

        public void Update()
        {
            if (IsCooldown)
            {
                if (_cooldown <= 0)
                {
                    StopCooldown();
                }

                _slotIcon.fillAmount += 1 / _cooldown * Time.deltaTime;
                if (_slotIcon.fillAmount >= 1)
                {
                    _slotIcon.fillAmount = 1;
                    StopCooldown();
                }
            }
        }

        public void LoadItem(PlayerItem item)
        {
            _item = item;
            if (_item != null)
            {
                _cooldown = _item.Cooldown;
                Debug.Log("Item is consommable ? " + _item.Consommable);
                Debug.Log("Item stock = " + _item.Stock);
            }
        }

        public override void LoadImage(Sprite icon)
        {
            _slotIcon.sprite = icon;
            _slotIcon.gameObject.SetActive(true);

            if (icon == null)
            {
                _slotIcon.gameObject.SetActive(false);
            }
            else
            {
                if (!_item.Consommable)
                {
                    StartCooldown();
                }
            }
        }

        public void Use()
        {
            if (_item == null)
            {
                return;
            }

            if (_item.Consommable)
            {
                if (_item.Stock <= 0)
                {
                    LoadItem(null);
                    LoadImage(null);
                }
            }
            else
            {
                StartCooldown();
            }
        }

        public void StartCooldown()
        {
            if (_item != null)
            {
                _slotIcon.fillAmount = 0;
                IsCooldown = true;
            }
        }

        public void StopCooldown()
        {
            IsCooldown = false;
        }
    }
}
