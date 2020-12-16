using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace hud
{
	public class DiscretionBar : MonoBehaviour
	{
		[SerializeField] private Slider _slider;

		// METHODS
		public void SetMaxDiscretion(int discretion)
		{
			_slider.maxValue = discretion;
			_slider.value = discretion;
		}

		public void SetDiscretion(int discretion)
		{
			_slider.value = discretion;
		}
	}
}
