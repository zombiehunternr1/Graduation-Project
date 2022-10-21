using UnityEngine;
using UnityEngine.Events;

public class BaseEventGenericListener<T> : MonoBehaviour
{
    [SerializeField] private BaseEventGeneric<T> _genericEvent;
    [SerializeField] private UnityEvent<T> _unityEvent;
    private void OnEnable()
    {
        _genericEvent.RegisterListener(this);
    }
    private void OnDisable()
    {
        _genericEvent.UnregisterListener(this);
    }
    public void Invoke(T type)
    {
        _unityEvent.Invoke(type);
    }
}
