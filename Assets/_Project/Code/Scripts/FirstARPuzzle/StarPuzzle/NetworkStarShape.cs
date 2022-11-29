using Mirror;
using UnityEngine;

public class NetworkStarShape : NetworkBehaviour
{
    [SerializeField] private StarInfoSO _starInfoSO;
    [SerializeField] private Renderer _starRenderer;
    public StarInfoSO starInfo
    {
        get
        {
            return _starInfoSO;
        }
    }
    public string trackerName
    {
        get
        {
            return _starInfoSO.name;
        }
    }
    private void OnEnable()
    {
        Show(false);
    }
    public void Show(bool value)
    {
        _starRenderer.enabled = value;
    }
    public void SetPositionAndRotation(Vector3 pos, Quaternion rot)
    {
        transform.SetPositionAndRotation(pos, rot);
    }
}
