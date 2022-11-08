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
    private void RpcResetButtons()
    {
        _buttonTestManager.ResetButtons();
    }
    [Command(requiresAuthority = false)]
    public void CmdResetButtons()
    {
        RpcResetButtons();
    }
    [ClientRpc]
    private void RpcAllowPressStatus(bool allowPress)
    {
        _buttonTestManager.AllowPressStatus(allowPress);
    }
    [Command(requiresAuthority = false)]
    public void CmdAllowPressStatus(bool allowPress)
    {
        RpcAllowPressStatus(allowPress);
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
        _buttonTestManager.CheckButtonsPressed();
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
