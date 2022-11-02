using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Mirror;

public class ARImageTrackingVersion2 : NetworkBehaviour
{
    [SerializeField] private Transform _TrackableObjectReference;
    [SerializeField] private ARTrackedImageManager _aRTrackedImageManager;
    [SerializeField] private List<GameObject> _objectsToSpawn = new List<GameObject>();
    private List<NetworkCube> _spawnedNetworkObjects = new List<NetworkCube>();
    private void Start()
    {
        if (isServer)
        {
            foreach (GameObject spawnableObject in _objectsToSpawn)
            {
                GameObject instantiatedObject = Instantiate(spawnableObject, Vector3.zero, Quaternion.identity);
                instantiatedObject.transform.SetParent(_TrackableObjectReference);
                NetworkServer.Spawn(instantiatedObject);
            }
        }
        _spawnedNetworkObjects.AddRange(FindObjectsOfType<NetworkCube>());
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
        foreach(ARTrackedImage trackedImage in args.added)
        {
            UpdateImage(trackedImage);
        }
        foreach(ARTrackedImage trackedImage in args.updated)
        {
            UpdateImage(trackedImage);
        }
    }
    private void UpdateImage(ARTrackedImage trackedImage)
    {
        foreach (NetworkCube networkCube in _spawnedNetworkObjects)
        {
            if (networkCube.TrackerName == trackedImage.referenceImage.name)
            {
                networkCube.SetPositionAndRotation(trackedImage.transform.position, trackedImage.transform.rotation);
                networkCube.Show(true);
            }
            if(trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Limited)
            {
                networkCube.Show(false);
            }
        }
    }
}
