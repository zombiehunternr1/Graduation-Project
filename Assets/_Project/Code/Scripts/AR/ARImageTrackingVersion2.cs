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
    [SerializeField] private List<GameObject> _spawnedObjects = new List<GameObject>();

    private void Start()
    {
        if (!isServer)
        {
            foreach (GameObject prefab in NetworkManager.singleton.spawnPrefabs)
            {
                GameObject instantiatedObject = (GameObject)Instantiate(prefab, Vector3.zero, Quaternion.identity);
                instantiatedObject.name = prefab.name;
                _spawnedObjects.Add(instantiatedObject);
            }
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
        foreach (ARTrackedImage trackedImage in args.removed)
        {
            foreach(GameObject spawnedObject in _spawnedObjects)
            {
                if(spawnedObject.name == trackedImage.name)
                {
                    spawnedObject.SetActive(false);
                }
            }
        }
    }
    private void UpdateImage(ARTrackedImage trackedImage)
    {
        foreach (GameObject spawnedObject in _spawnedObjects)
        {
            if (spawnedObject.name == trackedImage.referenceImage.name)
            {
                spawnedObject.SetActive(true);
                spawnedObject.transform.SetLocalPositionAndRotation(trackedImage.transform.position, transform.transform.rotation);
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
                    foreach(GameObject spawnedobject in _spawnedObjects)
                    {
                        if(hit.collider.name == spawnedobject.name)
                        {
                            //CmdChangeColor(hit.collider.gameObject);
                            MeshRenderer mesh = hit.collider.GetComponent<MeshRenderer>();
                            mesh.material.color = Color.black;
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
