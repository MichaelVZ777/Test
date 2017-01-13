using UnityEngine;

namespace SafeTween
{
    public static class CameraExtension
    {
        public static TweenCameraFOV TweenFOV(this Camera camera, float target, float duration, float delay = 0)
        {
            var tweener = new Tweener();
            var tweenProperty = new TweenCameraFOV(camera, camera.fieldOfView, target, delay, duration + delay);
            tweener.Add(tweenProperty);
            tweener.PlayForward();
            return tweenProperty;
        }
    }
}