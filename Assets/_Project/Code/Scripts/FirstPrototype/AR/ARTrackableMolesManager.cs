using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARTrackableMolesManager : NetworkBehaviour
{
    [SerializeField] private DebugEvent _debugEvent;
    [SerializeField] private ARTrackedImageManager _aRTrackedImageManager;
    [SerializeField] private List<GameObject> _moleObjects = new List<GameObject>();
    private List<NetworkMoles> _spawnedNetworkMoles = new List<NetworkMoles>();
    private void Start()
    {
        if (isServer)
        {
            foreach (GameObject spawnableObject in _moleObjects)
            {
                NetworkServer.Spawn(spawnableObject);
            }
        }
        _spawnedNetworkMoles.AddRange(FindObjectsOfType<NetworkMoles>());
    }
    private void OnEnable()
    {
        _aRTrackedImageManager.trackedImagesChanged += OnImageChanged;
    }
    private void OnDisable()
    {
        _aRTrackedImageManager.trackedImagesChanged -= OnImageChanged;
    }
    private void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (ARTrackedImage trackedImage in args.added)
        {
            UpdateImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in args.updated)
        {
            UpdateImage(trackedImage);
        }
    }
    private void UpdateImage(ARTrackedImage trackedImage)
    {
        foreach(NetworkMoles networkmole in _spawnedNetworkMoles)
        {
            _debugEvent.Invoke(networkmole.trackername);
            if (networkmole.trackername == trackedImage.referenceImage.name)
            {
                if (trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Limited)
                {
                    networkmole.Show(false);
                    return;
                }
                networkmole.SetPositionAndRotation(trackedImage.transform.position, transform.transform.rotation);
                networkmole.Show(true);
            }
        }
    }
}
