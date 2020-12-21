using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace loadingscreen
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private Slider _slider = default;
        [SerializeField] private Text _progressText = default;
        [SerializeField] private Text _title = default;


        public void ChangeProgressText(string newText)
        {
            _progressText.text = newText;
        }

        public void ChangeSliderValue(float newValue)
        {
            _slider.value = newValue;
        }

        public void ChangeTitleText(string newTitle)
        {
            _title.text = newTitle;
        }
    }
}
