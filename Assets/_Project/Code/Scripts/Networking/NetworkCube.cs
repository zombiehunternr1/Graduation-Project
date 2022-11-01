using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkCube : NetworkBehaviour
{
    [SerializeField] private UpdatePressedNetworkCubeEvent _cubePressed;
    [SerializeField] private GameObject _cubePrefab;
    private string _trackerName;
    private bool _isReset = false;
    private GameObject _cubeInstance;
    private MeshRenderer _meshRenderer;
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
        _meshRenderer = _cubeInstance.GetComponent<MeshRenderer>();
        _cubeOriginalColor = _meshRenderer.material.color;
        _cubeInstance.SetActive(false);
        if (isServer)
        {
            _cubeColor = _cubeInstance.GetComponent<MeshRenderer>().material.color;
        }
        else
        {
            ChangeColor();
        }
    }
    public void SetPositionAndRotation(Vector3 pos, Quaternion rot)
    {
        _cubeInstance.transform.SetPositionAndRotation(pos, rot);
    }
    public void Show(bool shown)
    {
        _cubeInstance.SetActive(shown);
    }
    public void OnScreenTapped(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 postion = context.ReadValue<Vector2>();
            Ray ray = Camera.main.ScreenPointToRay(postion);
            if(Physics.Raycast(ray, out RaycastHit hit, 200))
            {
                if(hit.collider.gameObject == _cubeInstance)
                {
                    _cubeColor = Color.black;
                    CmdChangeColor();
                    _cubePressed.Invoke(_trackerName);
                }
            }
        }
    }
    public void ResetColor()
    {
        _isReset = true;
        CmdChangeColor();
    }
    private void ChangeColor()
    {
        if (_isReset)
        {
            _meshRenderer.material.color = _cubeOriginalColor;
            _isReset = false;
        }
        else
        {
            _meshRenderer.material.color = _cubeColor;
        }
    }
    [ClientRpc]
    private void RpcChangeColor()
    {
        ChangeColor();
    }
    [Command(requiresAuthority = false)]
    private void CmdChangeColor()
    {
        RpcChangeColor();
    }
}
