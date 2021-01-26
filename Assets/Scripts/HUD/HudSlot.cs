using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace hud
{
    public abstract class HudSlot : MonoBehaviour
    {
        [SerializeField] protected Image _slotIcon = default;

        public void Awake()
        {
            if (_slotIcon.sprite == null)
            {
                _slotIcon.gameObject.SetActive(false);
            }
        }

        public virtual void LoadImage(Sprite icon)
        {
            _slotIcon.sprite = icon;
            _slotIcon.gameObject.SetActive(true);

            if (icon == null)
            {
                _slotIcon.gameObject.SetActive(false);
            }
        }
    }
}
