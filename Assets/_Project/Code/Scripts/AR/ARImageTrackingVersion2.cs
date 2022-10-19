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
    private List<GameObject> _spawnedObjects;
    //[SerializeField][HideInInspector] private List<GameObject> _spawnedPrefabs;

    private void Start()
    {
        if (!isServer)
        {
            return;
        }
        /*
        foreach (GameObject prefab in _placeblePrefabs)
        {
            GameObject newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            _spawnedPrefabs.Add(newPrefab);
            newPrefab.SetActive(false);
        }
        */
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
        foreach (GameObject spawnedObject in NetworkManager.singleton.spawnPrefabs)
        {
            if (spawnedObject.name == trackedImage.referenceImage.name)
            {
                if (spawnedObject.activeInHierarchy)
                {
                    return;
                }
                else
                {
                    _blueText.text = "Show object!";
                    GameObject test = Instantiate(spawnedObject, Vector3.zero, Quaternion.identity);
                    test.SetActive(true);
                    test.transform.SetPositionAndRotation(trackedImage.transform.position, trackedImage.transform.rotation);
                }
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
                    foreach(GameObject spawnedobject in NetworkManager.singleton.spawnPrefabs)
                    {
                        if(hit.collider.name == spawnedobject.name)
                        {
                            CmdChangeColor(hit.collider.gameObject);
                        }
                    }
                    /*
                    foreach (KeyValuePair<string, GameObject> gameObject in _spawnedPrefabs)
                    {
                        if (hit.collider.name == gameObject.Key)
                        {
                            
                        }
                    }
                    */
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
