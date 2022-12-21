using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class FindTheMatchNetworkManager : NetworkBehaviour
{
    [SerializeField] private Renderer _stageRenderer;
    [SerializeField] private List<FindTheMatchNetworkObject> _matchObjects;

    public void Show(bool show)
    {
        _stageRenderer.enabled = show;
        foreach(FindTheMatchNetworkObject matchObject in _matchObjects)
        {
            matchObject.SetActiveState(show);
        }
    }
    public void SetPositionAndRotation(Vector3 position, Quaternion quaternion)
    {
        transform.SetPositionAndRotation(position, quaternion);
    }
    public void SetupGame()
    {
        foreach (FindTheMatchNetworkObject matchObject in _matchObjects)
        {
            matchObject.SetupGame(isServer);
        }
    }
}
