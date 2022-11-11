using Mirror;
using UnityEngine;

public class NetworkMole : NetworkBehaviour
{
    [SerializeField] private GameObject _moleObjectPrefab;
    private GameObject _moleObjectInstance;
    private Renderer _moleRenderer;
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
        _moleRenderer = _moleObjectInstance.GetComponent<Renderer>(); 
        _moleRenderer.enabled = false;
    }
    public void Show(bool show)
    {
        _moleRenderer.enabled = show;
    }
    public void SetPositionAndRotation(Vector3 pos, Quaternion rot)
    {
        _moleObjectInstance.transform.SetPositionAndRotation(pos, rot);
    }
}
