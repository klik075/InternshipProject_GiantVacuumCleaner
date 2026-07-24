using System.Collections.Generic;
using Scripts.Framework.Events.Interfaces;

namespace Scripts.Framework.Events.SO
{
    public abstract class GameEventSO<T> : DescriptionSO
    {
        #region Fields

        protected readonly List<IGameEventListener<T>> _gameEventListeners = new();

        protected readonly object _lock = new();

        #endregion
    
        #region Event Regist and Raise

        public void RaiseEvent(T item)
        {
            lock (_lock) for (int idx = _gameEventListeners.Count - 1; idx >= 0; --idx) _gameEventListeners[idx].OnEventRaised(item);
            ResetListeners();
        }

        public void AddListener(IGameEventListener<T> listener)
        {
            lock (_lock) if (!_gameEventListeners.Contains(listener)) _gameEventListeners.Add(listener);
        }

        public void RemoveListener(IGameEventListener<T> listener)
        {
            lock (_lock) if (_gameEventListeners.Contains(listener)) _gameEventListeners.Remove(listener);
        }

        public abstract void ResetListeners();

        #endregion
    }
}
