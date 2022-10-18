using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Game Event Input", menuName = "Scriptable Objects/Events/Game Events/Input")]
public class GameEventInput : ScriptableObject
{
    private List<GameEventInputListener> _listeners = new List<GameEventInputListener>();
    public void RaiseInputEvent(InputAction.CallbackContext ctx)
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].OnEventRaised(ctx);
        }
    }
    public void RegisterListener(GameEventInputListener listener)
    {
        _listeners.Add(listener);
    }
    public void UnregisterListener(GameEventInputListener listener)
    {
        _listeners.Remove(listener);
    }
}
