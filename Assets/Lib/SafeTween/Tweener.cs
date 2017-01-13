using UnityEngine;
using System.Collections.Generic;
using System;

namespace SafeTween
{
    public class Tweener
    {
        List<TweenPropertyBase> tweenProperties;
        public float speed = 1;
        public bool repeat;
        public bool autoDestroy = true;
        float time;
        float duration;
        bool isReverse;

        protected Action OnForwardAction;
        protected Action OnBackwardAction;
        protected Action OnForwardCompletedAction;
        protected Action OnBackwardCompletedAction;
        protected List<TimedAction> OnForwardTimedActions;
        protected List<TimedAction> OnBackwardTimedActions;

        public float normalizedTime { get { return time / duration; } }

        public Tweener()
        {
            tweenProperties = new List<TweenPropertyBase>();
        }

        public void Add(TweenPropertyBase tweenProperty)
        {
            tweenProperty.tweener = this;
            tweenProperty.isPlaying = true;
            tweenProperties.Add(tweenProperty);

            if (tweenProperty.endTime > duration)
                duration = tweenProperty.endTime;
        }

        /// <summary>
        /// Reset to start position
        /// </summary>
        public void Reset()
        {
            foreach (var property in tweenProperties)
                property.UpdateValue(0);
            time = 0;
        }

        /// <summary>
        /// Set to end position
        /// </summary>
        public void SetToEnd()
        {
            foreach (var property in tweenProperties)
                property.SetValue(1);
            time = duration;
        }

        /// <summary>
        /// Play from start
        /// </summary>
        public void Play()
        {
            foreach (var property in tweenProperties)
                property.SetValue(0);
            ResetTimedAction();
            PlayForward(0);
        }

        /// <summary>
        /// Play forward from current position
        /// </summary>
        public void PlayForward()
        {
            PlayForward(time);
        }

        /// <summary>
        /// Play forward from position
        /// </summary>
        /// <param name="time">position</param>
        public void PlayForward(float time)
        {
            foreach (var property in tweenProperties)
            {
                property.isPlaying = true;
                property.isReverse = false;
            }

            if (OnForwardAction != null)
                OnForwardAction();

            this.time = time;
            isReverse = false;
            SafeTweenManager.Play(this);
        }

        /// <summary>
        /// Play backward from end
        /// </summary>
        public void Reverse()
        {
            foreach (var property in tweenProperties)
                property.SetValue(1);
            PlayBackward(duration);
        }

        /// <summary>
        /// Play backward from current position
        /// </summary>
        public void PlayBackward()
        {
            PlayBackward(time);
        }

        /// <summary>
        /// Play backward from position
        /// </summary>
        /// <param name="time">position</param>
        public void PlayBackward(float time)
        {
            foreach (var property in tweenProperties)
            {
                property.isPlaying = true;
                property.isReverse = true;
            }

            if (OnBackwardAction != null)
                OnBackwardAction();

            time = duration;
            isReverse = true;
            SafeTweenManager.Play(this);
        }

        /// <summary>
        /// Stop animation at current position
        /// </summary>
        public void Stop()
        {
            SafeTweenManager.Stop(this);
        }

        public void SetDuration(float duration)
        {
            this.duration = Mathf.Clamp(duration, this.duration, float.MaxValue);
        }

        public void ResetTimedAction()
        {
            if (OnForwardTimedActions != null)
                foreach (var timedAction in OnForwardTimedActions)
                    timedAction.triggered = false;

            if (OnBackwardTimedActions != null)
                foreach (var timedAction in OnBackwardTimedActions)
                    timedAction.triggered = false;
        }

        public void Update()
        {
            if (isReverse)
                time -= Time.deltaTime * speed;
            else
                time += Time.deltaTime * speed;

            bool allNull = true;
            foreach (var tweenProperty in tweenProperties)
                if (tweenProperty.TargetNotNull() && (tweenProperty.isPlaying || repeat))
                {
                    tweenProperty.Update(time);
                    allNull = false;
                }

            if (allNull && autoDestroy)
                Stop();

            RunTimedAction();

            if (!isReverse && normalizedTime > 1)
                OnForwardComplete();

            if (isReverse && normalizedTime < 0)
                OnBackwardComplete();
        }

        void RunTimedAction()
        {
            if (!isReverse && OnForwardTimedActions != null)
                foreach (var timedAction in OnForwardTimedActions)
                    if (!timedAction.triggered && normalizedTime * duration > timedAction.triggerTime)
                    {
                        timedAction.triggered = true;
                        timedAction.action();
                    }

            if (isReverse && OnBackwardTimedActions != null)
                foreach (var timedAction in OnBackwardTimedActions)
                    if (!timedAction.triggered && normalizedTime * duration < timedAction.triggerTime)
                    {
                        timedAction.triggered = true;
                        timedAction.action();
                    }
        }

        void OnForwardComplete()
        {
            if (repeat)
                time = 0;
            else
                Stop();

            if (OnForwardCompletedAction != null)
                OnForwardCompletedAction();
        }

        void OnBackwardComplete()
        {
            if (repeat)
                time = duration;
            else
                Stop();

            if (OnBackwardCompletedAction != null)
                OnBackwardCompletedAction();
        }

        public virtual Tweener OnForward(Action OnForwardAction)
        {
            this.OnForwardAction = OnForwardAction;
            return this;
        }

        public virtual Tweener OnBackword(Action OnBackwardAction)
        {
            this.OnBackwardAction = OnBackwardAction;
            return this;
        }

        public virtual Tweener OnForwardComplete(Action OnBackwardCompletedAction)
        {
            this.OnForwardCompletedAction = OnBackwardCompletedAction;
            return this;
        }

        public virtual Tweener OnBackwardComplete(Action OnForwardCompletedAction)
        {
            this.OnBackwardCompletedAction = OnForwardCompletedAction;
            return this;
        }

        public virtual Tweener OnTime(float time, Action timedAction, TimedActionType timedType = TimedActionType.Forward)
        {
            switch (timedType)
            {
                case TimedActionType.Forward:
                    if (OnForwardTimedActions == null)
                        OnForwardTimedActions = new List<TimedAction>();
                    OnForwardTimedActions.Add(new TimedAction { triggerTime = time, action = timedAction });
                    break;
                case TimedActionType.Backward:
                    if (OnBackwardTimedActions == null)
                        OnBackwardTimedActions = new List<TimedAction>();
                    OnBackwardTimedActions.Add(new TimedAction { triggerTime = time, action = timedAction });
                    break;
                case TimedActionType.Both:
                    if (OnForwardTimedActions == null)
                        OnForwardTimedActions = new List<TimedAction>();
                    OnForwardTimedActions.Add(new TimedAction { triggerTime = time, action = timedAction });

                    if (OnBackwardTimedActions == null)
                        OnBackwardTimedActions = new List<TimedAction>();
                    OnBackwardTimedActions.Add(new TimedAction { triggerTime = time, action = timedAction });
                    break;
            }
            if (time > duration)
                duration = time;
            return this;
        }

        public enum TimedActionType
        {
            Forward,
            Backward,
            Both
        }

        protected class TimedAction
        {
            public bool triggered;
            public float triggerTime;
            public Action action;
        }
    }
}