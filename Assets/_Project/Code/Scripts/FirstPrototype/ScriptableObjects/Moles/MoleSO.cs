using Mirror;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "MoleSO", menuName ="Scriptable Objects/Moles/Mole")]
public class MoleSO : ScriptableObject
{
    //Put in info about moles like with buttons
    [SerializeField] private GameObject _moleObjectReference;
    [SyncVar][SerializeField] private bool _isCooldownFinished;
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
