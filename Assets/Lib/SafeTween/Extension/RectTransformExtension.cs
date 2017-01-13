using UnityEngine;

namespace SafeTween
{
    public static class RectTransformExtension
    {
        public static TweenLocalPosition MoveLocalPositionRelative(this RectTransform rectTransform, Vector3 target, float duration, float delay = 0)
        {
            var tweener = new Tweener();
            var tweenProperty = new TweenLocalPosition(rectTransform, rectTransform.localPosition, rectTransform.localPosition + target, delay, duration + delay);
            tweener.Add(tweenProperty);
            tweener.PlayForward();
            return tweenProperty;
        }

        public static TweenLocalPosition MoveLocalPosition(this RectTransform rectTransform, Vector3 target, float duration, float delay = 0)
        {
            var tweener = new Tweener();
            var tweenProperty = new TweenLocalPosition(rectTransform, rectTransform.localPosition, target, delay, duration + delay);
            tweener.Add(tweenProperty);
            tweener.PlayForward();
            return tweenProperty;
        }

        public static TweenAnchorPosition MoveAnchorPosition(this RectTransform rectTransform, Vector2 target, float duration, float delay = 0)
        {
            var tweener = new Tweener();
            var tweenProperty = new TweenAnchorPosition(rectTransform, rectTransform.anchoredPosition, target, delay, duration + delay);
            tweener.Add(tweenProperty);
            tweener.PlayForward();
            return tweenProperty;
        }

        public static TweenSizeDelta AnimateScale(this RectTransform rectTransform, Vector2 target, float duration, float delay = 0)
        {
            var tweener = new Tweener();
            var tweenProperty = new TweenSizeDelta(rectTransform, rectTransform.sizeDelta, target, delay, duration + delay);
            tweener.Add(tweenProperty);
            tweener.PlayForward();
            return tweenProperty;
        }
    }
}