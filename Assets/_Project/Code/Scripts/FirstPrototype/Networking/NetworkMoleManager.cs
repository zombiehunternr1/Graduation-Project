using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class NetworkMoleManager : NetworkBehaviour
{
    [SerializeField] private List<GameObject> _moleObjectReferences;
    private List<Renderer> _moleRenderers;
    [SyncVar][SerializeField] private List<string> _trackerNames;
    public List<string> trackernames
    {
        get
        {
            return _trackerNames;
        }
    }
    private void Start()
    {
        _trackerNames = new List<string>();
        _moleRenderers = new List<Renderer>();
        foreach(GameObject moleObject in _moleObjectReferences)
        {
            _trackerNames.Add(moleObject.name);
            Renderer moleRenderer = moleObject.GetComponent<Renderer>();
            _moleRenderers.Add(moleRenderer);
            moleRenderer.enabled = false;
        }
    }
    public void Show(bool show)
    {
        foreach(Renderer moleRender in _moleRenderers)
        {
            moleRender.enabled = show;
        }
    }
    public void SetPositionAndRotation(Vector3 pos, Quaternion rot)
    {
        transform.SetPositionAndRotation(pos, rot);
    }
}
