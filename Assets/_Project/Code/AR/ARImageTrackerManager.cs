using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;


public class ARImageTrackerManager : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager _arTrackedImageManager;
    [SerializeField] private List<GameObject> _cubesToSpawn;
    private List<GameObject> _cubesExist = new List<GameObject>();

    private void OnEnable()
    {
        _arTrackedImageManager.trackedImagesChanged += OnColorDetected;
    }
    private void OnDisable()
    {
        _arTrackedImageManager.trackedImagesChanged -= OnColorDetected;
    }
    private void OnColorDetected(ARTrackedImagesChangedEventArgs args)
    {
        foreach(ARTrackedImage trackedImage in args.added)
        {
            for(int i = 0; i < _cubesToSpawn.Count; i++)
            {
                if(trackedImage.name == _cubesToSpawn[i].GetComponent<Material>().name)
                {
                    if (!_cubesExist.Contains(_cubesToSpawn[i]))
                    {
                        _arTrackedImageManager.trackedImagePrefab = _cubesToSpawn[i];
                    }
                    _cubesExist.Add(_cubesToSpawn[i]);
                }
            }
        }
    }
}
