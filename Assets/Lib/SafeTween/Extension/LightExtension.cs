using UnityEngine;

namespace SafeTween
{
    public static class LightExtension
    {
        public static TweenLightIntensity TweenIntensity(this Light light, float target, float duration, float delay = 0)
        {
            var tweener = new Tweener();
            var tweenProperty = new TweenLightIntensity(light, light.intensity, target, delay, duration + delay);
            tweener.Add(tweenProperty);
            tweener.PlayForward();
            return tweenProperty;
        }

        public static TweenLightRange TweenRange(this Light light, float target, float duration, float delay = 0)
        {
            var tweener = new Tweener();
            var tweenProperty = new TweenLightRange(light, light.range, target, delay, duration + delay);
            tweener.Add(tweenProperty);
            tweener.PlayForward();
            return tweenProperty;
        }

        public static TweenLightSpotAngle TweenSpotAngle(this Light light, float target, float duration, float delay = 0)
        {
            var tweener = new Tweener();
            var tweenProperty = new TweenLightSpotAngle(light, light.spotAngle, target, delay, duration + delay);
            tweener.Add(tweenProperty);
            tweener.PlayForward();
            return tweenProperty;
        }
    }
}