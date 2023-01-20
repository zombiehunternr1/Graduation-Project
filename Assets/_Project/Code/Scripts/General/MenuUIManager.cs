using Mirror;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIManager : NetworkBehaviour
{
    [Header("Text references")]
    [SerializeField] private TextMeshProUGUI _debugField;
    [SerializeField] private TextMeshProUGUI _howToSeeARObjectsText;
    [SerializeField] private TextMeshProUGUI _howToPlayInstructionText;
    [SerializeField] private TextMeshProUGUI _gameModeInstructionText;
    [SerializeField] private TextMeshProUGUI _clientInformationText;
    [Header("Image references")]
    [SerializeField] private RawImage _howToPlayInstructionImgReference;
    [SerializeField] private Texture2D _howToPlayMimicTexture;
    [SerializeField] private Texture2D _howToPlayGuessTexture;
    [SerializeField] private GameObject _objectsInUI;
    [Header("Button references")]
    [SerializeField] private Button _tryAgainBtn;
    [SerializeField] private Button _returnToMenuBtn;
    [SerializeField] private List<Button> _gameModeBtns;
    [Header("Rect transform References")]
    [SerializeField] private RectTransform _menuUITransform;
    [SerializeField] private RectTransform _gameModeUITransform;
    [SerializeField] private List<RectTransform> _menuUIPagesTransforms;
    [Header("Events")]
    [SerializeField] private RpcStartGameEvent _rpcStartGameEvent;
    private bool _isDancing;
    private void Start()
    {
        if (!isServer)
        {
            _howToPlayInstructionText.text = "Guess the correct answer your partner is mimicking!";
            _howToPlayInstructionImgReference.texture = _howToPlayGuessTexture;
            _howToSeeARObjectsText.text = "Hold your phone's camera in front of the QR-Code to see the options";
            _gameModeInstructionText.gameObject.SetActive(false);
            foreach(Button gameModeBtn in _gameModeBtns)
            {
                gameModeBtn.enabled = false;
            }
        }
        else
        {
            _howToPlayInstructionText.text = "Mimic the answer before the time runs out!";
            _howToPlayInstructionImgReference.texture = _howToPlayMimicTexture;
            _howToSeeARObjectsText.text = "Hold your phone's camera in front of the QR-Code to see the answer";
            _clientInformationText.gameObject.SetActive(false);
        }
    }
    public void DisplayDebug(string text)
    {
        _debugField.text = text;
    }
    public void StartGameMode(bool currentGameMode)
    {
        _isDancing = currentGameMode;
        _rpcStartGameEvent.Invoke(_isDancing);
        DisableGameOverButtons();
        DisableMenuUI();
    }
    public void TryAgain()
    {
        _rpcStartGameEvent.Invoke(_isDancing);
        DisableGameOverButtons();
        DisableMenuUI();
    }
    public void ReturnToMenu()
    {
        DisableGameOverButtons();
        foreach (RectTransform menuUITransform in _menuUIPagesTransforms)
        {
            menuUITransform.gameObject.SetActive(false);
        }
        _gameModeUITransform.gameObject.SetActive(true);
        _menuUITransform.gameObject.SetActive(true);
        _objectsInUI.SetActive(true);
    }
    public void DisableMenuUI()
    {
        _menuUITransform.gameObject.SetActive(false);
        _objectsInUI.SetActive(false);
    }
    public void DisableGameOverButtons()
    {
        _tryAgainBtn.gameObject.SetActive(false);
        _returnToMenuBtn.gameObject.SetActive(false);
    }
    public void EnableGameOverButtons()
    {
        _tryAgainBtn.gameObject.SetActive(true);
        _returnToMenuBtn.gameObject.SetActive(true);
    }   
}
