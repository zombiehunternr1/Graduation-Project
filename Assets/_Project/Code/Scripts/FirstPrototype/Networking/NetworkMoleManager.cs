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
            return Random.Range(0, _networkMoles.Count);
        }
    }
    private bool allMolesWacked
    {
        get
        {
            foreach(MoleSO moleSO in _moleListSO.molesList)
            {
                if (!moleSO.isWacked)
                {
                    return false;
                }
            }
            return true;
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
                    moleSO.isCooldownFinished = false;
                    moleSO.allowPress = false;
                    moleSO.isWacked = true;
                    _cmdMoleUpdateEvent.Invoke();
                    if (allMolesWacked)
                    {
                        StopAllCoroutines();
                        _debugEvent.Invoke("All moles have been wacked!");
                        StartCoroutine(ResetMoles());
                    }
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
            if (!_moleListSO.molesList[_moleIndex].isWacked)
            {
                if (_moleListSO.molesList[_moleIndex].isCooldownFinished)
                {
                    _moleListSO.molesList[_moleIndex].isCooldownFinished = false;
                    _cmdMoleUpdateEvent.Invoke();
                    yield return null;
                }
                else
                {
                    _networkMoles[_moleIndex].StartCooldown();
                    _cmdMoleUpdateEvent.Invoke();
                    yield return new WaitForSeconds(1.5f);
                }
            }
        }
        _moleListSO.molesList[_previousIndex].allowPress = false;
        _cmdMoleUpdateEvent.Invoke();
        yield return null;
        _previousIndex = -1;
        yield return new WaitForSeconds(3);
        StartCoroutine(RandomMoleCooldown());
    }
    private IEnumerator ResetMoles()
    {
        yield return new WaitForSeconds(3);
        _debugEvent.Invoke(null);
        foreach (MoleSO moleSO in _moleListSO.molesList)
        {
            moleSO.allowPress = false;
            moleSO.isCooldownFinished = false;
            moleSO.isWacked = false;
        }
        _cmdMoleUpdateEvent.Invoke();
        StartCoroutine(RandomMoleCooldown());
    }
}
