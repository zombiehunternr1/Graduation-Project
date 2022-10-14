using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARImageTrackingVersion2 : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager _aRTrackedImageManager;
    [SerializeField] private List<GameObject> _placeblePrefabs;
    private Dictionary<string, GameObject> _spawnedPrefabs = new Dictionary<string, GameObject>();

    private void Awake()
    {
        foreach (GameObject prefab in _placeblePrefabs)
        {
            GameObject newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newPrefab.name = prefab.name;
            _spawnedPrefabs.Add(prefab.name, newPrefab);
            newPrefab.SetActive(false);
        }
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
        foreach(ARTrackedImage trackedImage in args.removed)
        {
            _spawnedPrefabs[trackedImage.name].SetActive(false);
        }
    }
    private void UpdateImage(ARTrackedImage trackedImage)
    {
        foreach (KeyValuePair<string, GameObject> gameObject in _spawnedPrefabs)
        {
            if (gameObject.Key == trackedImage.referenceImage.name)
            {
                if (trackedImage.referenceImage.name == "Blue")
                {
                    GameObject cube1 = _spawnedPrefabs[gameObject.Key];
                    cube1.SetActive(true);
                    cube1.transform.SetPositionAndRotation(trackedImage.transform.position, trackedImage.transform.rotation);
                    return;
                }
                if (trackedImage.referenceImage.name == "Green")
                {
                    GameObject cube2 = _spawnedPrefabs[gameObject.Key];
                    cube2.SetActive(true);
                    cube2.transform.SetPositionAndRotation(trackedImage.transform.position, trackedImage.transform.rotation);
                    return;
                }
            }
        }
    }
}
