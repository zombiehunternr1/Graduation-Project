using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkPlayerMoleManager : NetworkBehaviour
{
    [SerializeField] private DebugEvent _debugEvent;
    [SerializeField] private RpcMolePressedEvent _rpcMolePressedEvent;
    [SerializeField] private RpcMoleUpdateColorEvent _rpcMoleUpdateColorEvent;
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
                    _rpcMolePressedEvent.Invoke(hit.collider.name);
                    return;
                }
            }
        }
    }
    #region MoleManager
    [ClientRpc]
    private void RpcUpdateMoleColor()
    {
        _rpcMoleUpdateColorEvent.Invoke();
    }
    [Command(requiresAuthority = false)]
    public void CmdUpdateMoleColor()
    {
        RpcUpdateMoleColor();
    }
    #endregion
}
