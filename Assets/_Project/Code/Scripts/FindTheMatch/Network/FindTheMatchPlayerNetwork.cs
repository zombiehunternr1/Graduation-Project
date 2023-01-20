using FMOD.Studio;
using FMODUnity;
using Mirror;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class FindTheMatchPlayerNetwork : NetworkBehaviour
{
    [SerializeField] private DebugEvent _debugEvent;
    [SerializeField] private EnableGameOverButtonsEvent _enableStartBtnEvent;
    [SerializeField] private UpdateTimerEvent _updateTimerEvent;
    [SerializeField] private UpdateResultEvent _updateResultEvent;
    [SerializeField] private CmdAnswerPressedEvent _cmdAnswerPressedEvent;
    [SerializeField] private RpcDisableGameOverButtonsEvent _rpcDisableBtnEvent;
    [SerializeField] private RpcSetupGameModeEvent _rpcSetupGameModeEvent;
    [SerializeField] private RpcStartPuzzleEvent _rpcStartPuzzleEvent;
    [SerializeField] private RpcStopPuzzleEvent _rpcStopPuzzleEvent;
    [SerializeField] private RpcGetAnswerFromServerEvent _rpcGetAnswerFromServerEvent;
    [SerializeField] private RpcUpdateAnswerEvent _rpcUpdateAnswerEvent;
    [SerializeField] private RpcDisplayResultEvent _rpcDisplayResultEvent;
    [SerializeField] private RpcDisableUIElementEvent _rpcDisableUIElementEvent;
    [SerializeField] private RpcReturnToMenuEvent _rpcReturnToMenuEvent;
    [SerializeField] private RpcDisplayKeyStatusEvent _rpcDisplayKeyStatusEvent;
    [SerializeField] private RpcDisplayConfettiParticlesEvent _rpcDisplayConfettiParticlesEvent;
    [SerializeField] private float _startingCountDown = 10;
    [SerializeField] private float _countDownSpeed = 1;
    [SerializeField] private float _volumeFadingSpeed = 0.2f;
    [SerializeField] private int _roundsTotal = 3;
    [SerializeField] private EventReference _backgroundMusic;
    [SerializeField] private EventReference _countdownSFX;
    [SerializeField] private EventReference _successSFX;
    [SerializeField] private EventReference _failedSFX;
    [SerializeField] private EventReference _pressSFX;
    [SerializeField] private EventReference _keyCollectedSFX;
    private EventInstance _backgroundInstance;
    private EventInstance _soundEffectInstance;
    private float _currentBackgroundVolume = 1f;
    private float _currentTime;
    private float _decreasedTime;
    private float _seconds;
    private bool _timePassed;
    private bool _backgroundMusicCreated;
    private bool _soundEffectCreated;
    private bool _hasAnswered;
    private string _correctAnswer;
    private int _currentRound = 1;

    private void OnValidate()
    {
        if(_roundsTotal <= 0)
        {
            _roundsTotal = 1;
            Debug.LogWarning("Can't have a total rounds count equal or less than zero!");
        }
    }
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
                    _soundEffectInstance = RuntimeManager.CreateInstance(_pressSFX);
                    _soundEffectInstance.start();
                    _soundEffectInstance.release();
                    _cmdAnswerPressedEvent.Invoke(hit.collider.gameObject.name);
                    return;
                }
            }
        }
    }
    #region FindTheMatchManager
    public void StartGame(bool isDancing)
    {
        RpcSetupGame(isDancing);
    }
    private void StopMusic()
    {
        StopAllCoroutines();
        _backgroundInstance.setVolume(0);
    }
    private void StartCountDown()
    {
        if (isServer)
        {
            _hasAnswered = false;
            RpcDisableUIElement();
            StartCoroutine(CountDown());
        }
        if (_backgroundMusicCreated)
        {
            StartCoroutine(FadeOutVolume(false));
            return;
        }
        RpcStartBackgroundMusic(_backgroundMusic);
    }
    private IEnumerator CountDown()
    {
        RpcUpdateResult("Round " + _currentRound + " / " + _roundsTotal);
        RpcUpdateTimer("Get ready!");
        yield return new WaitForSeconds(3);
        RpcPlaySoundEffect(_countdownSFX);
        RpcUpdateTimer("3");
        yield return new WaitForSeconds(1);
        RpcUpdateTimer("2");
        yield return new WaitForSeconds(1);
        RpcUpdateTimer("1");
        yield return new WaitForSeconds(1);
        RpcUpdateTimer("GO!");
        yield return new WaitForSeconds(1.5f);
        RpcUpdateTimer(_currentTime.ToString());
        RpcStartPuzzle();
        yield return new WaitForSeconds(1);
        RpcStartTimerStatus();
    }
    private void CheckAnswer(string answer)
    {
        if (isServer)
        {
            if (_timePassed || answer != _correctAnswer)
            {
                if (!_hasAnswered)
                {
                    _hasAnswered = true;
                    EndResult(_timePassed, false);
                    return;
                }
            }
            else
            {
                if (!_hasAnswered)
                {
                    _hasAnswered = true;
                    EndResult(_timePassed, true);
                    return;
                }
            }
        }
    }
    private void EndResult(bool timePassed, bool wasCorrect)
    {
        if (timePassed || !wasCorrect)
        {
            RpcDisplayResult(0);
            RpcPlaySoundEffect(_failedSFX);
            if (timePassed)
            {
                RpcUpdateResult("You didn't press in time");
            }
            else
            {
                RpcUpdateResult("You've pressed the wrong answer");
            }
            RpcStopAllCoroutines();
            RpcDisplayNextStep(false);
            return;
        }
        else
        {
            RpcDisplayResult(1);
            RpcPlaySoundEffect(_successSFX);
            RpcUpdateResult("You've pressed the correct answer!");
        }       
        if(_currentRound == _roundsTotal)
        {
            RpcUpdateResult("Congratulations! You've got the key to escape the escape room!");
            RpcStopAllCoroutines();
            RpcDisplayNextStep(false);
            RpcPlaySoundEffect(_keyCollectedSFX);
            RpcDisplayKeyCollected();
            RpcDisplayConfettiParticles();
            StartCoroutine(WaitBeforeRetry());
        }
        else
        {
            _decreasedTime = _decreasedTime - 2;
            _currentTime = _decreasedTime;
            _currentRound++;
            RpcStopAllCoroutines();
            RpcDisplayNextStep(true);
        }
    }
    private IEnumerator DisplayNextStep(bool isSuccess)
    {
        if (isSuccess)
        {
            yield return new WaitForSeconds(3);
            RpcUpdateResult("Get Ready for the next round!");
            yield return new WaitForSeconds(3);
            RpcUpdateResult(null);
            StartCountDown();
        }
        else
        {
            StartCoroutine(WaitBeforeRetry());
        }
    }
    private IEnumerator WaitBeforeRetry()
    {
        yield return new WaitForSeconds(5);
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
            _timePassed = false;
            while (_currentTime > 0)
            {
                _currentTime -= Time.deltaTime * _countDownSpeed;
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
            RpcUpdateTimer(null);
            RpcAnswerSelected(null);
        }
    }
    private IEnumerator FadeOutVolume(bool isFadeOut)
    {
        if (isFadeOut)
        {
            while (_currentBackgroundVolume > 0.25f)
            {
                _currentBackgroundVolume -= Time.deltaTime * _volumeFadingSpeed;
                _backgroundInstance.setVolume(_currentBackgroundVolume);
                yield return _currentBackgroundVolume;
            }
            _currentBackgroundVolume = 0.25f;
        }
        else
        {
            while (_currentBackgroundVolume < 1f)
            {
                _currentBackgroundVolume += Time.deltaTime * _volumeFadingSpeed;
                _backgroundInstance.setVolume(_currentBackgroundVolume);
                yield return _currentBackgroundVolume;
            }
            _currentBackgroundVolume = 1f;
        }
        _backgroundInstance.setVolume(_currentBackgroundVolume);
    }
    [ClientRpc]
    private void RpcDisplayConfettiParticles()
    {
        _rpcDisplayConfettiParticlesEvent.Invoke();
    }
    [ClientRpc]
    private void RpcDisplayKeyCollected()
    {
        _rpcDisplayKeyStatusEvent.Invoke("Showing");
    }
    [ClientRpc]
    private void RpcDisableUIElement()
    {
        _rpcDisableUIElementEvent.Invoke();
    }
    [ClientRpc]
    private void RpcStopAllCoroutines()
    {
        StopAllCoroutines();
    }
    [ClientRpc]
    private void RpcDisplayNextStep(bool result)
    {
        StartCoroutine(DisplayNextStep(result));
    }
    [ClientRpc]
    private void RpcDisplayResult(float result)
    {
        StartCoroutine(FadeOutVolume(true));
        _rpcDisplayResultEvent.Invoke(result);
    }
    [ClientRpc]
    private void RpcPlaySoundEffect(EventReference soundEffect)
    {
        if (!_soundEffectCreated)
        {
            _soundEffectCreated = true;
            RuntimeManager.AttachInstanceToGameObject(_soundEffectInstance, transform);
        }
        _soundEffectInstance = RuntimeManager.CreateInstance(soundEffect);
        _soundEffectInstance.start();
        _soundEffectInstance.release();
    }
    [ClientRpc]
    private void RpcStartBackgroundMusic(EventReference backgroundMusic)
    {
        _backgroundMusicCreated = true;
        _backgroundInstance = RuntimeManager.CreateInstance(backgroundMusic);
        RuntimeManager.AttachInstanceToGameObject(_backgroundInstance, transform);
        _backgroundInstance.start();
        _backgroundInstance.release();
    }
    [ClientRpc]
    private void RpcSetupGame(bool isDancing)
    {
        _rpcDisplayKeyStatusEvent.Invoke("Hidden");
        _rpcSetupGameModeEvent.Invoke(isDancing);
        _currentTime = _startingCountDown;
        _decreasedTime = _startingCountDown;
        _currentRound = 1;
        StartCoroutine(FadeOutVolume(false));
        StartCountDown();
    }
    [ClientRpc]
    private void RpcStopGame()
    {
        _rpcStopPuzzleEvent.Invoke();
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
        if (isServer)
        {
            CheckAnswer(answer);
        }
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
            _rpcDisableBtnEvent.Invoke();
            StartCoroutine(Timer());
        }
    }
    [ClientRpc]
    private void RpcRequestAnswer()
    {
        _rpcGetAnswerFromServerEvent.Invoke();
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
        _rpcUpdateAnswerEvent.Invoke(value);
    }
    [Command(requiresAuthority =false)]
    public void CmdSendAnswer(int value, string answer)
    {
        RpcSendAnswer(value, answer);
    }
    [ClientRpc]
    private void RpcStartPuzzle()
    {
        _rpcStartPuzzleEvent.Invoke();
    }
    [Command(requiresAuthority = false)]
    public void CmdReturnToMenu()
    {
        RpcReturnToMenu();
    }
    [ClientRpc]
    private void RpcReturnToMenu()
    {
        StopMusic();
        _rpcReturnToMenuEvent.Invoke();
    }
    #endregion
}
