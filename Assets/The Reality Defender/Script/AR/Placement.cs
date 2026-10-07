using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class Placement : MonoBehaviour
{
    [SerializeField] GameObject gameBasePrefab;
    [SerializeField] ARPlaneManager planeManager;

    public static event Action<GameObject> OnBasePlaced;

    ARRaycastManager raycastManager;
    static readonly List<ARRaycastHit> hits = new List<ARRaycastHit>();
    bool placed;

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (placed || Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);
        if (touch.phase != TouchPhase.Began) return;

        // Ignore taps that land on UI (e.g. the FIRE button)
        if (EventSystem.current != null &&
            EventSystem.current.IsPointerOverGameObject(touch.fingerId)) return;

        if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose pose = hits[0].pose;   // position AND rotation, so it sits flat on the surface

            GameObject baseObj = Instantiate(gameBasePrefab, pose.position, pose.rotation);

            // Anchor it so tracking drift doesn't move it
            baseObj.AddComponent<ARAnchor>();

            placed = true;
            LockPlanes();
            OnBasePlaced?.Invoke(baseObj);
        }
    }

    void LockPlanes()
    {
        // Hide the grid and stop detecting new planes
        foreach (var plane in planeManager.trackables)
            plane.gameObject.SetActive(false);
        planeManager.enabled = false;
    }
}