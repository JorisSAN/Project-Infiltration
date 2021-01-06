using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace hud
{
    public class Slot : MonoBehaviour
    {
        [SerializeField] private Image _slotIcon = default;

        public void Awake()
        {
            if (_slotIcon.sprite == null)
            {
                _slotIcon.gameObject.SetActive(false);
            }
        }

        public void LoadImage(Sprite icon)
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
