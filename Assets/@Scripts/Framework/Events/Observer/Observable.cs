
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[Serializable]
public abstract class Observable : IObservable
{
    #region Fields

    [NonSerialized] private readonly List<IObserver> _observers = new();
    private int _observingCount;
    
    [XmlIgnore] public bool IsChanged { get; private set; }

    #endregion



    #region Override

    public void SetChanged()
    {
        IsChanged = true;

        if (_observingCount == 0) return;

        int length = _observers.Count;
        for (int i = 0; i < Math.Min(length, _observers.Count); i++)
        {
            IObserver current = _observers[i];
            current?.OnSomeChanged(this);
        }
        
        if (_observingCount == _observers.Count) return;
        
        for (int i = _observers.Count - 1; i >= 0; i--)
        {
            if (null == _observers[i]) _observers.RemoveAt(i);
        }
    }

    public void AddObserver(IObserver observer)
    {
        var index = _observers.IndexOf(observer);
        if (index == -1)
        {
            _observers.Add(observer);
            ++_observingCount;
            OnObserversChanged(_observingCount);
        }
        else
        {
            if (_observingCount == 1) return;
            _observers[index] = null;
            _observers.Add(observer);
        }
    }

    public void RemoveObserver(IObserver observer)
    {
        var index = _observers.IndexOf(observer);
        if (index != -1)
        {
            _observers[index] = null;
            --_observingCount;
            OnObserversChanged(_observingCount);
        }
    }

    #endregion



    #region Util Methods
    
    protected virtual void OnObserversChanged(int observingCount) { }

    public void Clear()
    {
        _observers.Clear();
        _observingCount = 0;
        OnObserversChanged(_observingCount);
    }
    
    public void Commit()
    {
        IsChanged = false;
    }

    public void SetChangedAndCommit()
    {
        SetChanged();
        Commit();
    }

    #endregion
}
