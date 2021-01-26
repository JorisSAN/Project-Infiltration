using player.skill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hud
{
    public class HudSkillSlot : HudSlot
    {
        private PlayerSkill _skill;
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

        public void LoadSkill(PlayerSkill skill)
        {
            _skill = skill;
            _cooldown = _skill.Cooldown;
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
                StartCooldown();
            }
        }

        public void StartCooldown()
        {
            _slotIcon.fillAmount = 0;
            IsCooldown = true;
        }

        public void StopCooldown()
        {
            IsCooldown = false;
        }
    }
}
