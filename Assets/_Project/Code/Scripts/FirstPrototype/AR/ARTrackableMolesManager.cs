using Mirror;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARTrackableMolesManager : NetworkBehaviour
{
    [SerializeField] private DebugEvent _debugEvent;
    [SerializeField] private Transform _parentTransform;
    [SerializeField] private ARTrackedImageManager _aRTrackedImageManager;
    [SerializeField] private GameObject _networkMoleManagerPrefab;
    private NetworkMoleManager _networkMoleManager;
    private void Start()
    {
        if (isServer)
        {
            GameObject networkMoleManager = Instantiate(_networkMoleManagerPrefab, Vector3.zero, Quaternion.identity);
            networkMoleManager.name = networkMoleManager.name.Replace("(Clone)", "");
            NetworkServer.Spawn(networkMoleManager);
        }
        _networkMoleManager = FindObjectOfType<NetworkMoleManager>();
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
        if(trackedImage.referenceImage.name == _networkMoleManager.name)
        {
            if(trackedImage.trackingState == TrackingState.Limited)
            {
                _networkMoleManager.Show(false);
            }
            else
            {
                _networkMoleManager.SetPositionAndRotation(trackedImage.transform.position, trackedImage.transform.rotation);
                _networkMoleManager.Show(true);
            }
        }
    }
}
