using Mirror;
using UnityEngine;

[CreateAssetMenu(fileName = "Button", menuName = "Scriptable Objects/Testing/Button Test")]
public class ButtonSO : ScriptableObject
{
    [SerializeField] private GameObject _cubeReference;
    [SyncVar] [SerializeField] private bool _isSelected;
    [SyncVar] [SerializeField] private bool _allowPress = true;
    [SerializeField] private Color32 _cubeNewColor;
    [SerializeField] private Color32 _cubeOriginalColor;
    public GameObject cubeReference
    {
        get
        {
            return _cubeReference;
        }
    }
    public bool setSelected
    {
        set
        {
            _isSelected = value;
        }
    }
    public bool isSelected
    {
        get
        {
            return _isSelected;
        }
    }
    public bool setAllowPress
    {
        set
        {
            _allowPress = value;
        }
    }
    public bool isAllowPress
    {
        get
        {
            return _allowPress;
        }
    }
    public Color32 newCubeColor
    {
        get
        {
            return _cubeNewColor;
        }
    }
    public Color32 originalColor
    {
        get
        {
            return _cubeOriginalColor;
        }
    }
}
