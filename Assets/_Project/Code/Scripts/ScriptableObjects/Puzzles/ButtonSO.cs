using UnityEngine;

[CreateAssetMenu(fileName = "Button", menuName = "Scriptable Objects/Testing/Button Test")]
public class ButtonSO : ScriptableObject
{
    [SerializeField] private GameObject _cubeReference;
    [SerializeField] private bool _isSelected;

    public GameObject CubeReference
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
    public bool IsSelected
    {
        get
        {
            return _isSelected;
        }
    }
}
