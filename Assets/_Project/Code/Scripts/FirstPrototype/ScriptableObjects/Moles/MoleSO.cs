using Mirror;
using UnityEngine;

[CreateAssetMenu(fileName = "MoleSO", menuName ="Scriptable Objects/Moles/Mole")]
public class MoleSO : ScriptableObject
{
    //Put in info about moles like with buttons
    [SerializeField] private GameObject _moleObjectReference;
    [SyncVar][SerializeField] private bool _isSelected;
    [SyncVar][SerializeField] private bool _allowPress;
    [SerializeField] private Color32 _moleNewColor;
    [SerializeField] private Color32 _MoleOriginalColor;
    public GameObject moleObjectReference
    {
        get
        {
            return _moleObjectReference;
        }
    }
    public bool isSelected
    {
        get
        {
            return _isSelected;
        }
        set
        {
            _isSelected = value;
        }
    }
    public bool allowPress
    {
        get
        {
            return _allowPress;
        }
        set
        {
            _allowPress = value;
        }
    }
    public Color32 newColor
    {
        get
        {
            return _moleNewColor;
        }
    }
    public Color32 originalColor
    {
        get
        {
            return _MoleOriginalColor;
        }
    }
}
