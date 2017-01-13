using UnityEngine;

namespace SafeTween
{
    public class TweenPropertyGeneric<T> : TweenPropertyBase where T : new()
    {
        protected T target;

        public override bool TargetNotNull()
        {
            return target != null;
        }
    }

    public class TweenPropertyUnityGeneric<T> : TweenPropertyBase where T : Object
    {
        protected T target;

        public override bool TargetNotNull()
        {
            return target != null;
        }
    }
}