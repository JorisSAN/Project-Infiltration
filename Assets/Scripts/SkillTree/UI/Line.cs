using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace skilltree
{
    public class Line : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform = default;
        [SerializeField] private Image _image = default;

        public RectTransform RectTransform
        {
            get
            {
                return _rectTransform;
            }
        }

        public void ChangeImageColor(Color color)
        {
            _image.color = color;
        }

        public void ResetPosition()
        {
            float actualLocalX = gameObject.transform.localPosition.x;
            float actualLocalY = gameObject.transform.localPosition.y;
            gameObject.transform.localPosition = new Vector3(actualLocalX, actualLocalY, 0);
        }
    }
}
