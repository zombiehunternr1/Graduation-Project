using Mirror;
using UnityEngine;

public class NetworkMole : NetworkBehaviour
{
    [SerializeField] private DebugEvent _debugEvent;
    [SerializeField] private MoleSO _moleSO;
    private Renderer _moleRenderer;
    private Collider _moleCollider;
    [SyncVar][SerializeField] private string _trackerName;
    [SyncVar] private Color32 _moleColor;
    [SyncVar] private Color32 _moleOriginalColor;
    public string trackername
    {
        get
        {
            return _trackerName;
        }
    }
    public Renderer moleRenderer
    {
        get
        {
            return _moleRenderer;
        }
        set
        {
            _moleRenderer = value;
        }
    }
    public Collider moleCollider
    {
        get
        {
            return _moleCollider;
        }
        set
        {
            _moleCollider = value;
        }
    }
    private void OnEnable()
    {
        _moleRenderer = GetComponent<Renderer>();
        _moleCollider = GetComponent<Collider>();
        _trackerName = gameObject.name;
    }
    private void Start()
    {
        if(_moleSO != null)
        {
            _moleOriginalColor = _moleSO.originalColor;
        }
        if (isServer)
        {
            _moleColor = _moleOriginalColor;
            _moleRenderer.material.color = _moleColor;
        }
    }
    public void UpdateColor()
    {
        if (_moleSO != null)
        {
            if (_moleSO.allowPress)
            {
                _moleOriginalColor = _moleSO.newColor;
                _moleRenderer.material.color = _moleOriginalColor;
            }
            else
            {
                _moleColor = _moleSO.originalColor;
                _moleRenderer.material.color = _moleColor;
            }
        }
    }
}
