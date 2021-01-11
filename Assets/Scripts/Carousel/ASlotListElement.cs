using UnityEngine;

namespace carousel
{
    public abstract class ASlotListElementDisplayer<T> : ACarouselElementDisplayer<T>
    {
        [SerializeField] private CanvasGroup _canvasGroup = default;

        public void SetDisplayerAlpha(float alpha)
        {
            _canvasGroup.alpha = alpha;
        }
        public float GetDisplayerAlpha()
        {
            return _canvasGroup.alpha;
        }
    }
}

