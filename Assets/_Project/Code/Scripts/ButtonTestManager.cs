using System.Collections;
using UnityEngine;

public class ButtonTestManager : MonoBehaviour
{
    [SerializeField] private ButtonsListSO _buttonListSO;
    [SerializeField] private DebugEvent _debugEvent;
    [SerializeField] private UpdateTimerEvent _updateTimerEvent;
    [SerializeField] private ResetCubeColorEvent _resetCubeColorEvent;
    [SerializeField] private float _startTime = 10f;
    [SerializeField] private float _speed = 1f;
    private float _currentTime = 0f;
    private bool _isTimerStarted = false;

    public void OnCubePressed(string cube)
    {
        foreach(ButtonSO button in _buttonListSO.Buttons)
        {
            if (button.CubeReference.name == cube)
            {
                button.setSelected = true;
                CheckButtonsPressed();
            }
        }
    }
    private void CheckButtonsPressed()
    {
        if (_isTimerStarted)
        {
            if (AllButtonsPressed)
            {
                StopAllCoroutines();
                _isTimerStarted = false;
                _currentTime = _startTime;
                _debugEvent.Invoke("Pressed both buttons within the time limit!");
            }
        }
        else
        {
            StartCoroutine(Timer());
        }
    }
    private IEnumerator Timer()
    {
        _isTimerStarted = true;
        _currentTime = _startTime;
        while(_currentTime > 0)
        {
            _currentTime -= Time.deltaTime * _speed;
            float seconds = Mathf.FloorToInt(_currentTime % 60);
            if (seconds == 1)
            {
                _updateTimerEvent.Invoke("Time remaining: " + seconds.ToString() + " Second");
            }
            else
            {
                _updateTimerEvent.Invoke("Time remaining: " + seconds.ToString() + " Seconds");
            }
            yield return _currentTime;
        }
        _isTimerStarted = false;
        _currentTime = _startTime;
        ResetButtons();
        _resetCubeColorEvent.Invoke();
        _updateTimerEvent.Invoke("Time remaining: " + _startTime.ToString() + " Seconds");
        _debugEvent.Invoke("You didn't press them in time!");
        yield return new WaitForSeconds(3);
        _debugEvent.Invoke(null);
        yield return null;
    }
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
    private void ResetButtons()
    {
        foreach(ButtonSO button in _buttonListSO.Buttons)
        {
            button.setSelected = false;
        }
    }
}
