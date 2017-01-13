using UnityEngine;
using UnityEngine.UI;

namespace SafeTween
{
    public static class TextExtension
    {
        public static TweenTextColor Fill(this Text text, Color target, float duration, float delay = 0)
        {
            var tweener = new Tweener();
            var tweenTextColor = new TweenTextColor(text, text.color, target, delay, duration + delay);
            tweener.Add(tweenTextColor);
            tweener.PlayForward();
            return tweenTextColor;
        }
    }
}