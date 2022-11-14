using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkPlayerMoleManager : NetworkBehaviour
{
    [SerializeField] private DebugEvent _debugEvent;
    public void OnScreenTapped(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 postion = context.ReadValue<Vector2>();
            Ray ray = Camera.main.ScreenPointToRay(postion);
            if (Physics.Raycast(ray, out RaycastHit hit, 200))
            {
                if (hit.collider.gameObject != null)
                {
                    _debugEvent.Invoke("I've pressed: " + hit.collider.name);
                    return;
                }
            }
        }
    }
    #region MoleManager

    #endregion
}
