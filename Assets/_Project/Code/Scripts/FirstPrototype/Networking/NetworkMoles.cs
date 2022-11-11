using Mirror;
using UnityEngine;

public class NetworkMoles : NetworkBehaviour
{
    [SerializeField] private GameObject _moleObjectPrefab;
    private GameObject _moleObjectInstance;
    private MeshRenderer _meshRenderer;
    [SyncVar][SerializeField] private string _trackerName;
    public string trackername
    {
        get
        {
            return _trackerName;
        }
    }
    private void OnEnable()
    {
        _moleObjectInstance = Instantiate(_moleObjectPrefab);
        _moleObjectInstance.name = _moleObjectInstance.name.Replace("(Clone)", "");
        _trackerName = _moleObjectInstance.name;
    }
    public void Show(bool show)
    {
        _meshRenderer.enabled = show;
    }
    public void SetPositionAndRotation(Vector3 pos, Quaternion rot)
    {
        _moleObjectInstance.transform.SetPositionAndRotation(pos, rot);
    }
}
