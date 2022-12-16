using Mirror;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class FindTheMatchPlayerNetwork : NetworkBehaviour
{
    [SerializeField] private DebugEvent _debugEvent;
    [SerializeField] private EnableStartBtnEvent _enableStartBtnEvent;
    [SerializeField] private UpdateTimerEvent _updateTimerEvent;
    [SerializeField] private UpdateResultEvent _updateResultEvent;
    [SerializeField] private CmdAnswerPressedEvent _cmdAnswerPressedEvent;
    [SerializeField] private CmdDisableBtnEvent _cmdDisableBtnEvent;
    [SerializeField] private CmdStartPuzzleEvent _cmdStartPuzzleEvent;
    [SerializeField] private CmdStopPuzzleEvent _cmdStopPuzzleEvent;
    [SerializeField] private CmdGetAnswerFromServerEvent _cmdGetAnswerFromServerEvent;
    [SerializeField] private CmdUpdateAnswerEvent _cmdUpdateAnswerEvent;
    [SerializeField] private float _startTime = 10;
    [SerializeField] private float _timeSpeed = 1;
    private float _currentTime = 0;
    private float _seconds;
    private bool _timePassed;
    private string _correctAnswer;
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
                    _cmdAnswerPressedEvent.Invoke(hit.collider.gameObject.name);
                    return;
                }
            }
        }
    }
    #region FindTheMatchManager
    public void StartGame()
    {
        RpcStartTimerStatus();
        RpcStartPuzzle();
    }
    private void CheckAnswer(string answer)
    {
        if (_timePassed)
        {
            return;
        }
        if(answer == _correctAnswer)
        {
            _updateResultEvent.Invoke("You've pressed the correct answer in time!");
            StopAllCoroutines();
            if (isServer)
            {
                StartCoroutine(WaitBeforeRetry());
            }
        }
    }
    private IEnumerator WaitBeforeRetry()
    {
        yield return new WaitForSeconds(3);
        RpcStopGame();
        _enableStartBtnEvent.Invoke();
    }
    private IEnumerator Timer()
    {
        _updateResultEvent.Invoke(null);
        _currentTime = _startTime;
        while (_currentTime > 0)
        {
            _timePassed = false;
            _currentTime -= Time.deltaTime * _timeSpeed;
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
        _timePassed = true;
        _updateTimerEvent.Invoke("Time remaining: " + 0 + " Seconds");
        _updateResultEvent.Invoke("You didn't press the correct answer in time!");
        yield return new WaitForSeconds(3);
        _updateTimerEvent.Invoke("Time remaining: " + _startTime + " Seconds");
        yield return new WaitForSeconds(1);
        StartCoroutine(Timer());
    }
    [ClientRpc]
    private void RpcStopGame()
    {
        _cmdStopPuzzleEvent.Invoke();
    }
    [ClientRpc]
    private void RpcAnswerSelected(string answer)
    {
        CheckAnswer(answer);
    }
    [Command(requiresAuthority = false)]
    public void CmdAnswerSelected(string answer)
    {
        RpcAnswerSelected(answer);
    }
    [ClientRpc]
    private void RpcStartTimerStatus()
    {
        _cmdDisableBtnEvent.Invoke();
        StartCoroutine(Timer());
    }
    [ClientRpc]
    private void RpcRequestAnswer()
    {
        _cmdGetAnswerFromServerEvent.Invoke();
    }
    [Command(requiresAuthority = false)]
    public void CmdRequestAnswer()
    {
        RpcRequestAnswer();
    }
    [ClientRpc]
    private void RpcSendAnswer(int value, string answer)
    {
        _correctAnswer = answer;
        _cmdUpdateAnswerEvent.Invoke(value);
    }
    [Command(requiresAuthority =false)]
    public void CmdSendAnswer(int value, string answer)
    {
        RpcSendAnswer(value, answer);
    }
    [ClientRpc]
    private void RpcStartPuzzle()
    {
        _cmdStartPuzzleEvent.Invoke();
    }
    #endregion
}
