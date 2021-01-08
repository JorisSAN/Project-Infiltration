using UnityEngine;

namespace carousel
{
    public abstract class ACarouselElementDisplayer<T> : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _rectTransform;
        public RectTransform RectTransform
        {
            get
            {
                return _rectTransform;
            }
        }

        public abstract void Replenish(T elementToInsert);
        public void HideDisplayer()
        {
            gameObject.SetActive(false);
        }

        protected void OnValidate()
        {
            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }
        }
    }
}