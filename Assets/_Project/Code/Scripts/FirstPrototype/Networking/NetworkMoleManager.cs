using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class NetworkMoleManager : NetworkBehaviour
{
    [SerializeField] private DebugEvent _debugEvent;
    [SerializeField] private List<NetworkMole> _networkMoles;
    [SerializeField] private MoleListSO _moleList;
    private void Start()
    {
        Show(false);
    }
    public void SetPositionAndRotation(Vector3 pos, Quaternion rot)
    {
        transform.SetPositionAndRotation(pos, rot);
    }
    public void Show(bool show)
    {
        foreach(NetworkMole mole in _networkMoles)
        {
            mole.moleRenderer.enabled = show;
            mole.moleCollider.enabled = show;
        }
    }
    public void OnMolePressed(string mole)
    {
        for(int i = 0; i < _networkMoles.Count; i++)
        {
            _debugEvent.Invoke("Before if statement: " + _networkMoles[i].trackername + "\n" + "Name of passed through mole: " + mole);
            if (_networkMoles[i].trackername == mole)
            {
                MoleSO moleSO = _moleList.molesList[i];
                _debugEvent.Invoke("Mole SO name is: " + moleSO.moleObjectReference.name);
                return;
            }
        }
    }
}
