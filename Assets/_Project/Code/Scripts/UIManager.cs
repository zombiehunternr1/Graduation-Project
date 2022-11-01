using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _debugField;
    [SerializeField] private TextMeshProUGUI _timerField;
    public void DisplayDebug(string text)
    {
        _debugField.text = text;
    }
    public void DisplayTimer(string text)
    {
        _timerField.text = text;
    }
}
