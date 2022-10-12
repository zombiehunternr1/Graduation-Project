using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARImageRecognition : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager _arTrackedImageManager;
    private void OnEnable()
    {
        _arTrackedImageManager.trackedImagesChanged += OnImageChanged;
    }
    private void OnDisable()
    {
        _arTrackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

    private void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach(ARTrackedImage trackedImage in args.added)
        {
            Debug.Log(trackedImage.name);
        }
    }
}
