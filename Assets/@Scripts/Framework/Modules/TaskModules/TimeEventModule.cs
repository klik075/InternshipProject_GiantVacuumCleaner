using System;
using System.Collections.Generic;

namespace Scripts.Framework.Modules.TaskModules
{
    public class TimeEventModule
    {
        private readonly List<TimelineEvent> _events = new();
        private float _currentTime;
        private readonly TimeLine _timer;

        public TimeEventModule(TimeLine timer)
        {
            _timer = timer;
            _timer.Tick += Update;
            _currentTime = 0f;
        }
        
        public void AddEvent(float time, Action action)
        {
            _events.Add(new TimelineEvent { Time = time, Action = action });
            _events.Sort((e1, e2) => e1.Time.CompareTo(e2.Time));
        }
        
        private void Update(float deltaTime)
        {
            _currentTime += _timer.DeltaTime;

            while (_events.Count > 0 && _events[0].Time <= _currentTime)
            {
                var timelineEvent = _events[0];
                _events.RemoveAt(0);
                timelineEvent.Action?.Invoke();
            }
        }
        
        public void Reset()
        {
            _currentTime = 0f;
            _events.Clear();
        }
        
        private class TimelineEvent
        {
            public float Time { get; set; }
            public Action Action { get; set; }
        }
    }
}