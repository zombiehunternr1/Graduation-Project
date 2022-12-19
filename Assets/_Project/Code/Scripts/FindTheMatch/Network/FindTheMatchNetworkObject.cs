using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FindTheMatchNetworkObject : MonoBehaviour
{
    [SerializeField] private DebugEvent _debugEvent;
    [SerializeField] private CmdRequestAnswerFromServerEvent _cmdRequestAnswerFromServerEvent;
    [SerializeField] private CmdSendAnswerEvent _cmdSendAnswerEvent;
    [SerializeField] private List<Collider> _colliderOptions;
    [SerializeField] private List<GameObject> _optionModelReferences;
    [SerializeField] private GameObject _answerModelReference;
    [HideInInspector][SerializeField] private List<Renderer> _optionModelRenderers;
    [HideInInspector][SerializeField] private List<Renderer> _answerModelRenderers;
    [SerializeField] private float _crossFadingSpeed = 0.1f;
    private List<int> _wrongOptionsList;
    private bool _gameStarted = false;
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
    public void StartGame()
    {
        _gameStarted = true;
    }
    public void StopGame()
    {
        _gameStarted = false;
    }
    public void SetActiveState(bool value)
    {
        if (_isServer)
        {
            if (!_gameStarted)
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
                if (_gameStarted)
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
                if (_gameStarted)
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
        _wrongOptionsList = new List<int>();
        if (isServer)
        {
            _isServer = isServer;
            _currentAnswer = pickRandomAnswer;
            if(_answerModelRenderers.Count == 0)
            {
                _answerModelRenderers.AddRange(_answerModelReference.GetComponentsInChildren<Renderer>());
            }
            _answerModelReference.GetComponent<Animator>().SetFloat("ShowAnswer", _currentAnswer);
            _answerModelReference.GetComponent<Animator>().Play("Answer");

        }
        if(_optionModelRenderers.Count == 0)
        {
            foreach (GameObject optionModel in _optionModelReferences)
            {
                _optionModelRenderers.AddRange(optionModel.GetComponentsInChildren<Renderer>());
            }
        }
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
        _answerModelReference.GetComponent<Animator>().SetFloat("ShowResult", value);
        _answerModelReference.GetComponent<Animator>().CrossFade("Result", _crossFadingSpeed);
        for(int i = 0; i < _optionModelReferences.Count; i++)
        {
            _optionModelReferences[i].GetComponent<Animator>().SetFloat("ShowResult", value);
            _optionModelReferences[i].GetComponent<Animator>().CrossFade("Result", _crossFadingSpeed);
        }
    }
    private void DisplayOptions()
    {
        for (int i = 0; i < _optionModelReferences.Count; i++)
        {
            if (i == _currentAnswer)
            {
                _optionModelReferences[i].GetComponent<Animator>().SetFloat("ShowAnswer", _currentAnswer);
                _optionModelReferences[i].GetComponent<Animator>().Play("Answer");
            }
            else
            {
                _optionModelReferences[i].GetComponent<Animator>().SetFloat("ShowAllWrongs", pickRandomwWrong);
                _optionModelReferences[i].GetComponent<Animator>().Play("WrongAnswers");
            }
        }
    }
}
