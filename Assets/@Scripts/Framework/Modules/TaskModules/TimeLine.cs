using System;
using UnityEngine;

namespace Scripts.Framework.Modules.TaskModules
{
    public class TimeLine
    {
        public event Action<float> Tick = delegate { };
        public event Action PostTick = delegate { };
        public event Action FixedTick= delegate { };
        public event Action OneSecondTick= delegate { };

        private float _lastTime;
        private float _lastSecondTick;
        private float _scaleTime;

        public TimeLine()
        {
            _lastTime = GetTime();
            _lastSecondTick = _lastTime;
            _scaleTime = 1f;
            DeltaTime = 0f;
            Time = 0f;
        }

        public float Time { get; private set; }

        public float DeltaTime { get; private set; }

        public float TimeScale 
        { 
            get => _scaleTime;
            set => _scaleTime = Math.Max(0f, value);
        }
        public float UnscaledTime { get; private set; }

        public void Update()
        {
            float now = GetTime();
            float delta = now - _lastTime;
            UnscaledTime += delta;
            DeltaTime = delta * TimeScale;
            Time += DeltaTime;

            bool isNewSecondTick = Mathf.Floor(now) > Mathf.Floor(_lastSecondTick);
            if (isNewSecondTick)
            {
                OneSecondTick.Invoke();
                _lastSecondTick = now;
            }

            Tick.Invoke(DeltaTime);
            _lastTime = now;
        }
        public void LateUpdate() => PostTick.Invoke();
        public void FixedUpdate() =>  FixedTick.Invoke();
        private float GetTime() => Environment.TickCount / 1000f;
    }
}