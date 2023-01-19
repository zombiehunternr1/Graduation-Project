using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIManager : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI _debugField;
    [SerializeField] private TextMeshProUGUI _howToSeeARObjectsText;
    [SerializeField] private TextMeshProUGUI _howToPlayInstructionText;
    [SerializeField] private TextMeshProUGUI _clientInformationField;
    [SerializeField] private RawImage _howToPlayInstructionImgReference;
    [SerializeField] private Texture2D _howToPlayMimicTexture;
    [SerializeField] private Texture2D _howToPlayGuessTexture;
    [SerializeField] private Button _startGameBtn;
    [SerializeField] private Button _tryAgainBtn;
    [SerializeField] private RectTransform _menuUITransform;
    [SerializeField] private RpcStartGameEvent _rpcStartGameEvent;
    private void Start()
    {
        if (!isServer)
        {
            _howToPlayInstructionText.text = "Guess the correct answer your partner is mimicking!";
            _howToPlayInstructionImgReference.texture = _howToPlayGuessTexture;
            _howToSeeARObjectsText.text = "Hold your phone's camera in front of the QR-Code to see the options";
            _startGameBtn.gameObject.SetActive(false);
        }
        else
        {
            _howToPlayInstructionText.text = "Mimic the answer before the time runs out!";
            _howToPlayInstructionImgReference.texture = _howToPlayMimicTexture;
            _howToSeeARObjectsText.text = "Hold your phone's camera in front of the QR-Code to see the answer";
            _clientInformationField.gameObject.SetActive(false);
        }
    }
    public void DisplayDebug(string text)
    {
        _debugField.text = text;
    }
    public void StartGame()
    {
        _rpcStartGameEvent.Invoke();
        DisableTryAgainBtn();
        DisableMenuUI();
    }
    public void DisableMenuUI()
    {
        _menuUITransform.gameObject.SetActive(false);
    }
    public void DisableTryAgainBtn()
    {
        _tryAgainBtn.gameObject.SetActive(false);
    }
    public void EnableTryAgainBtn()
    {
        _tryAgainBtn.gameObject.SetActive(true);
    }   
}
