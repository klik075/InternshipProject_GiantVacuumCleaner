using System;

namespace Scripts.Framework.Events.Listeners.Ones
{
    public sealed class OneListener : OneListenerBase<Action>
    {
        public void Invoke()
        {
            if (_count == 0)
                return;

            int length = _list.Count;
            for (int i = 0; i < Math.Min(length, _list.Count); i++)
            {
                var current = _list[i];
                if (current != null)
                {
                    current.Invoke();
                }
            }

            if (_count == _list.Count)
                return;

            for (int i = _list.Count - 1; i >= 0; i--)
            {
                if (null == _list[i])
                {
                    _list.RemoveAt(i);
                }
            }
        }
    }

    public sealed class OneListener<T> : OneListenerBase<Action<T>>
    {
        public void Invoke(T value)
        {
            if (_count == 0)
                return;

            int length = _list.Count;
            for (int i = 0; i < Math.Min(length, _list.Count); i++)
            {
                var current = _list[i];
                if (current != null)
                {
                    current.Invoke(value);
                }
            }

            if (_count == _list.Count)
                return;

            for (int i = _list.Count - 1; i >= 0; i--)
            {
                if (null == _list[i])
                {
                    _list.RemoveAt(i);
                }
            }
        }
    }

    public sealed class OneListener<T1, T2> : OneListenerBase<Action<T1, T2>>
    {
        public void Invoke(T1 value1, T2 value2)
        {
            if (_count == 0)
                return;

            int length = _list.Count;
            for (int i = 0; i < Math.Min(length, _list.Count); i++)
            {
                var current = _list[i];
                if (current != null)
                {
                    current.Invoke(value1, value2);
                }
            }

            if (_count == _list.Count)
                return;

            for (int i = _list.Count - 1; i >= 0; i--)
            {
                if (null == _list[i])
                {
                    _list.RemoveAt(i);
                }
            }
        }
    }

    public sealed class OneListener<T1, T2, T3> : OneListenerBase<Action<T1, T2, T3>>
    {
        public void Invoke(T1 value1, T2 value2, T3 value3)
        {
            if (_count == 0)
                return;

            int length = _list.Count;
            for (int i = 0; i < Math.Min(length, _list.Count); i++)
            {
                var current = _list[i];
                if (current != null)
                {
                    current.Invoke(value1, value2, value3);
                }
            }

            if (_count == _list.Count)
                return;

            for (int i = _list.Count - 1; i >= 0; i--)
            {
                if (null == _list[i])
                {
                    _list.RemoveAt(i);
                }
            }
        }
    }
}