using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Reflection;

public class LocalPlayerInit : NetworkBehaviour
{
    [SerializeField] private List<Component> _localPlayerComponents = new List<Component>();
    private void Start()
    {
        if (!isLocalPlayer)
        {
            foreach(Component component in _localPlayerComponents)
            {
                //Sets disabled if current player is not a local player
                PropertyInfo info = component.GetType().GetProperty("enabled");
                if (info != null)
                {
                   info.SetValue(component, false, null);
                }
            }
        }
    }
}
