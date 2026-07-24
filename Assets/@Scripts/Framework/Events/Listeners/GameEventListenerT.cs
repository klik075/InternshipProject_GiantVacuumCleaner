using System;
using Scripts.Framework.Events.Interfaces;
using Scripts.Framework.Events.SO;
using UnityEngine;

namespace Scripts.Framework.Events.Listeners
{
    [Serializable]
    public class GameEventListener<T> : IGameEventListener<T>
    {
        #region Fields

        [Header("Event, this class is observing")]
        [SerializeField] private GameEventSO<T> _gameEvent;

        // Event handler invoked once the event is trigger
        private event Action<T> EventHandler = delegate { };

        public GameEventSO<T> GameEvent => _gameEvent;

        #endregion



        #region Methods

        public void Subscribe() => _gameEvent.AddListener(this);

        public void Unsubscribe() => _gameEvent.RemoveListener(this);

        public void OnEventRaised(T item) => EventHandler.Invoke(item);

        public void SubstitutionEvent(Action<T> invokeAction)
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
