using UnityEngine;
using TMPro;

public class MoleUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _debugText;
    [SerializeField] private TextMeshProUGUI _resultText;
    public void DisplayDebugText(string text)
    {
        _debugText.text = text;
    }
    public void DisplayResultText(string text)
    {
        _resultText.text = text;
    }
}
