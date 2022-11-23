using UnityEngine;

public class UpdateMoleColliderStatus : MonoBehaviour
{
    [SerializeField] Collider _moleCollider;
    public void EnableCollider()
    {
        _moleCollider.enabled = true;
    }
    public void DisableCollider()
    {
        _moleCollider.enabled = false;
    }
}
