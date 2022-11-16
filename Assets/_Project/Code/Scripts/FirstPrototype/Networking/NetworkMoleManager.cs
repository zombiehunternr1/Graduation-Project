using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkMoleManager : NetworkBehaviour
{
    [SerializeField] private DebugEvent _debugEvent;
    [SerializeField] private CmdMoleUpdateColorEvent _cmdMoleUpdateEvent;
    [SerializeField] private List<NetworkMole> _networkMoles;
    [SerializeField] private MoleListSO _moleListSO;
    private int _moleIndex;
    private int _previousIndex = -1;
    private int randomMoleIndex
    {
        get
        {
            return Random.Range(0, _networkMoles.Count - 1);
        }
    }
    private void Start()
    {
        StartCoroutine(RandomMoleCooldown());
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
            if (_networkMoles[i].trackername == mole)
            {
                MoleSO moleSO = _moleListSO.molesList[i];
                if (moleSO.allowPress)
                {
                    _debugEvent.Invoke(moleSO.moleObjectReference.name + " is allowed to be pressed");
                    moleSO.allowPress = false;
                    _cmdMoleUpdateEvent.Invoke();
                }
                else
                {
                    _debugEvent.Invoke(moleSO.moleObjectReference.name + " is not allowed to be pressed");
                }
                return;
            }
        }
    }
    private IEnumerator RandomMoleCooldown()
    {
        while (_moleIndex != _previousIndex)
        {
            _previousIndex = _moleIndex;
            _moleIndex = randomMoleIndex;
            if(_moleIndex == _previousIndex)
            {
                if (_moleListSO.molesList[_moleIndex].allowPress)
                {
                    _moleListSO.molesList[_moleIndex].allowPress = false;
                    _cmdMoleUpdateEvent.Invoke();
                    yield return null;
                }
                else
                {
                    _moleListSO.molesList[_moleIndex].allowPress = true;
                    _cmdMoleUpdateEvent.Invoke();
                    yield return new WaitForSeconds(1.5f);
                }
            }
            else
            {
                _moleListSO.molesList[_previousIndex].allowPress = false;
                _cmdMoleUpdateEvent.Invoke();
                yield return null;
            }
        }
        _previousIndex = -1;
        yield return new WaitForSeconds(3);
        StartCoroutine(RandomMoleCooldown());
    }
}
