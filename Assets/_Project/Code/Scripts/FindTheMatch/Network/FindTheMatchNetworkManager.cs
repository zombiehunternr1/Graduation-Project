using Mirror;
using UnityEngine;

public class FindTheMatchNetworkManager : NetworkBehaviour
{
    [SerializeField] FindTheMatchNetworkObject _matchObject;
    public void SetPositionAndRotation(Vector3 position, Quaternion quaternion)
    {
        transform.SetPositionAndRotation(position, quaternion);
    }
    public void SetupGame()
    {
        _matchObject.SetupGame(isServer);
    }
    public void Show(bool show)
    {
        _matchObject.SetActiveState(show);
    }
}
