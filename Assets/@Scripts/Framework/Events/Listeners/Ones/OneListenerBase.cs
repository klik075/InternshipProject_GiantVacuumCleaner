using System.Collections;
using System.Collections.Generic;

namespace Scripts.Framework.Events.Listeners.Ones
{
    public abstract class OneListenerBase<T> : IEnumerable<T> where T : class
    {
        #region Fields

        protected readonly List<T> _list = new();
        protected int _count;

        public int Count => _count;

        #endregion



        #region Add & Remove

        public void Add(T action)
        {
            var index = _list.IndexOf(action);
            if (index == -1)
            {
                _list.Add(action);
                _count++;
            }
            else
            {
                if (_count == 1) return;
                _list[index] = null;
                _list.Add(action);
            }
        }

        public void Remove(T action)
        {
            var index = _list.IndexOf(action);
            if (index != -1)
            {
                _list[index] = null;
                _count--;
            }
        }

        public void RemoveAll()
        {
            _list.Clear();
            _count = 0;
        }

        #endregion



        #region Utils

        public bool Contains(T action) => _list.Contains(action);
    
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

        #endregion
    }
}