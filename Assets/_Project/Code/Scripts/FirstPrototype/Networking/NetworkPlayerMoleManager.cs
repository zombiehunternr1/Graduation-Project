using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkPlayerMoleManager : NetworkBehaviour
{
    [SerializeField] private DebugEvent _debugEvent;
    [SerializeField] private RpcUpdateUITextEvent _rpcUpdateUITextEvent;
    [SerializeField] private RpcMolePressedEvent _rpcMolePressedEvent;
    [SerializeField] private RpcMoleUpdateColorEvent _rpcMoleUpdateColorEvent;
    [SerializeField] private RpcMoleWackedEvent _rpcMoleWackedEvent;
    [SerializeField] private RpcCheckAllMolesWackedEvent _rpcCheckAllMolesWackedEvent;
    [SerializeField] private RpcResetAllMolesEvent _rpcResetAllMolesEvent;
    [SerializeField] private RpcRestartMoleGameEvent _rpcRestartMoleGameEvent;
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
                    string moleName = hit.collider.GetComponent<NetworkMole>().trackername;
                    _rpcMolePressedEvent.Invoke(moleName);
                    return;
                }
            }
        }
    }
    #region MoleManager
    [ClientRpc]
    private void RpcUpdateMoleColor(string moleName, Color32 moleColor)
    {
        _rpcMoleUpdateColorEvent.Invoke(moleName, moleColor);
    }
    [Command(requiresAuthority = false)]
    public void CmdUpdateMoleColor(string moleName, Color32 moleColor)
    {
        RpcUpdateMoleColor(moleName, moleColor);
    }
    [ClientRpc]
    private void RpcMoleWacked(string moleName)
    {
        _rpcMoleWackedEvent.Invoke(moleName);
    }
    [Command(requiresAuthority = false)]
    public void CmdMoleWacked(string moleName)
    {
        RpcMoleWacked(moleName);
    }
    [ClientRpc]
    private void RpcResetAllMoles()
    {
        _rpcResetAllMolesEvent.Invoke();
    }
    [Command(requiresAuthority = false)]
    public void CmdResetAllMoles()
    {
        RpcResetAllMoles();
    }
    [ClientRpc]
    private void RpcRestartMoleGame()
    {
        _rpcRestartMoleGameEvent.Invoke();
    }
    [Command(requiresAuthority = false)]
    public void CmdRestartMoleGame()
    {
        RpcRestartMoleGame();
    }
    [ClientRpc]
    private void RpcCheckAllMolesWacked()
    {
        _rpcCheckAllMolesWackedEvent.Invoke();
    }
    [Command(requiresAuthority = false)]
    public void CmdCheckAllMolesWacked()
    {
        RpcCheckAllMolesWacked();
    }
    [ClientRpc]
    private void RpcUpdateUIDisplay(string text)
    {
        _rpcUpdateUITextEvent.Invoke(text);
    }
    [Command(requiresAuthority = false)]
    public void CmdUpdateUIDisplay(string text)
    {
        RpcUpdateUIDisplay(text);
    }
    #endregion
}
