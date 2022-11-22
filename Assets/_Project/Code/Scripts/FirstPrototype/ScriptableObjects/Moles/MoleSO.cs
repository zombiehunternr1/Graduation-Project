using UnityEngine;

[CreateAssetMenu(fileName = "MoleSO", menuName ="Scriptable Objects/Moles/Mole")]
public class MoleSO : ScriptableObject
{
    [UniqueIdentifier][SerializeField] private string _moleId;
    [SerializeField] private GameObject _moleObjectReference;
    [SerializeField] private bool _isCooldownFinished;
    [SerializeField] private bool _allowPress;
    [SerializeField] private bool _isWacked;
    [SerializeField] private Color32 _moleNewColor;
    [SerializeField] private Color32 _MoleOriginalColor;
    [SerializeField] private Color32 _moleWackedColor;
    public string moleId
    {
        get
        {
            return _moleId;
        }
    }
    public GameObject moleObjectReference
    {
        get
        {
            return _moleObjectReference;
        }
    }
    public bool isCooldownFinished
    {
        get
        {
            return _isCooldownFinished;
        }
    }
    public bool allowPress
    {
        get
        {
            return _allowPress;
        }
    }
    public bool isWacked
    {
        get
        {
            return _isWacked;
        }
    }
    public Color32 moleNewColor
    {
        get
        {
            return _moleNewColor;
        }
    }
    public Color32 moleOriginalColor
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
