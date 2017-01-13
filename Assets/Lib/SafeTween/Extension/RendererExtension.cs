using UnityEngine;

namespace SafeTween
{
    public static class RendererExtension
    {
        public static TweenMaterialColor TweenColor(this Renderer renderer, string colorName, Color target, float duration, float delay = 0)
        {
            var tweener = new Tweener();
            var tweenProperty = new TweenMaterialColor(renderer, colorName, renderer.material.GetColor(colorName), target, delay, duration + delay);
            tweener.Add(tweenProperty);
            tweener.PlayForward();
            return tweenProperty;
        }

        public static TweenMaterialColorAlpha TweenAlpha(this Renderer renderer, string colorName, float target, float duration, float delay = 0)
        {
            var tweener = new Tweener();
            var tweenProperty = new TweenMaterialColorAlpha(renderer, colorName, renderer.material.GetColor(colorName).a, target, delay, duration + delay);
            tweener.Add(tweenProperty);
            tweener.PlayForward();
            return tweenProperty;
        }
    }
}