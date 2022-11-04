using UnityEngine;

[CreateAssetMenu(fileName = "Button", menuName = "Scriptable Objects/Testing/Button Test")]
public class ButtonSO : ScriptableObject
{
    [SerializeField] private GameObject _cubeReference;
    [SerializeField] private bool _isSelected;
    [SerializeField] private Color32 _cubeNewColor;
    [SerializeField] private Color32 _cubeOriginalColor;
    public GameObject CubeReference
    {
        get
        {
            return _cubeReference;
        }
    }
    public bool SetSelected
    {
        set
        {
            _isSelected = value;
        }
    }
    public bool IsSelected
    {
        get
        {
            return _isSelected;
        }
    }
    public Color32 NewCubeColor
    {
        get
        {
            return _cubeNewColor;
        }
    }
    public Color32 OriginalColor
    {
        get
        {
            return _cubeOriginalColor;
        }
    }
}
