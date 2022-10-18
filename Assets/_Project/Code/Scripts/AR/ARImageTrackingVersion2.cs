using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.InputSystem;
using Mirror;
using Unity.VisualScripting;
using UnityEngine.XR.ARSubsystems;

public class ARImageTrackingVersion2 : NetworkBehaviour
{
    [SerializeField] private ARTrackedImageManager _aRTrackedImageManager;
    [SerializeField] private List<GameObject> _placeblePrefabs;
    private Dictionary<string, GameObject> _spawnedPrefabs = new Dictionary<string, GameObject>();

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
        foreach (KeyValuePair<uint, NetworkIdentity> networkObject in NetworkServer.spawned)
        {
            if (networkObject.Value.name == trackedImage.referenceImage.name)
            {
                CmdShowObject(networkObject.Value.gameObject, trackedImage.transform);
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
    [ClientRpc]
    private void RpcShowObject(GameObject gameObject, Transform trackedImagePosition)
    {
        gameObject.SetActive(true);
        gameObject.transform.SetPositionAndRotation(trackedImagePosition.position, trackedImagePosition.rotation);
    }
    [Command(requiresAuthority = false)]
    private void CmdChangeColor(GameObject gameObject)
    {
        RpcChangeColor(gameObject);
    }
    [Command(requiresAuthority = false)]
    private void CmdShowObject(GameObject gameObject, Transform trackedImagePosition)
    {
        RpcShowObject(gameObject, trackedImagePosition);
    }
}
