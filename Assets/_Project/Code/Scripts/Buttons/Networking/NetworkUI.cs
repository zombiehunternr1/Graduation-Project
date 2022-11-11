using Mirror;
using UnityEngine;

public class NetworkUI : MonoBehaviour
{
    public void SetIP(string ipadress)
    {
        NetworkManager.singleton.networkAddress = ipadress;
    }
}
