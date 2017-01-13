using UnityEngine;

namespace SafeTween
{
    public static class TransformExtension
    {
        public static TweenLocalPosition MoveLocalPosition(this Transform transform, Vector3 target, float duration, float delay = 0)
        {
            var tweener = new Tweener();
            var tweenProperty = new TweenLocalPosition(transform, transform.localPosition, target, delay, duration + delay);
            tweener.Add(tweenProperty);
            tweener.PlayForward();
            return tweenProperty;
        }

        public static TweenPosition MovePosition(this Transform transform, Vector3 target, float duration, float delay = 0)
        {
            var tweener = new Tweener();
            var tweenProperty = new TweenPosition(transform, transform.position, target, delay, duration + delay);
            tweener.Add(tweenProperty);
            tweener.PlayForward();
            return tweenProperty;
        }

        public static TweenLocalRotation RotateLocal(this Transform transform, Vector3 target, float duration, float delay = 0)
        {
            var tweener = new Tweener();
            var tweenProperty = new TweenLocalRotation(transform, transform.localRotation.eulerAngles, target, delay, duration + delay);
            tweener.Add(tweenProperty);
            tweener.PlayForward();
            return tweenProperty;
        }

        public static TweenLocalRotation TweenRotationLocal(this Transform transform, Quaternion target, float duration, float delay = 0)
        {
            var tweener = new Tweener();
            var tweenProperty = new TweenLocalRotation(transform, transform.localRotation, target, delay, duration + delay);
            tweener.Add(tweenProperty);
            tweener.PlayForward();
            return tweenProperty;
        }

        public static TweenLocalRotation TweenRotate(this Transform transform, Vector3 target, float duration, float delay = 0)
        {
            var tweener = new Tweener();
            var tweenProperty = new TweenLocalRotation(transform, transform.rotation.eulerAngles, target, delay, duration + delay);
            tweener.Add(tweenProperty);
            tweener.PlayForward();
            return tweenProperty;
        }

        public static TweenLocalRotation TweenRotation(this Transform transform, Quaternion target, float duration, float delay = 0)
        {
            var tweener = new Tweener();
            var tweenProperty = new TweenLocalRotation(transform, transform.rotation, target, delay, duration + delay);
            tweener.Add(tweenProperty);
            tweener.PlayForward();
            return tweenProperty;
        }
    }
}