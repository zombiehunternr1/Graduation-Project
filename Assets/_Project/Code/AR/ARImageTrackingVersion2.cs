using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.InputSystem;
using TMPro;

public class ARImageTrackingVersion2 : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textFieldBlue;
    [SerializeField] private TextMeshProUGUI _textFieldGreen;
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
                _spawnedPrefabs[gameObject.Key].SetActive(true);
                _spawnedPrefabs[gameObject.Key].transform.SetPositionAndRotation(trackedImage.transform.position, trackedImage.transform.rotation);
            }
        }
    }
    RaycastHit hit;
    public void OnScreenTapped(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 position = context.ReadValue<Vector2>();
            _textFieldBlue.text = position.ToString();
            Ray ray = Camera.main.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out hit, 200))
            {
                if (hit.collider != null)
                {
                    foreach (KeyValuePair<string, GameObject> gameObject in _spawnedPrefabs)
                    {
                        if (hit.collider.name == gameObject.Key)
                        {
                            MeshRenderer mesh = gameObject.Value.GetComponent<MeshRenderer>();
                            mesh.material.SetColor("_Color", Color.black);
                        }
                    }
                }
            }         
        }
    }
}
