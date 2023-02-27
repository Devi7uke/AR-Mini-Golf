using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallController : MonoBehaviour{
    public static BallController instance;
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private GameObject ball;
    [SerializeField]
    private FixedJoystick fixedJoystick;
    [SerializeField]
    private float forceLimit, forceModifier = 1, horizontalInput = 0, verticalInput = 0;
    [SerializeField]
    private Vector3 startPosition;
    private Vector3 previousPosition;
    public float movementSpeed = 10.0f;
    private float force;
    private Rigidbody rb;
    private Vector3 shotDirection, predictLineDirection;
    public void Awake(){
        if (instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }
    }
    void Start(){
        rb = GetComponent<Rigidbody>();
    }
    void Update(){
        if(Input.touchCount > 0){
            lineRenderer.gameObject.SetActive(true);
            lineRenderer.SetPosition(1, predictLineDirection);
            if (Input.GetTouch(0).phase == TouchPhase.Ended){
                previousPosition = transform.position;
                Debug.Log("Force aplicated");
                rb.AddForce((transform.parent.TransformDirection(shotDirection)) * forceModifier * movementSpeed, ForceMode.Acceleration);
                lineRenderer.gameObject.SetActive(false);
            }
        }
    }
    void LateUpdate(){
        horizontalInput = -fixedJoystick.Horizontal;
        verticalInput = -fixedJoystick.Vertical;
        predictLineDirection = new Vector3(2 * horizontalInput, 0, 2 * verticalInput);
        shotDirection = new Vector3(horizontalInput * movementSpeed, 0, verticalInput * movementSpeed);
    }

    void OnTriggerEnter(Collider other){
        if(other.tag == "HoleOne"){
            Debug.Log("Won Hole One");
        }else if(other.tag == "HoleTwo"){
            Debug.Log("Won Hole Two");
        }else if(other.tag == "HoleThree"){
            Debug.Log("Won Hole Three");
        }else if(other.tag == "HoleOneArea"){
            gameObject.transform.position = startPosition;
        }
    }
}