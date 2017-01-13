using UnityEngine;
using System.Collections;

namespace SafeTween
{
    public static class CanvasGroupExtension
    {
        public static TweenCanvasGroupAlpha TweenAlpha(this CanvasGroup canvasGroup, float target, float duration, float delay = 0)
        {
            var tweener = new Tweener();
            var tweenCanvasGroupAlpha = new TweenCanvasGroupAlpha(canvasGroup, canvasGroup.alpha, target, delay, duration + delay);
            tweener.Add(tweenCanvasGroupAlpha);
            tweener.PlayForward();
            return tweenCanvasGroupAlpha;
        }
    }
}