using Mirror;
using System.Collections;
using UnityEngine;

public class ButtonTestManager : NetworkBehaviour
{
    [SerializeField] private DebugEvent _debugEvent;
    [SerializeField] private NetworkButtonTestManager _networkButtonTestManager;
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
            for(int i = 0; i < _buttonListSO.Buttons.Count; i++)
            {
                if (_buttonListSO.Buttons[i].CubeReference.name == cube)
                {
                    _buttonListSO.Buttons[i].SetSelected = true;
                    _networkButtonTestManager.CmdUpdateButtonStatus(i, true);
                    _networkButtonTestManager.CmdCheckButtonsPressed();
                    return;
                }
            }
        }
    }
    public void CheckButtonsPressed()
    {
        if (_isTimerStarted)
        {
            if (AllButtonsPressed)
            {
                _networkButtonTestManager.CmdSetTimerStatus(false);
                _networkButtonTestManager.CmdTaskCompleted();
            }
        }
        else
        {
            _networkButtonTestManager.CmdSetTimerStatus(true);
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
        _networkButtonTestManager.CmdAllowPressStatus(false);
        _networkButtonTestManager.CmdSetTimerStatus(false);
        _updateTimerEvent.Invoke("Time remaining: " + 0 + " Seconds");
        _updateResultEvent.Invoke("You didn't press them in time!");
        StartCoroutine(ResetTime());
    }
    public void TaskCompleted()
    {
        StopAllCoroutines();
        StartCoroutine(ResetTime());
    }
    public void ResetButtons()
    {
        StopAllCoroutines();
        _updateTimerEvent.Invoke("Time remaining: " + _startTime.ToString() + " Seconds");
        _updateResultEvent.Invoke(null);
        _resetCubeColorEvent.Invoke();
        for(int i = 0; i < _buttonListSO.Buttons.Count; i++)
        {
            _buttonListSO.Buttons[i].SetSelected = false;
        }
        _networkButtonTestManager.CmdAllowPressStatus(true);
    }
    public void UpdateButtonStatus(int buttonIndex, bool isSelected)
    {
        _buttonListSO.Buttons[buttonIndex].SetSelected = isSelected;
    }
    public void AllowPressStatus(bool allowPress)
    {
        _allowPress = allowPress;
    }
    public void SetTimerStatus(bool timerStarted)
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
        _networkButtonTestManager.CmdResetButtons();
    }
}
