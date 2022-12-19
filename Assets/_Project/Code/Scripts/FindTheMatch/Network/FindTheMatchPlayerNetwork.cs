using FMOD.Studio;
using FMODUnity;
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
    [SerializeField] private EventReference _CountdownSFX;
    private float _currentTime = 0;
    private float _seconds;
    [SyncVar]private bool _timePassed;
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
    public void StartCountDown()
    {
        if (isServer)
        {
            StartCoroutine(CountDown());
        }
    }
    private IEnumerator CountDown()
    {
        RpcUpdateTimer("Get ready!");
        yield return new WaitForSeconds(3);
        RpcStartCountdownSFX();
        RpcUpdateTimer("3");
        yield return new WaitForSeconds(1);
        RpcUpdateTimer("2");
        yield return new WaitForSeconds(1);
        RpcUpdateTimer("1");
        yield return new WaitForSeconds(1);
        RpcUpdateTimer("GO!");
        yield return new WaitForSeconds(1.5f);
        RpcUpdateTimer(_startTime.ToString());
        RpcStartPuzzle();
        yield return new WaitForSeconds(1);
        RpcStartTimerStatus();

    }
    private void CheckAnswer(string answer)
    {
        if (isServer)
        {
            if (_timePassed)
            {
                RpcUpdateResult("You didn't press the correct answer in time!");
            }
            else if (answer == _correctAnswer)
            {
                RpcUpdateResult("You've pressed the correct answer in time!");
            }
            StopAllCoroutines();
            StartCoroutine(WaitBeforeRetry());
        }
    }
    private IEnumerator WaitBeforeRetry()
    {
        yield return new WaitForSeconds(3);
        RpcStopGame();
        if (isServer)
        {
            _enableStartBtnEvent.Invoke();
        }
    }
    private IEnumerator Timer()
    {
        if (isServer)
        {
            RpcUpdateResult(null);
            _currentTime = _startTime;
            _timePassed = false;
            while (_currentTime > 0)
            {
                _currentTime -= Time.deltaTime * _timeSpeed;
                _seconds = Mathf.FloorToInt(_currentTime % 60);
                if (_seconds == 1)
                {
                    RpcUpdateTimer(_seconds.ToString());
                }
                else if (_seconds! > 0)
                {
                    RpcUpdateTimer(_seconds.ToString());
                }
                yield return _currentTime;
            }
            _timePassed = true;
            RpcUpdateTimer("0");
            RpcAnswerSelected(null);
        }
    }
    [ClientRpc]
    private void RpcStartCountdownSFX()
    {
        EventInstance countDownEvent = RuntimeManager.CreateInstance(_CountdownSFX);
        RuntimeManager.AttachInstanceToGameObject(countDownEvent, transform);
        countDownEvent.start();
        countDownEvent.release();
    }
    [ClientRpc]
    private void RpcStopGame()
    {
        _updateTimerEvent.Invoke(null);
        _updateResultEvent.Invoke(null);
        _cmdStopPuzzleEvent.Invoke();
    }
    [ClientRpc]
    private void RpcUpdateTimer(string time)
    {
        _updateTimerEvent.Invoke(time);
    }
    [ClientRpc]
    private void RpcUpdateResult(string text)
    {
        _updateResultEvent.Invoke(text);
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
        if (isServer)
        {
            _cmdDisableBtnEvent.Invoke();
            StartCoroutine(Timer());
        }
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