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

    static readonly List<ARRaycastHit> hits =
        new List<ARRaycastHit>();

    bool placed;
    bool canPlace = true;


    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }


    public void SetCanPlace(bool value)
    {
        canPlace = value;
    }


    void Update()
    {
        // Don't allow placement while paused
        if (!canPlace || placed || Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase != TouchPhase.Began)
            return;


        // Ignore UI touches
        if (EventSystem.current != null &&
            EventSystem.current.IsPointerOverGameObject(touch.fingerId))
        {
            return;
        }


        if (raycastManager.Raycast(
            touch.position,
            hits,
            TrackableType.PlaneWithinPolygon))
        {
            Pose pose = hits[0].pose;

            GameObject baseObj =
                Instantiate(
                    gameBasePrefab,
                    pose.position,
                    pose.rotation
                );


            // Anchor
            baseObj.AddComponent<ARAnchor>();

            placed = true;
            canPlace = false;

            LockPlanes();

            OnBasePlaced?.Invoke(baseObj);
        }
    }
public bool IsPlaced()
{
    return placed;
}

    void LockPlanes()
    {
        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }

        planeManager.enabled = false;
    }
}