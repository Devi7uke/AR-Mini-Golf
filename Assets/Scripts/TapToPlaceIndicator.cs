using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

public class TapToPlaceIndicator : MonoBehaviour{
    [SerializeField]
    private List<GameObject> objectsToPlace = new List<GameObject>();
    public GameObject placementIndicator;
    private ARRaycastManager aRRaycastManager;
    private ARPlaneManager aRPlaneManager;
    private Pose placementPose;
    private bool placementPoseIsValid = false;
    private bool placementState = false;
    private GameObject placedObject;

    void Start(){
        aRRaycastManager = GetComponent<ARRaycastManager>();
        aRPlaneManager = GetComponent<ARPlaneManager>();
    }

    void Update(){
        if(!placementState){
            UpdatePlacementPose();
            UpdatePlacementIndicator();
        }
        if ( !placementState && placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began){
            PlaceObject();
            placementIndicator.SetActive(false);
            placementState = true;
        }
    }

    private void PlaceObject(){
        placedObject = Instantiate(objectsToPlace[0], placementPose.position, placementPose.rotation);
    }

    private void UpdatePlacementIndicator(){
        if (placementPoseIsValid){
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else{
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPose(){
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        aRRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid){
            placementPose = hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
        }
    }

    public void ChangePosition(){
        placementState = false;
        Destroy(placedObject);
        placementIndicator.SetActive(true);

    }
}