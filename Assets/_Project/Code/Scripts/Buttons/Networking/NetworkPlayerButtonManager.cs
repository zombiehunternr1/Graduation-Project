using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkPlayerButtonManager : NetworkBehaviour
{
    [SerializeField] private DebugEvent _debugEvent;
    [SerializeField] private RpcUpdatePressedNetworkCubeEvent _rpcCubePressedEvent;
    [SerializeField] private RpcCheckAllButtonsStatusEvent _rpcCheckAllButtonsStatusEvent;
    [SerializeField] private RpcUpdateButtonStatusEvent _rpcUpdatePressedNetworkCubeEvent;
    [SerializeField] private RpcUpdateCubeResetStatusEvent _rpcUpdateCubeResetStatusEvent;
    [SerializeField] private RpcTaskCompleteEvent _rpcTaskCompleteEvent;
    [SerializeField] private RpcTimerStartStatusEvent _rpcTimerStartedEvent;
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
                    _rpcCubePressedEvent.Invoke(hit.collider.name);
                    return;
                }
            }
        }
    }
    #region ButtonManager
    [ClientRpc]
    private void RpcUpdateButtonStatus(int buttonIndex, bool isSelected)
    {
        _rpcUpdatePressedNetworkCubeEvent.Invoke(buttonIndex, isSelected);
        CmdCheckAllButtonsPressed();
    }
    [Command(requiresAuthority = false)]
    public void CmdUpdateButtonStatus(int buttonIndex, bool isSelected)
    {
        RpcUpdateButtonStatus(buttonIndex, isSelected);
    }
    [ClientRpc]
    private void RpcResetCubes(bool isReset)
    {
        _rpcUpdateCubeResetStatusEvent.Invoke(isReset);
    }
    [Command(requiresAuthority = false)]
    public void CmdResetCubes(bool isReset)
    {
        RpcResetCubes(isReset);
    }
    [ClientRpc]
    private void RpcSetTimerStatus(bool timerStarted)
    {
        _rpcTimerStartedEvent.Invoke(timerStarted);
    }
    [Command(requiresAuthority = false)]
    public void CmdSetTimerStatus(bool timerStarted)
    {
        RpcSetTimerStatus(timerStarted);
    }
    [ClientRpc]
    private void RpcCheckAllButtonsPressed()
    {
        _rpcCheckAllButtonsStatusEvent.Invoke();
    }
    [Command(requiresAuthority = false)]
    public void CmdCheckAllButtonsPressed()
    {
        RpcCheckAllButtonsPressed();
    }
    [ClientRpc]
    private void RpcTaskCompleted()
    {
        _rpcTaskCompleteEvent.Invoke();
    }
    [Command(requiresAuthority = false)]
    public void CmdTaskCompleted()
    {
        RpcTaskCompleted();
    }
    #endregion
}
