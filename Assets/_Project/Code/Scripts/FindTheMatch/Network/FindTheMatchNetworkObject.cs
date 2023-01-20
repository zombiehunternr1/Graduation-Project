using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FindTheMatchNetworkObject : MonoBehaviour
{
    [SerializeField] private float _crossFadingSpeed = 0.1f;
    [Header("UI object references")]
    [SerializeField] private TextMeshPro _displayStageCountdownTimerText;
    [SerializeField] private TextMeshPro _displayStageRoundText;
    [SerializeField] private Renderer _displayStageResultRenderer;
    [Header("GameObject references")]
    [SerializeField] private GameObject _answerModelReference;
    [SerializeField] private List<GameObject> _optionModelReferences;
    [Header("Renderer references")]
    [SerializeField] private List<Renderer> _stageModelRenderers;
    [HideInInspector][SerializeField] private List<Renderer> _optionModelRenderers;
    [HideInInspector][SerializeField] private List<Renderer> _answerModelRenderers;
    [Header("Component references")]
    [SerializeField] private List<ParticleSystem> _confettiParticles;
    [SerializeField] private List<Material> _displayAnswerOnStageMaterials;
    [SerializeField] private List<Collider> _colliderOptions;
    [Header("Events")]
    [SerializeField] private DebugEvent _debugEvent;
    [SerializeField] private CmdRequestAnswerFromServerEvent _cmdRequestAnswerFromServerEvent;
    [SerializeField] private CmdSendAnswerEvent _cmdSendAnswerEvent;
    private List<int> _wrongOptionsList;
    private bool _isGameStarted = false;
    private bool _isGameFinished = false;
    private bool _isDisplayResult = false;
    private bool _allowStageDisplay = false;
    private int _currentAnswer;
    private int _previousAnswer = -1;
    private bool _isServer;
    private int randomValue
    {
        get
        {
            return Random.Range(0, _optionModelReferences.Count);
        }
    }
    private int pickRandomAnswer
    {
        get
        {
            while (_previousAnswer == _currentAnswer || _previousAnswer == -1)
            {
                _currentAnswer = randomValue;
                return _currentAnswer;
            }
            _previousAnswer = _currentAnswer;
            return _currentAnswer;
        }
    }
    private int pickRandomwWrong
    {
        get
        {
            int chosenRandomValue = Random.Range(0, _optionModelReferences.Count);
            while (!_wrongOptionsList.Contains(chosenRandomValue))
            {
                chosenRandomValue = Random.Range(0, _optionModelReferences.Count);
            }
            _wrongOptionsList.Remove(chosenRandomValue);
            return chosenRandomValue;
        }
    }
    public void SetupGameMode(bool isDancing)
    {
        if (isDancing)
        {
            _answerModelReference.GetComponent<Animator>().SetFloat("GameMode", 0);
            foreach(GameObject option in _optionModelReferences)
            {
                option.GetComponent<Animator>().SetFloat("GameMode", 0);
            }
        }
        else
        {
            _answerModelReference.GetComponent<Animator>().SetFloat("GameMode", 1);
            foreach (GameObject option in _optionModelReferences)
            {
                option.GetComponent<Animator>().SetFloat("GameMode", 1);
            }
        }
        _allowStageDisplay = true;
    }
    public void UpdateStageResultDisplay(string result)
    {
        _displayStageRoundText.text = result;
    }
    public void UpdateStageTimerDisplay(string currentTime)
    {
        _displayStageCountdownTimerText.text = currentTime;
    }
    public void StartGame()
    {
        _isDisplayResult = false;
        _isGameFinished = false;
        _isGameStarted = true;
    }
    public void StopGame()
    {
        _isGameStarted = false;
        _isDisplayResult = true;
    }
    public void ReturnedToMenu()
    {
        _allowStageDisplay = false;
        _isGameStarted = false;
        _isGameFinished = false;
        DisableResultUIDisplay();
    }
    public void DisableResultUIDisplay()
    {
        _isDisplayResult = false;
        _displayStageResultRenderer.enabled = false;
    }
    public void SetActiveState(bool value)
    {
        if (_isDisplayResult)
        {
            _displayStageResultRenderer.enabled = value;
        }
        foreach (Renderer stageRenderer in _stageModelRenderers)
        {
            if (_allowStageDisplay)
            {
                stageRenderer.enabled = value;
            }
            else
            {
                stageRenderer.enabled = false;
            }
        }
        if (_isServer)
        {
            if (!_isGameStarted && !_isGameFinished)
            {
                foreach (Collider collider in _colliderOptions)
                {
                    collider.enabled = false;
                }
                foreach (Renderer answerRenderer in _answerModelRenderers)
                {
                    answerRenderer.enabled = false;
                }
                foreach (Renderer optionRenderer in _optionModelRenderers)
                {
                    optionRenderer.enabled = false;
                }
            }
            else
            {
                foreach (Renderer answerRenderer in _answerModelRenderers)
                {
                    answerRenderer.enabled = value;
                }
            }
        }
        else
        {
            foreach (Collider collider in _colliderOptions)
            {
                if (_isGameStarted && !_isGameFinished)
                {
                    collider.enabled = value;
                }
                else
                {
                    collider.enabled = false;
                }
            }
            foreach (Renderer optionRenderer in _optionModelRenderers)
            {
                if (_isGameStarted || _isGameFinished)
                {
                    optionRenderer.enabled = value;
                }
                else
                {
                    optionRenderer.enabled = false;
                }
            }
        }
    }
    public void SetupGame(bool isServer)
    {
        DisableResultUIDisplay();
        _isGameStarted = false;
        _isGameFinished = false;
        _wrongOptionsList = new List<int>();
        if (isServer)
        {
            _isServer = isServer;
            if(_answerModelRenderers.Count == 0)
            {
                _answerModelRenderers.AddRange(_answerModelReference.GetComponentsInChildren<Renderer>());
            }
        }
        if(_optionModelRenderers.Count == 0)
        {
            foreach (GameObject optionModel in _optionModelReferences)
            {
                _optionModelRenderers.AddRange(optionModel.GetComponentsInChildren<Renderer>());
            }
        }
        SetupRound();
        if (!isServer)
        {
            _cmdRequestAnswerFromServerEvent.Invoke();
        }
    }
    public void GetAnswerFromServer()
    {
        if (!_isServer)
        {
            return;
        }
        _cmdSendAnswerEvent.Invoke(_currentAnswer, _optionModelReferences[_currentAnswer].transform.parent.name);
    }
    public void UpdateAnswer(int currentAnswer)
    {
        _currentAnswer = currentAnswer;
        for (int i = 0; i < _optionModelReferences.Count; i++)
        {
            if (i != _currentAnswer)
            {
                _wrongOptionsList.Add(i);
            }
        }
        DisplayOptions();
    }
    public void DisplayResult(float value)
    {
        _displayStageResultRenderer.material = _displayAnswerOnStageMaterials[(int)value];
        _displayStageCountdownTimerText.text = null;
        _isDisplayResult = true;
        _isGameFinished = true;
        _answerModelReference.GetComponent<Animator>().SetFloat("ShowResult", value);
        _answerModelReference.GetComponent<Animator>().CrossFade("Result", _crossFadingSpeed);
        for(int i = 0; i < _colliderOptions.Count; i++)
        {
            _colliderOptions[i].enabled = false;
        }
        for(int i = 0; i < _optionModelReferences.Count; i++)
        {
            _optionModelReferences[i].GetComponent<Animator>().SetFloat("ShowResult", value);
            _optionModelReferences[i].GetComponent<Animator>().CrossFade("Result", _crossFadingSpeed);
        }
    }
    public void DisplayConfettiParticles()
    {
        foreach(ParticleSystem confettiParticle in _confettiParticles)
        {
            confettiParticle.Play();
        }
    }
    private void SetupRound()
    {
        if (!_isServer)
        {
            return;
        }
        DisableResultUIDisplay();
        _currentAnswer = pickRandomAnswer;
        _answerModelReference.GetComponent<Animator>().SetFloat("ShowAnswer", _currentAnswer);
        _answerModelReference.GetComponent<Animator>().Play("GameModeAnswers");
        _cmdSendAnswerEvent.Invoke(_currentAnswer, _optionModelReferences[_currentAnswer].transform.parent.name);
    }
    private void DisplayOptions()
    {
        for (int i = 0; i < _optionModelReferences.Count; i++)
        {
            if (i == _currentAnswer)
            {
                _optionModelReferences[i].GetComponent<Animator>().SetFloat("ShowAnswer", _currentAnswer);
                _optionModelReferences[i].GetComponent<Animator>().Play("GameModeAnswers");
            }
            else
            {
                _optionModelReferences[i].GetComponent<Animator>().SetFloat("ShowAllWrongs", pickRandomwWrong);
                _optionModelReferences[i].GetComponent<Animator>().Play("GameModeOptions");
            }
        }
    }
}
