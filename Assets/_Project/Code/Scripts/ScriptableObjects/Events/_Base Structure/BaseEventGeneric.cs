using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEventGeneric<T> : ScriptableObject
{
    protected List<BaseEventGenericListener<T>> _listeners = new List<BaseEventGenericListener<T>>();

    public virtual void Invoke(T type)
    {
        foreach(BaseEventGenericListener<T> listener in _listeners)
        {
            listener.Invoke(type);
        }
    }
    public void RegisterListener(BaseEventGenericListener<T> listener)
    {
        _listeners.Add(listener);
    }
    public void UnregisterListener(BaseEventGenericListener<T> listener)
    {
        _listeners.Remove(listener);
    }
}
