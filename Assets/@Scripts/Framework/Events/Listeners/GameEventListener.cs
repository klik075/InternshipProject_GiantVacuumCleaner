using System;
using Scripts.Framework.Events.Interfaces;
using Scripts.Framework.Events.SO;
using UnityEngine;

namespace Scripts.Framework.Events.Listeners
{
    /// <summary>
    /// 일반적인 이벤트 옵저빙 클래스 (Listener)
    /// </summary>
    [Serializable]
    public class GameEventListener : IGameEventListener
    {
        #region Fields

        [Header("Event, this class is observing")]
        [SerializeField] private GameEventSO _gameEvent;

        // Event handler invoked once the event is trigger
        private event Action EventHandler = delegate { };

        public GameEventSO GameEvent => _gameEvent;

        #endregion



        #region Methods

        public void Subscribe() => _gameEvent.AddListener(this);

        public void Unsubscribe() => _gameEvent.RemoveListener(this);

        public void OnEventRaised() => EventHandler.Invoke();

        public void SubstitutionEvent(Action invokeAction)
        {
            if (invokeAction == null)
            {
                Debugger.LogError("Invoke Action arguments is null");
                return;
            }

            EventHandler = invokeAction;
        }

        #endregion
    }
}
