using Mirror;
using System.Drawing;
using UnityEngine;
public class NetworkCube : NetworkBehaviour
{
    [SerializeField] private DebugEvent _debugEvent;
    [SerializeField] private ButtonSO _buttonSO;
    [SerializeField] private GameObject _cubePrefab;
    private GameObject _cubeInstance;
    private Renderer _cubeRenderer;
    [SyncVar][SerializeField] private string _trackerName;
    [SyncVar] private Color32 _cubeColor;
    [SyncVar] private Color32 _cubeOriginalColor;
    public string TrackerName
    {
        get
        {
            return _trackerName;
        }
    }
    private void Start()
    {
        _cubeInstance = Instantiate(_cubePrefab);
        _cubeInstance.name = _cubeInstance.name.Replace("(Clone)", "");
        _trackerName = _cubeInstance.name;
        _cubeRenderer = _cubeInstance.GetComponent<Renderer>();
        if(_buttonSO != null)
        {
            _cubeOriginalColor = _buttonSO.originalColor;
        }
        _cubeRenderer.enabled = false;
        if (isServer)
        {
            _cubeColor = _cubeOriginalColor;
            _cubeRenderer.material.color = _cubeColor;
        }
    }
    public void SetPositionAndRotation(Vector3 pos, Quaternion rot)
    {
        _cubeInstance.transform.SetPositionAndRotation(pos, rot);
    }
    public void Show(bool shown)
    {
        _cubeRenderer.enabled = shown;
    }
    public void UpdateColor(bool isReset)
    {
        if(_buttonSO != null)
        {
            if (isReset)
            {
                _cubeOriginalColor = _buttonSO.originalColor;
                _cubeRenderer.material.color = _cubeOriginalColor;
            }
            else if(_buttonSO.isSelected)
            {
                _cubeColor = _buttonSO.newCubeColor;
                _cubeRenderer.material.color = _cubeColor;
            }
        }
    }
}
