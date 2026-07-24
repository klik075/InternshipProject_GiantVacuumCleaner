using System.Collections.Generic;
using Scripts.Framework.Events.Interfaces;

namespace Scripts.Framework.Events.SO
{
    /// <summary>
    /// 게임 이벤트 중개 채널
    /// </summary>
    public abstract class GameEventSO : DescriptionSO
    {
        #region Fields

        protected readonly List<IGameEventListener> _gameEventListeners = new();
        // 이벤트는 멀티 쓰레드 취약점이 존재하기에 대입 연산 및 'lock'이용
        protected readonly object _lock = new();

        #endregion
        
        #region Event Regist and Raise

        public void RaiseEvent()
        {
            lock (_lock)
            {
                for (int idx = _gameEventListeners.Count - 1; idx >= 0; --idx)
                {
                    _gameEventListeners[idx].OnEventRaised();
                }
            }

            ResetListeners();
        }

        public void AddListener(IGameEventListener listener)
        {
            lock (_lock)
            {
                if (!_gameEventListeners.Contains(listener))
                {
                    _gameEventListeners.Add(listener);
                }
            }
        }

        public void RemoveListener(IGameEventListener listener)
        {
            lock (_lock)
            {
                if (_gameEventListeners.Contains(listener))
                {
                    _gameEventListeners.Remove(listener);
                }
            }
        }

        public abstract void ResetListeners();

        #endregion
    }
}
