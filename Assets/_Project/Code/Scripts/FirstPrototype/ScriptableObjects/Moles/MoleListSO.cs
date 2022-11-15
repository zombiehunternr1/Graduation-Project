using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="MoleListSO", menuName ="Scriptable Objects/Moles/Mole List")]
public class MoleListSO : ScriptableObject
{
    [SerializeField] private List<MoleSO> _molesList;
    public List<MoleSO> molesList
    {
        get
        {
            return _molesList;
        }
    }
}
