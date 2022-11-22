using Mirror;
using System.Collections;
using UnityEngine;

public class NetworkMole : NetworkBehaviour
{
    [SerializeField] private DebugEvent _debugEvent;
    [SerializeField] private CmdCheckAllMolesWackedEvent _cmdCheckAllMolesWackedEvent;
    [SerializeField] private MoleSO _moleSO;
    private Renderer _moleRenderer;
    private Collider _moleCollider;
    [SyncVar] private string _moleId;
    [SyncVar] private string _trackerName;
    [SyncVar] private Color32 _moleColor;
    [SyncVar] private Color32 _molePopOutColor;
    [SyncVar] private Color32 _moleOriginalColor;
    [SyncVar] private Color32 _moleWackedColor;
    [SyncVar] private bool _isCooldownFinished;
    [SyncVar] private bool _allowPress;
    [SyncVar] private bool _isWacked;
    public string moleId
    {
        get
        {
            return _moleId;
        }
    }
    public string trackername
    {
        get
        {
            return _trackerName;
        }
    }
    public Color32 moleColor
    {
        get
        {
            return _moleColor;
        }
        private set
        {
            _moleColor = value;
        }
    }
    public Color32 moleOriginalColor
    {
        get
        {
            return _moleOriginalColor;
        }
        private set
        {
            _moleOriginalColor = value;
        }
    }
    public Color32 molePopOutColor
    {
        get
        {
            return _molePopOutColor;
        }
        private set
        {
            _molePopOutColor = value;
        }
    }
    public Color32 moleWackedColor
    {
        get
        {
            return _moleWackedColor;
        }
        private set
        {
            _moleWackedColor = value;
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
    public bool isWacked
    {
        get
        {
            return _isWacked;
        }
        private set
        {
            _isWacked = value;
        }
    }
    public bool isCooldownFinished
    {
        get
        {
            return _isCooldownFinished;
        }
        private set
        {
            _isCooldownFinished = value;
        }
    }
    public bool isAllowedPress
    {
        get
        {
            return _allowPress;
        }
        private set
        {
            _allowPress = value;
        }
    }
    private void OnEnable()
    {
        _moleRenderer = GetComponent<Renderer>();
        _moleCollider = GetComponent<Collider>();
        _moleRenderer.enabled = false;
        _moleCollider.enabled = false;
        if(_moleSO != null)
        {
            _moleId = _moleSO.moleId;
            _trackerName = _moleSO.moleObjectReference.name;
        }
    }
    private void Start()
    {
        transform.SetPositionAndRotation(transform.parent.position, transform.parent.rotation);
        if (_moleSO != null)
        {
            SetDefaultSettings();
        }
    }
    public void WackedMole(string moleName)
    {
        if(moleName == _trackerName)
        {
            StopAllCoroutines();
            isWacked = true;
            _cmdCheckAllMolesWackedEvent.Invoke();
        }
    }
    public void ResetMole()
    {
        if (_moleSO != null)
        {
            SetDefaultSettings();
        }
    }
    public void UpdateColor(string moleName, Color32 moleColor)
    {
        if (_trackerName == moleName)
        {
            if (isAllowedPress)
            {
                isAllowedPress = false;
            }
            if (isWacked)
            {
                StopAllCoroutines();
                isCooldownFinished = false;
                _moleColor = moleColor;
                _moleRenderer.material.color = _moleColor;
                return;
            }
            if (isCooldownFinished)
            {
                isCooldownFinished = false;
                _moleColor = moleColor;
                _moleRenderer.material.color = _moleColor;
                return;
            }
            else
            {
                _moleColor = moleColor;
                _moleRenderer.material.color = _moleColor;
                StartCooldown();
            }
        }
    }
    public void StartCooldown()
    {
        StartCoroutine(CoolDownRoutine());
    }
    private void SetDefaultSettings()
    {
        moleWackedColor = _moleSO.moleWackedColor;
        molePopOutColor = _moleSO.moleNewColor;
        moleOriginalColor = _moleSO.moleOriginalColor;
        moleColor = _moleOriginalColor;
        moleRenderer.material.color = _moleSO.moleOriginalColor;
        isAllowedPress = _moleSO.allowPress;
        isCooldownFinished = _moleSO.isCooldownFinished;
        isWacked = _moleSO.isWacked;
    }
    private IEnumerator CoolDownRoutine()
    {
        isAllowedPress = true;
        isCooldownFinished = false;
        yield return new WaitForSeconds(3);
        _moleRenderer.material.color = _moleOriginalColor;
        isCooldownFinished = true;
        _allowPress = false;
    }
}
