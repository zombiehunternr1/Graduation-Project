using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugUIManager : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI _debugField;
    [SerializeField] private TextMeshProUGUI _resultField;
    [SerializeField] private TextMeshProUGUI _timerField;
    [SerializeField] private Button _startBtn;
    [SerializeField] private CmdStartGameEvent _cmdStartGameEvent;
    public void DisplayDebug(string text)
    {
        _debugField.text = text;
    }
    public void DisplayTimer(string text)
    {
        _timerField.text = text;
    }
    public void DisplayResult(string text)
    {
        _resultField.text = text;
    }
    public void StartGame()
    {
        _cmdStartGameEvent.Invoke();
        DisableStartBtn();
    }
    public void DisableStartBtn()
    {
        _startBtn.gameObject.SetActive(false);
    }
    public void EnableStartBtn()
    {
        _startBtn.gameObject.SetActive(true);
        _startBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Try again?";
    }
    private void Start()
    {
        if (!isServer)
        {
            DisableStartBtn();
        }
    }
}
