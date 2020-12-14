using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace skilltree
{
    public class ButtonSkillCategory : MonoBehaviour
    {
        [SerializeField] private Button _button = default;
        [SerializeField] private Text _content = default;

        // METHODS

        /// <summary>
        /// Change the content of the button
        /// </summary>
        /// <param name="nexContent"></param>
        public void ChangeContent(string nexContent)
        {
            _content.text = nexContent;
        }

        public void RemoveAllListeners()
        {
            _button.onClick.RemoveAllListeners();
        }

        public void AddListener(UnityAction action)
        {
            _button.onClick.AddListener(action);
        }

        public void ResetPosition()
        {
            float actualLocalX = gameObject.transform.localPosition.x;
            float actualLocalY = gameObject.transform.localPosition.y;
            gameObject.transform.localPosition = new Vector3(actualLocalX, actualLocalY, 0);
        }
    }
}
