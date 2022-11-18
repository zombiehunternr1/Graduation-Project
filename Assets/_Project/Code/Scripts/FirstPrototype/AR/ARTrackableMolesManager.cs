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
    [SerializeField] private NetworkMoleManager _networkMoleManager;
    [SerializeField] private List<GameObject> _molesToSpawn;
    private void Start()
    {
        if (isServer)
        {
            for(int i = 0; i < _molesToSpawn.Count; i++)
            {
                GameObject instantiatedMole = Instantiate(_molesToSpawn[i]);
                instantiatedMole.transform.SetParent(_networkMoleManager.spawnPositions[i]);
                instantiatedMole.name = instantiatedMole.name.Replace("(Clone)", "");
                NetworkServer.Spawn(instantiatedMole);
                NetworkMole networkMole = instantiatedMole.GetComponent<NetworkMole>();
                _networkMoleManager.networkMoles.Add(networkMole);
            }
            _networkMoleManager.StartGame();
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
