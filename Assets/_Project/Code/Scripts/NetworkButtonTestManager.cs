using Mirror;
using UnityEngine;

public class NetworkButtonTestManager : NetworkBehaviour
{
    [SerializeField]private ButtonTestManager _buttonTestManager;

    [ClientRpc]
    private void RpcUpdateButtonStatus(int buttonIndex, bool isSelected)
    {
        _buttonTestManager.UpdateButtonStatus(buttonIndex, isSelected);
    }
    [Command(requiresAuthority = false)]
    public void CmdUpdateButtonStatus(int buttonIndex, bool isSelected)
    {
        RpcUpdateButtonStatus(buttonIndex, isSelected);
    }
    [ClientRpc]
    private void RpcResetButtons(bool isReset)
    {
        _buttonTestManager.ResetButtons(isReset);
    }
    [Command(requiresAuthority = false)]
    public void CmdResetButtons(bool isReset)
    {
        RpcResetButtons(isReset);
    }
    [ClientRpc]
    private void RpcSetTimerStatus(bool timerStarted)
    {
        _buttonTestManager.SetTimerStatus(timerStarted);
    }
    [Command(requiresAuthority = false)]
    public void CmdSetTimerStatus(bool timerStarted)
    {
        RpcSetTimerStatus(timerStarted);
    }
    [ClientRpc]
    private void RpcCheckButtonsPressed()
    {
        _buttonTestManager.CheckAllButtonsPressed();
    }
    [Command(requiresAuthority = false)]
    public void CmdCheckButtonsPressed()
    {
        RpcCheckButtonsPressed();
    }
    [ClientRpc]
    private void RpcTaskCompleted()
    {
        _buttonTestManager.TaskCompleted();
    }
    [Command(requiresAuthority = false)]
    public void CmdTaskCompleted()
    {
        RpcTaskCompleted();
    }
}
