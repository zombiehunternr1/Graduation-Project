using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARTrackableMolesManager : NetworkBehaviour
{
    [SerializeField] private DebugEvent _debugEvent;
    [SerializeField] private Transform _parentTransform;
    [SerializeField] private ARTrackedImageManager _aRTrackedImageManager;
    [SerializeField] private List<GameObject> _moleObjects = new List<GameObject>();
    private List<NetworkMole> _spawnedNetworkMoles = new List<NetworkMole>();
    private void Start()
    {
        if (isServer)
        {
            foreach (GameObject moleObject in _moleObjects)
            {
                GameObject spawnedMoleObject = Instantiate(moleObject, Vector3.zero, Quaternion.identity);
                spawnedMoleObject.transform.SetParent(_parentTransform);
                NetworkServer.Spawn(spawnedMoleObject);
            }
        }
        _spawnedNetworkMoles.AddRange(FindObjectsOfType<NetworkMole>());
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
        foreach(NetworkMole networkmole in _spawnedNetworkMoles)
        {
            if (networkmole.trackername == trackedImage.referenceImage.name)
            {
                if (trackedImage.trackingState == TrackingState.Limited)
                {
                    networkmole.Show(false);
                }
                else
                {
                    networkmole.SetPositionAndRotation(trackedImage.transform.position, trackedImage.transform.rotation);
                    networkmole.Show(true);
                }
            }
        }
    }
}
