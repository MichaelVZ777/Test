using UnityEngine;
using System.Collections.Generic;
using System;

namespace UnityLib.Utility
{
    public class SimpleTimerManager : SingletonMonoManager<SimpleTimerManager>
    {
        public List<SimpleTimer> simpleTimers;

        void Awake()
        {
            simpleTimers = new List<SimpleTimer>();
        }

        public void AddTimer(SimpleTimer timer)
        {
            if (!simpleTimers.Contains(timer))
                simpleTimers.Add(timer);
        }

        public void RemoveTimer(SimpleTimer timer)
        {
            if (simpleTimers.Contains(timer))
                simpleTimers.Remove(timer);
        }

        void Update()
        {
            for (int i = 0; i < simpleTimers.Count; i++)
                simpleTimers[i].Tick(Time.deltaTime);
        }
    }

    public class SimpleTimer
    {

        SimpleTimerManager host;
        float timeLeft;
        float time;
        public Action onComplete;

        public SimpleTimer()
        {
            host = SimpleTimerManager.Instance;
        }

        public void ResetAndStart()
        {
            timeLeft = time;
        }

        public void SetAndStart(float time)
        {
            this.time = time;
            timeLeft = time;
            host.AddTimer(this);
        }

        public void Stop()
        {
            host.RemoveTimer(this);
        }

        public void Tick(float deltaTime)
        {
            timeLeft -= deltaTime;
            if (timeLeft <= 0)
            {
                Stop();
                if (onComplete != null)
                    onComplete();
            }
        }
    }
}