using UnityEngine;

namespace SafeTween
{
    public static class TweenRenderSettings
    {
        public static TweenAmbientColor AmbientColor(Color end, float duration, float delay = 0)
        {
            var tweener = new Tweener();
            var tweenProperty = new TweenAmbientColor(RenderSettings.ambientLight, end, delay, duration + delay);
            tweener.Add(tweenProperty);
            tweener.PlayForward();
            return tweenProperty;
        }

        public static TweenFogColor FogColor(Color end, float duration, float delay = 0)
        {
            var tweener = new Tweener();
            var tweenProperty = new TweenFogColor(RenderSettings.fogColor, end, delay, duration + delay);
            tweener.Add(tweenProperty);
            tweener.PlayForward();
            return tweenProperty;
        }

        public static TweenReflectionIntensity ReflectionIntensity(float end, float duration, float delay = 0)
        {
            var tweener = new Tweener();
            var tweenProperty = new TweenReflectionIntensity(RenderSettings.reflectionIntensity, end, delay, duration + delay);
            tweener.Add(tweenProperty);
            tweener.PlayForward();
            return tweenProperty;
        }

        public static TweenFogStartDistance FogStartDistance(float end, float duration, float delay = 0)
        {
            var tweener = new Tweener();
            var tweenProperty = new TweenFogStartDistance(RenderSettings.fogStartDistance, end, delay, duration + delay);
            tweener.Add(tweenProperty);
            tweener.PlayForward();
            return tweenProperty;
        }

        public static TweenFogEndDistance FogEndDistance(float end, float duration, float delay = 0)
        {
            var tweener = new Tweener();
            var tweenProperty = new TweenFogEndDistance(RenderSettings.fogEndDistance, end, delay, duration + delay);
            tweener.Add(tweenProperty);
            tweener.PlayForward();
            return tweenProperty;
        }
    }
}