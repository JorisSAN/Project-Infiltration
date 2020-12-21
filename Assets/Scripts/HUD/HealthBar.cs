using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace hud
{
	public class HealthBar : MonoBehaviour
	{
		[SerializeField] private Slider _slider = default;
		[SerializeField] private Gradient _gradient = default;
		[SerializeField] private Image _fill = default;

		// METHODS
		public void SetMaxHealth(int health)
		{
			_slider.maxValue = health;
			_slider.value = health;

			_fill.color = _gradient.Evaluate(1f);
		}

		public void SetHealth(int health)
		{
			_slider.value = health;

			_fill.color = _gradient.Evaluate(_slider.normalizedValue);
		}
	}
}
