using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkCube : NetworkBehaviour
{
    [SerializeField] private GameObject _cubePrefab;
    private string _trackerName;
    private GameObject _cubeInstance;
    MeshRenderer _meshRenderer;
    [SyncVar] private Color _cubeColor;
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
        _meshRenderer = _cubeInstance.GetComponent<MeshRenderer>();
        _trackerName = _cubePrefab.name;
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
                    CmdChangeColor();
                }
            }
        }
    }
    private void ChangeColor()
    {
        _meshRenderer.material.color = _cubeColor;
    }
    [ClientRpc]
    private void RpcChangeColor()
    {
        ChangeColor();
    }
    [Command(requiresAuthority = false)]
    private void CmdChangeColor()
    {
        _cubeColor = Color.black;
        RpcChangeColor();
    }
}
