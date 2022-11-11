using Mirror;
using System.Collections;
using UnityEngine;

public class ButtonManager : NetworkBehaviour
{
    [SerializeField] private DebugEvent _debugEvent;
    [SerializeField] private ButtonsListSO _buttonListSO;
    [SerializeField] private CmdTimerStartStatusEvent _commandTimerStartStatusEvent;
    [SerializeField] private CmdTaskCompleteEvent _commandTaskCompleteEvent;
    [SerializeField] private CmdUpdateCubeResetStatusEvent _commandCubeResetStatusEvent;
    [SerializeField] private CmdUpdateButtonStatusEvent _commandUpdateButtonStatusEvent;
    [SerializeField] private UpdateResultEvent _updateResultEvent;
    [SerializeField] private UpdateTimerEvent _updateTimerEvent;
    [SerializeField] private float _startTime = 10f;
    [SerializeField] private float _speed = 1f;
    private float _currentTime = 0f;
    private float _seconds;
    private bool _isTimerStarted = false;
    private bool AllButtonsPressed
    {
        get
        {
            int i = 0;
            foreach (ButtonSO button in _buttonListSO.buttonList)
            {
                if (button.isSelected)
                {
                    i++;
                }
                if (i >= _buttonListSO.buttonList.Count)
                {
                    return true;
                }
            }
            return false;
        }
    }
    public void OnCubePressed(string cube)
    {
        for (int i = 0; i < _buttonListSO.buttonList.Count; i++)
        {
            if (_buttonListSO.buttonList[i].cubeReference.name == cube)
            {
                if (_buttonListSO.buttonList[i].isAllowPress)
                {
                    _buttonListSO.buttonList[i].setAllowPress = false;
                    _commandUpdateButtonStatusEvent.Invoke(i, true);
                    _commandCubeResetStatusEvent.Invoke(false);
                    return;
                }
            }
        }
    }
    public void CheckAllButtonsPressed()
    {
        if (_isTimerStarted)
        {
            if (AllButtonsPressed)
            {
                _commandTimerStartStatusEvent.Invoke(false);
                _commandTaskCompleteEvent.Invoke();
            }
        }
        else
        {
            _commandTimerStartStatusEvent.Invoke(true);
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
        _commandTimerStartStatusEvent.Invoke(false);
        _updateTimerEvent.Invoke("Time remaining: " + 0 + " Seconds");
        _updateResultEvent.Invoke("You didn't press them in time!");
        StartCoroutine(ResetTime());
    }
    public void TaskCompleted()
    {
        StopAllCoroutines();
        StartCoroutine(ResetTime());
    }
    public void ResetButtons(bool isReset)
    {
        if (isReset)
        {
            StopAllCoroutines();
            _updateTimerEvent.Invoke("Time remaining: " + _startTime.ToString() + " Seconds");
            _updateResultEvent.Invoke(null);
            _isTimerStarted = false;
            for (int i = 0; i < _buttonListSO.buttonList.Count; i++)
            {
                _buttonListSO.buttonList[i].setSelected = false;
                _buttonListSO.buttonList[i].setAllowPress = true;
            }
        }
    }
    public void UpdateButtonStatus(int buttonIndex, bool isSelected)
    {
        _buttonListSO.buttonList[buttonIndex].setSelected = isSelected;
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
        _commandCubeResetStatusEvent.Invoke(true);
    }
}
