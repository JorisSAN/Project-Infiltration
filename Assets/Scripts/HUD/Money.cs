using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace hud
{
	public class Money : MonoBehaviour
	{
		[SerializeField] private Text _amount;
		[SerializeField] private Image _icon;

		// METHODS
		public void SetAmount(string amount)
		{
			_amount.text = amount;
		}

		public void SetIcon(Sprite icon)
		{
			_icon.sprite = icon;
		}
	}
}
