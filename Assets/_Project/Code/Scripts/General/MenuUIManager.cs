using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIManager : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI _debugField;
    [SerializeField] private TextMeshProUGUI _resultField;
    [SerializeField] private TextMeshProUGUI _timerField;
    [SerializeField] private TextMeshProUGUI _clientInformationField;
    [SerializeField] private Button _startBtn;
    [SerializeField] private RectTransform _menuUITransform;
    [SerializeField] private RpcStartGameEvent _rpcStartGameEvent;
    private void Start()
    {
        if (!isServer)
        {
            DisableStartBtn();
        }
        else
        {
            _clientInformationField.gameObject.SetActive(false);
        }
    }
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
        _rpcStartGameEvent.Invoke();
        DisableStartBtn();
        DisableMenuUI();
    }
    public void DisableMenuUI()
    {
        _menuUITransform.gameObject.SetActive(false);
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
}
