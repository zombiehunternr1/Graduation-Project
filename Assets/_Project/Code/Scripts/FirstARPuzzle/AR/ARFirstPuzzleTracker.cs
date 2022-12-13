using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARFirstPuzzleTracker : NetworkBehaviour
{
    [SerializeField] private DebugEvent _debugEvent;
    [SerializeField] private ARTrackedImageManager _aRTrackedImageManager;
    [SerializeField] private Transform _starShapeParentTransform;
    [SerializeField] private List<GameObject> _starShapesToSpawn;
    [SerializeField] private List<NetworkStarShape> _spawnedStarShapes;
    private void OnEnable()
    {
        _aRTrackedImageManager.trackedImagesChanged += OnImageChanged;
    }
    private void OnDisable()
    {
        _aRTrackedImageManager.trackedImagesChanged -= OnImageChanged;
    }
    private void Start()
    {
        if (isServer)
        {
            foreach (GameObject starShape in _starShapesToSpawn)
            {
                GameObject instantiatedStar = Instantiate(starShape, Vector3.zero, Quaternion.identity);
                instantiatedStar.name = instantiatedStar.name.Replace("(Clone)", "");
                instantiatedStar.transform.parent = _starShapeParentTransform;
                _spawnedStarShapes.Add(instantiatedStar.GetComponent<NetworkStarShape>());
            }
        }
        else
        {
            NetworkStarShape[] networkStarShapes = FindObjectsOfType<NetworkStarShape>();
            foreach(NetworkStarShape networkStarShape in networkStarShapes)
            {
                _spawnedStarShapes.Add(networkStarShape);
            }
        }
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
        foreach (NetworkStarShape starShape in _spawnedStarShapes)
        {
            if (trackedImage.referenceImage.name == starShape.starInfo.name)
            {
                if (trackedImage.trackingState == TrackingState.Limited)
                {
                    if (!starShape.starInfo.isSelected)
                    {
                        starShape.Show(false);
                    }
                    return;
                }
                else
                {
                    if (!starShape.starInfo.isSelected)
                    {
                        starShape.SetPositionAndRotation(trackedImage.transform.position, trackedImage.transform.rotation);
                        starShape.Show(true);
                    }
                    return;
                }
            }
        }
    }
}
