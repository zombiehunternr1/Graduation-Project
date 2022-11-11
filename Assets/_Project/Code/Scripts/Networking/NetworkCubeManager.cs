using Mirror;
using UnityEngine;

public class NetworkCubeManager : NetworkBehaviour
{
    [SerializeField] private NetworkCube _networkCube;
    [ClientRpc]
    public void RpcChangeColor(Color32 color)
    {
        //_networkCube.ChangeColor(color);
    }
    [Command(requiresAuthority = false)]
    public void CmdChangeColor(Color32 color)
    {
        RpcChangeColor(color);
    }
}
