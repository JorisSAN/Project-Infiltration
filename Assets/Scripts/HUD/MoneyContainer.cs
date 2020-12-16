using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace hud
{
	public class MoneyContainer : MonoBehaviour
	{
		[SerializeField] private Text _amount;
		[SerializeField] private Image _icon;
        [SerializeField] private int _beginFontSize = 36;
        [SerializeField] private int _bigAnimatedFont = 60;

        private IEnumerator _currentDisplayAmountChangeRoutine = null;

        // METHODS
        public bool IsShown()
        {
            return (gameObject.activeInHierarchy);
        }

        public void SetIcon(Sprite icon)
		{
			_icon.sprite = icon;
		}

        public void SetAmount(string amount)
        {
            _amount.text = amount;
        }

        public void DisplayCurrencyAmountChange(long newAmount, float animationTime = 1f)
        {
            newAmount = System.Math.Max(0, newAmount);

            //if there is already a display currency change routine for this currency stop it
            if (_currentDisplayAmountChangeRoutine != null)
            {
                StopCoroutine(_currentDisplayAmountChangeRoutine);
                _currentDisplayAmountChangeRoutine = null;
            }
            if (IsShown())
            {
                StartCoroutine(_currentDisplayAmountChangeRoutine = DisplayMoneyAmountChangeRoutine(_amount, newAmount, animationTime));
            }
        }

        private IEnumerator DisplayMoneyAmountChangeRoutine(Text targetText, long newAmount, float animationTime = 1f)
        {
            Debug.Log("Start display money change");

            long currentAmount = long.Parse(targetText.text.ToString());

            int beginningFontSize = _beginFontSize;
            int endFontSize = _bigAnimatedFont;

            float textEasingTime = 0.1f;
            float amountChangingAnimationTime = animationTime - textEasingTime * 2;

            Color colorBeforeAnim = Color.black;
            if (newAmount < currentAmount)
                targetText.color = Color.red;
            else
                targetText.color = Color.green;

            //Anim font get bigger
            float time = 0f;
            while (time < textEasingTime)
            {
                targetText.fontSize = Mathf.RoundToInt(Mathf.Lerp(beginningFontSize, endFontSize, time / textEasingTime));
                time += Time.unscaledDeltaTime;
                yield return null;
            }
            targetText.fontSize = endFontSize;
            //-

            //Anim amount changing
            time = 0f;
            while (time < amountChangingAnimationTime)
            {
                targetText.text = Mathf.RoundToInt(Mathf.Lerp(currentAmount, newAmount, time / amountChangingAnimationTime)).ToString();
                time += Time.unscaledDeltaTime;
                yield return null;
            }
            targetText.text = newAmount.ToString();
            //-

            //Anim font get smaller
            time = 0f;
            while (time < textEasingTime)
            {
                targetText.fontSize = Mathf.RoundToInt(Mathf.Lerp(endFontSize, beginningFontSize, time / textEasingTime));
                time += Time.unscaledDeltaTime;
                yield return null;
            }
            //-

            //Reset text color and size
            targetText.fontSize = beginningFontSize;
            targetText.color = colorBeforeAnim;

            _currentDisplayAmountChangeRoutine = null;
        }
    }
}
