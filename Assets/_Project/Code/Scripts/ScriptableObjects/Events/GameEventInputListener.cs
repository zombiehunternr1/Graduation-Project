using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameEventInputListener : MonoBehaviour
{
    [SerializeField] private GameEventInput _inputEvent;
    [SerializeField] private UnityEvent<InputAction.CallbackContext> _respondse;
    private void OnEnable()
    {
        _inputEvent.RegisterListener(this);
    }
    private void OnDisable()
    {
        _inputEvent.UnregisterListener(this);
    }
    public void OnEventRaised(InputAction.CallbackContext ctx)
    {
        _respondse.Invoke(ctx);
    }
}
