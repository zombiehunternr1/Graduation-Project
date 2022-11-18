using Mirror;
using UnityEngine;

[CreateAssetMenu(fileName = "MoleSO", menuName ="Scriptable Objects/Moles/Mole")]
public class MoleSO : ScriptableObject
{
    [SerializeField] private GameObject _moleObjectReference;
    [SyncVar][SerializeField] Vector3 _position;
    [SyncVar][SerializeField] private bool _isCooldownFinished;
    [SyncVar][SerializeField] private bool _allowPress;
    [SyncVar][SerializeField] private bool _isWacked;
    [SerializeField] private Color32 _moleNewColor;
    [SerializeField] private Color32 _MoleOriginalColor;
    [SerializeField] private Color32 _moleWackedColor;

    public GameObject moleObjectReference
    {
        get
        {
            return _moleObjectReference;
        }
    }
    public Vector3 molePosition
    {
        get
        {
            return _position;
        }
        set
        {
            _position = value;
        }
    }
    public bool isCooldownFinished
    {
        get
        {
            return _isCooldownFinished;
        }
        set
        {
            _isCooldownFinished = value;
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
    public bool isWacked
    {
        get
        {
            return _isWacked;
        }
        set
        {
            _isWacked = value;
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
    public Color32 moleWackedColor
    {
        get
        {
            return _moleWackedColor;
        }
    }
}
