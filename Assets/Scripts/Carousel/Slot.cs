using utils.runtime;
using utils.animationcurve;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace carousel
{
    [Serializable]
    public class Slot<TDisplayer, UInfo> : MonoBehaviour where TDisplayer : ASlotListElementDisplayer<UInfo>
    {
        [SerializeField] private RectTransform _slotTransform = default;
        public List<TDisplayer> Displayer;
        [SerializeField, Curve(0, 1, 0, 1)] private AnimationCurve _ease = ExtAnimationCurve.GetDefaultCurve(ExtAnimationCurve.EaseType.EASE_IN_OUT);
        public bool isAnimating;
        [SerializeField] private float _scale = default;
        [SerializeField] private float _alpha = default;
        [SerializeField] private float _animationDuration = default;
        public virtual void AddDisplayer(TDisplayer displayer)
        {
            Displayer.Add(displayer);
            displayer.RectTransform.position = _slotTransform.position;
            displayer.RectTransform.localScale = new Vector3(_scale, _scale, _scale);
            ResizeSlot(displayer.RectTransform);
            displayer.SetDisplayerAlpha(_alpha);
        }
        private void ResizeSlot(RectTransform displayer)
        {
            _slotTransform.SetHeight(displayer.rect.height * _scale);
            _slotTransform.SetWidth(displayer.rect.width * _scale);
        }
        public virtual void ChangeDisplayerOfSlotWithAnimation(Slot<TDisplayer, UInfo> slotToUnlink)
        {
            Displayer.Add(slotToUnlink.Displayer[0]);
            slotToUnlink.Displayer.RemoveAt(0);
            StartCoroutine(LerpTranslate(_animationDuration));
        }
        public virtual void ChangeDisplayerOfSlotWithoutAnimation(Slot<TDisplayer, UInfo> slotToUnlink)
        {
            Displayer.Add(slotToUnlink.Displayer[0]);
            slotToUnlink.Displayer.RemoveAt(0);
            TDisplayer displayer = Displayer[Displayer.Count - 1];
            displayer.transform.position = _slotTransform.position;
            displayer.transform.localScale = new Vector3(_scale, _scale, _scale);
            displayer.SetDisplayerAlpha(_alpha);
        }
        private IEnumerator LerpTranslate(float duration)
        {
            isAnimating = true;
            TDisplayer displayer = Displayer[Displayer.Count - 1];
            Transform transform = displayer.transform;
            Vector2 startPosition = transform.position;
            Vector3 originalScale = transform.localScale;
            float originalAlpha = displayer.GetDisplayerAlpha();

            float timeElapsed = 0;
            if (duration == 0)
            {
                transform.position = _slotTransform.position;
                transform.localScale = new Vector3(_scale, _scale, _scale);
            }
            while (timeElapsed < duration)
            {
                transform.position = Vector2.Lerp(startPosition, _slotTransform.position, GetEasedProgress(timeElapsed / duration));
                transform.localScale = Vector3.Lerp(originalScale, new Vector3(_scale, _scale, _scale), timeElapsed / duration);
                float newAlpha = Mathf.Lerp(originalAlpha, _alpha, timeElapsed / duration);
                displayer.SetDisplayerAlpha(newAlpha);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            isAnimating = false;
        }
        protected float GetEasedProgress(float time)
        {
            return (_ease.Evaluate(time));
        }
    }
}