using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.InputSystem;
using Mirror;
using TMPro;

public class ARImageTrackingVersion2 : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI _greenText;
    [SerializeField] private TextMeshProUGUI _blueText;
    [SerializeField] private ARTrackedImageManager _aRTrackedImageManager;
    [SerializeField] private List<GameObject> _placeblePrefabs;
    private SyncDictionary<string, GameObject> _spawnedPrefabs = new SyncDictionary<string, GameObject>();

    private void Start()
    {
        if (!isServer)
        {
            return;
        }
        foreach (GameObject prefab in _placeblePrefabs)
        {
            GameObject newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newPrefab.name = prefab.name;
            _spawnedPrefabs.Add(prefab.name, newPrefab);
            newPrefab.SetActive(false);
            NetworkServer.Spawn(newPrefab);
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
        _blueText.text = "Image detected!";
        foreach (KeyValuePair<string, GameObject> spawnedObject in _spawnedPrefabs)
        {
            if (spawnedObject.Value.name == trackedImage.referenceImage.name)
            {
                _blueText.text = "Show object!";
                gameObject.SetActive(true);
                gameObject.transform.SetPositionAndRotation(trackedImage.transform.position, trackedImage.transform.rotation);
            }
        }
    }
    RaycastHit hit;
    public void OnScreenTapped(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 position = context.ReadValue<Vector2>();
            Ray ray = Camera.main.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out hit, 200))
            {
                if (hit.collider != null)
                {
                    foreach (KeyValuePair<string, GameObject> gameObject in _spawnedPrefabs)
                    {
                        if (hit.collider.name == gameObject.Key)
                        {
                            CmdChangeColor(gameObject.Value);
                        }
                    }
                }
            }         
        }
    }
    [ClientRpc]
    private void RpcChangeColor(GameObject gameObject)
    {
        MeshRenderer mesh = gameObject.GetComponent<MeshRenderer>();
        mesh.material.color = Color.black;
    }
    [Command(requiresAuthority = false)]
    private void CmdChangeColor(GameObject gameObject)
    {
        RpcChangeColor(gameObject);
    }
}
