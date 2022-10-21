using UnityEngine;
using UnityEngine.Events;

public class BaseEventGenericListener<T> : MonoBehaviour
{
    [SerializeField] private BaseEventGeneric<T> _baseEvent;
    [SerializeField] private UnityEvent<T> _unityEvent;
    private void OnEnable()
    {
        _baseEvent.RegisterListener(this);
    }
    private void OnDisable()
    {
        _baseEvent.UnregisterListener(this);
    }
    public void Invoke(T type)
    {
        _unityEvent.Invoke(type);
    }
}
