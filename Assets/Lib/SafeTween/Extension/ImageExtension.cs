using UnityEngine;
using UnityEngine.UI;

namespace SafeTween
{
    public static class ImageExtension
    {
        public static TweenImageAlpha TweenAlpha(this Image image, float target, float duration, float delay = 0)
        {
            var tweener = new Tweener();
            var tweenProperty = new TweenImageAlpha(image, image.color.a, target, delay, duration + delay);
            tweener.Add(tweenProperty);
            tweener.PlayForward();
            return tweenProperty;
        }

        public static TweenImageFill TweenFill(this Image image, float target, float duration, float delay = 0)
        {
            var tweener = new Tweener();
            var tweenProperty = new TweenImageFill(image, image.fillAmount, target, delay, duration + delay);
            tweener.Add(tweenProperty);
            tweener.PlayForward();
            return tweenProperty;
        }

        public static TweenImageColor TweenColor(this Image image, Color target, float duration, float delay = 0)
        {
            var tweener = new Tweener();
            var tweenProperty = new TweenImageColor(image, image.color, target, delay, duration + delay);
            tweener.Add(tweenProperty);
            tweener.PlayForward();
            return tweenProperty;
        }
    }
}