using FMOD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARCursor : MonoBehaviour
{
    [SerializeField] private Transform _cursorObject;
    [SerializeField] private Transform _targetObjectToPlace;
    [SerializeField] ARRaycastManager _arRaycastManager;
    [SerializeField] private bool _useCursor;

    void Start()
    {
        _cursorObject.gameObject.SetActive(_useCursor);
    }
    void Update()
    {
        if (_useCursor)
        {
            UpdateCursor();
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (_useCursor)
            {
                Instantiate(_targetObjectToPlace, transform.position, transform.rotation);
            }
            else
            {
                List<ARRaycastHit> hits = new List<ARRaycastHit>();
                _arRaycastManager.Raycast(Input.GetTouch(0).position, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);
                if(hits.Count > 0)
                {
                    Instantiate(_targetObjectToPlace, hits[0].pose.position, hits[0].pose.rotation);
                }
            }
        }
    }

    private void UpdateCursor()
    {
        Vector2 screenposition = Camera.main.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        _arRaycastManager.Raycast(screenposition, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);
        if(hits.Count > 0)
        {
            transform.position = hits[0].pose.position;
            transform.rotation = hits[0].pose.rotation;
        }
    }
}
