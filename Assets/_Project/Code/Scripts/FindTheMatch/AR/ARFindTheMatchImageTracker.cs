using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARFindTheMatchImageTracker : NetworkBehaviour
{
    [SerializeField] private DebugEvent _debugEvent;
    [SerializeField] private Transform _TrackableObjectReference;
    [SerializeField] private ARTrackedImageManager _aRTrackedImageManager;
    [SerializeField] private List<GameObject> _objectsToSpawn = new List<GameObject>();
    private FindTheMatchNetworkManager _spawnedNetworkManager;
    private void Start()
    {
        if (isServer)
        {
            foreach (GameObject spawnableObject in _objectsToSpawn)
            {
                GameObject instantiatedObject = Instantiate(spawnableObject, Vector3.zero, Quaternion.identity);
                instantiatedObject.name = instantiatedObject.name.Replace("(Clone)", "");
                instantiatedObject.transform.SetParent(_TrackableObjectReference);
                NetworkServer.Spawn(instantiatedObject);
            }
        }
        _spawnedNetworkManager = FindObjectOfType<FindTheMatchNetworkManager>();
        _spawnedNetworkManager.name = _spawnedNetworkManager.name.Replace("(Clone)", "");
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
        if(_spawnedNetworkManager.name == trackedImage.referenceImage.name)
        {
            if(trackedImage.trackingState == TrackingState.Limited)
            {
                _spawnedNetworkManager.Show(false);
                return;
            }
            _spawnedNetworkManager.SetPositionAndRotation(trackedImage.transform.position, trackedImage.transform.rotation);
            _spawnedNetworkManager.Show(true);
        }
    }
}
