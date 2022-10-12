using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;


public class ARImageTrackerManager : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager _arTrackedImageManager;
    [SerializeField] private TextMeshProUGUI _greenText;
    [SerializeField] private TextMeshProUGUI _blueText;
    private void OnEnable()
    {
        _arTrackedImageManager.trackedImagesChanged += OnImageTrackedChanged;
    }
    private void OnDisable()
    {
        _arTrackedImageManager.trackedImagesChanged -= OnImageTrackedChanged;
    }

    GameObject _spawnBlue;
    GameObject _spawnGreen;
    private void OnImageTrackedChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (ARTrackedImage trackedImage in args.added)
        {
            if(_spawnGreen == null)
            {
                _spawnGreen = Instantiate(trackedImage.gameObject);
            }
            if(_spawnBlue == null)
            {
                _spawnBlue = Instantiate(trackedImage.gameObject);
            }
            if(trackedImage.trackingState != UnityEngine.XR.ARSubsystems.TrackingState.None)
            {
                if(trackedImage.referenceImage.name == "Blue")
                {
                    _spawnBlue.transform.GetChild(0).gameObject.SetActive(true);
                }
                if(trackedImage.referenceImage.name == "Green")
                {
                    _spawnGreen.transform.GetChild(1).gameObject.SetActive(true);
                }
            }
        }
        foreach(ARTrackedImage trackedImage in args.updated)
        {
            if(trackedImage.referenceImage.name == "Blue")
            {
                _spawnBlue.transform.SetPositionAndRotation(trackedImage.transform.position, trackedImage.transform.rotation);
            }
            if(trackedImage.referenceImage.name == "Green")
            {
                _spawnGreen.transform.SetPositionAndRotation(trackedImage.transform.position, trackedImage.transform.rotation);
            }
        }
    }
}
