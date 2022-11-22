using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkMoleManager : MonoBehaviour
{
    [SerializeField] private DebugEvent _debugEvent;
    [SerializeField] private CmdUpdateUITextEvent _cmdUpdateUITextEvent;
    [SerializeField] private CmdResetAllMolesEvent _cmdResetAllMolesEvent;
    [SerializeField] private CmdMoleWackedEvent _cmdMoleWackedEvent;
    [SerializeField] private CmdMoleUpdateColorEvent _cmdMoleUpdateEvent;
    [SerializeField] private CmdRestartMoleGameEvent _cmdRestartMoleGameEvent;
    [SerializeField] private List<NetworkMole> _networkMoles;
    [SerializeField] private List<Transform> _spawnPositions;
    [SerializeField] private MoleListSO _moleListSO;
    private int _moleIndex;
    private int _previousIndex = -1;
    public List<NetworkMole> networkMoles
    {
        get
        {
            return _networkMoles;
        }
    }
    public List<Transform> spawnPositions
    {
        get
        {
            return _spawnPositions;
        }
    }
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
            foreach(NetworkMole mole in _networkMoles)
            {
                if (!mole.isWacked)
                {
                    return false;
                }
            }
            return true;
        }
    }
    private void Start()
    {
        Show(false);
    }
    public void StartGame()
    {
        foreach(NetworkMole networkMole in _networkMoles)
        {
            networkMole.moleRenderer.material.color = networkMole.moleOriginalColor;
        }
        StartCoroutine(RandomMoleCooldown());
    }
    public void SetPositionAndRotation(Vector3 pos, Quaternion rot)
    {
        transform.SetPositionAndRotation(pos, rot);
        for(int i = 0; i < _networkMoles.Count; i++)
        {
            _networkMoles[i].transform.SetPositionAndRotation(_spawnPositions[i].position, rot);
        }
    }
    public void Show(bool show)
    {
        for(int i = 0; i < _networkMoles.Count; i++)
        {
            _networkMoles[i].moleCollider.enabled = show;
            _networkMoles[i].moleRenderer.enabled = show;
        }
    }
    public void ResetMoles()
    {
        _previousIndex = -1;
        StartCoroutine(WaitBeforeReset());
    }
    public void OnMolePressed(string mole)
    {
        for(int i = 0; i < _networkMoles.Count; i++)
        {
            if (_networkMoles[i].trackername == mole)
            {
                if (_networkMoles[i].isAllowedPress)
                {
                    _cmdMoleWackedEvent.Invoke(_networkMoles[i].trackername);
                    _cmdMoleUpdateEvent.Invoke(_networkMoles[i].trackername, _networkMoles[i].moleWackedColor);
                }
                return;
            }
        }
    }
    public void CheckAllMolesWacked()
    {
        if (allMolesWacked)
        {
            _cmdUpdateUITextEvent.Invoke("All moles have been wacked!");
            StartCoroutine(WaitTillRestartGame()); 
        }
    }
    private IEnumerator WaitTillRestartGame()
    {
        yield return new WaitForSeconds(3);
        _cmdRestartMoleGameEvent.Invoke();
    }
    private IEnumerator RandomMoleCooldown()
    {
        while (_moleIndex != _previousIndex)
        {
            _previousIndex = _moleIndex;
            _moleIndex = randomMoleIndex;
            if (!_networkMoles[_moleIndex].isWacked)
            {
                if (_networkMoles[_moleIndex].isCooldownFinished)
                {
                    _cmdMoleUpdateEvent.Invoke(_networkMoles[_moleIndex].trackername, _networkMoles[_moleIndex].moleOriginalColor);
                    yield return new WaitForSeconds(1.5f);
                }
                else
                {
                    _cmdMoleUpdateEvent.Invoke(_networkMoles[_moleIndex].trackername, _networkMoles[_moleIndex].molePopOutColor);
                    yield return new WaitForSeconds(1.5f);
                }
            }
        }
        if (!_networkMoles[_previousIndex].isWacked)
        {
            if (_networkMoles[_previousIndex].isCooldownFinished)
            {
                _cmdMoleUpdateEvent.Invoke(_networkMoles[_previousIndex].trackername, _networkMoles[_previousIndex].moleOriginalColor);
            }
        }
        _previousIndex = -1;
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(RandomMoleCooldown());
    }
    private IEnumerator WaitBeforeReset()
    {
        yield return new WaitForSeconds(3);
        _cmdResetAllMolesEvent.Invoke();
        _cmdUpdateUITextEvent.Invoke(null);
        yield return new WaitForSeconds(1);
    }
}
