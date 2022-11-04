using Mirror;
using System.Collections;
using UnityEngine;

public class ButtonTestManager : NetworkBehaviour
{
    [SerializeField] private ButtonsListSO _buttonListSO;
    [SerializeField] private UpdateResultEvent _updateResultEvent;
    [SerializeField] private UpdateTimerEvent _updateTimerEvent;
    [SerializeField] private ResetCubeColorEvent _resetCubeColorEvent;
    [SerializeField] private float _startTime = 10f;
    [SerializeField] private float _speed = 1f;
    private float _currentTime = 0f;
    private float _seconds;
    private bool _isTimerStarted = false;
    private bool _allowPress = true;
    private bool AllButtonsPressed
    {
        get
        {
            int i = 0;
            foreach (ButtonSO button in _buttonListSO.Buttons)
            {
                if (button.IsSelected)
                {
                    i++;
                }
                if (i >= _buttonListSO.Buttons.Count)
                {
                    return true;
                }
            }
            return false;
        }
    }
    public void OnCubePressed(string cube)
    {
        if (_allowPress)
        {
            foreach (ButtonSO button in _buttonListSO.Buttons)
            {
                if (button.CubeReference.name == cube)
                {
                    button.SetSelected = true;
                    CmdUpdateButtonStatus(button);
                    CmdCheckButtonsPressed();
                }
            }
        }
    }
    private void CheckButtonsPressed()
    {
        if (_isTimerStarted)
        {
            if (AllButtonsPressed)
            {
                CmdSetTimerStatus(false);
                CmdTaskCompleted();
            }
        }
        else
        {
            CmdSetTimerStatus(true);
            StartCoroutine(Timer());
        }
    }
    private IEnumerator Timer()
    {
        _currentTime = _startTime;
        while (_currentTime > 0)
        {
            _currentTime -= Time.deltaTime * _speed;
            _seconds = Mathf.FloorToInt(_currentTime % 60);
            if (_seconds == 1)
            {
                _updateTimerEvent.Invoke("Time remaining: " + _seconds.ToString() + " Second");
            }
            else if (_seconds! > 0)
            {
                _updateTimerEvent.Invoke("Time remaining: " + _seconds.ToString() + " Seconds");
            }
            yield return _currentTime;
        }
        CmdAllowPressStatus(false);
        CmdSetTimerStatus(false);
        _updateTimerEvent.Invoke("Time remaining: " + 0 + " Seconds");
        _updateResultEvent.Invoke("You didn't press them in time!");
        StartCoroutine(ResetTime());
    }
    private void TaskCompleted()
    {
        StartCoroutine(ResetTime());
    }
    private void ResetButtons()
    {
        StopAllCoroutines();
        _updateTimerEvent.Invoke("Time remaining: " + _startTime.ToString() + " Seconds");
        _updateResultEvent.Invoke(null);
        _resetCubeColorEvent.Invoke();
        foreach (ButtonSO button in _buttonListSO.Buttons)
        {
            button.SetSelected = false;
            CmdUpdateButtonStatus(button);
        }
        CmdAllowPressStatus(true);
    }
    private void UpdateButtonStatus(ButtonSO pressedButton)
    {
        foreach (ButtonSO button in _buttonListSO.Buttons)
        {
            if (button == pressedButton)
            {
                button.SetSelected = pressedButton.IsSelected;
            }
        }
    }
    private void AllowPressStatus(bool allowPress)
    {
        _allowPress = allowPress;
    }
    private void SetTimerStatus(bool timerStarted)
    {
        _isTimerStarted = timerStarted;
    }
    private IEnumerator ResetTime()
    {
        if (AllButtonsPressed)
        {
            _updateResultEvent.Invoke("Pressed both buttons within the time limit!");
        }
        yield return new WaitForSeconds(3);
        CmdResetButtons();
    }
    [ClientRpc]
    private void RpcUpdateButtonStatus(ButtonSO pressedButton)
    {
        UpdateButtonStatus(pressedButton);
    }
    [Command(requiresAuthority = false)]
    private void CmdUpdateButtonStatus(ButtonSO pressedButton)
    {
        RpcUpdateButtonStatus(pressedButton);
    }
    [ClientRpc]
    private void RpcResetButtons()
    {
        ResetButtons();
    }
    [Command(requiresAuthority = false)]
    private void CmdResetButtons()
    {
        RpcResetButtons();
    }
    [ClientRpc]
    private void RpcAllowPressStatus(bool allowPress)
    {
        AllowPressStatus(allowPress);
    }
    [Command(requiresAuthority = false)]
    private void CmdAllowPressStatus(bool allowPress)
    {
        RpcAllowPressStatus(allowPress);
    }
    [ClientRpc]
    private void RpcSetTimerStatus(bool timerStarted)
    {
        SetTimerStatus(timerStarted);
    }
    [Command(requiresAuthority = false)]
    private void CmdSetTimerStatus(bool timerStarted)
    {
        RpcSetTimerStatus(timerStarted);
    }
    [ClientRpc]
    private void RpcCheckButtonsPressed()
    {
        CheckButtonsPressed();
    }
    [Command(requiresAuthority = false)]
    private void CmdCheckButtonsPressed()
    {
        RpcCheckButtonsPressed();
    }
    [ClientRpc]
    private void RpcTaskCompleted()
    {
        TaskCompleted();
    }
    [Command(requiresAuthority = false)]
    private void CmdTaskCompleted()
    {
        RpcTaskCompleted();
    }
}
