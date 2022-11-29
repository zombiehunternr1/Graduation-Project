using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkFirstARPuzzlePlayer : NetworkBehaviour
{
    [SerializeField] private DebugEvent _debugEvent;
    [SerializeField] private CmdShapeSelectedStatusEvent _CmdShapeSelectedStatusEvent;
    [SerializeField] private List<StarInfoSO> _allStarInfoSOList;
    public void OnScreenTapped(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 postion = context.ReadValue<Vector2>();
            Ray ray = Camera.main.ScreenPointToRay(postion);
            if (Physics.Raycast(ray, out RaycastHit hit, 200))
            {
                _debugEvent.Invoke(hit.collider.ToString());
                if (hit.collider.gameObject != null)
                {
                    _debugEvent.Invoke(hit.collider.gameObject.name);
                    NetworkStarShape starShape = hit.collider.GetComponent<NetworkStarShape>();
                    if (starShape.starInfo.isSelected)
                    {
                        _debugEvent.Invoke("This star was already picked up by another player");
                        return;
                    }
                    starShape.starInfo.isSelected = true;
                    _debugEvent.Invoke("Pressed the star");
                    string shapeName = hit.collider.GetComponent<NetworkStarShape>().trackerName;
                    _CmdShapeSelectedStatusEvent.Invoke(shapeName, starShape.starInfo.isSelected);
                    return;
                }
            }
        }
    }
    #region FirstARPuzzleManager
    [ClientRpc]
    private void RpcShapeSelectedStatus(string shapeName, bool value)
    {
        foreach(StarInfoSO starInfoSO in _allStarInfoSOList)
        {
            if(starInfoSO.name == shapeName)
            {
                starInfoSO.isSelected = value;
                return;
            }
        }
    }
    [Command(requiresAuthority = false)]
    public void CmdShapeSelectedStatus(string shapeName, bool value)
    {
        RpcShapeSelectedStatus(shapeName, value);
    }
    #endregion
}
