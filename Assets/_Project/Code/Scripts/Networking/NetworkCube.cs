using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkCube : NetworkBehaviour
{
    [SerializeField] private DebugEvent _debugEvent;
    [SerializeField] private NetworkCubeManager _networkCubeManager;
    [SerializeField] private ButtonSO _buttonSO;
    [SerializeField] private UpdatePressedNetworkCubeEvent _cubePressed;
    [SerializeField] private GameObject _cubePrefab;
    [SyncVar] [SerializeField] private string _trackerName;
    private bool _isReset = false;
    private bool _allowPress = true;
    private GameObject _cubeInstance;
    private Renderer _cubeRenderer;
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
            _cubeOriginalColor = _buttonSO.OriginalColor;
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
    public void OnScreenTapped(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 postion = context.ReadValue<Vector2>();
            Ray ray = Camera.main.ScreenPointToRay(postion);
            if(Physics.Raycast(ray, out RaycastHit hit, 200))
            {
                if(hit.collider.gameObject == _cubeInstance && _allowPress)
                {
                    _allowPress = false;
                    _networkCubeManager.CmdChangeColor(_buttonSO.NewCubeColor);
                    _cubePressed.Invoke(_trackerName);
                }
            }
        }
    }
    public void ResetColor()
    {
        _isReset = true;
        if(_buttonSO != null)
        {
            _networkCubeManager.CmdChangeColor(_buttonSO.OriginalColor);
        }
        _allowPress = true;
    }
    public void ChangeColor(Color32 color)
    {
        if (_isReset)
        {
            _isReset = false;
            _cubeOriginalColor = color;
            _cubeRenderer.material.color = _cubeOriginalColor;
        }
        else
        {
            if(_buttonSO != null)
            {
                _cubeColor = color;
                _cubeRenderer.material.color = _cubeColor;
            }
        }
    }
}
