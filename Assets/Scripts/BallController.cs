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
    [SerializeField]
    private ParticleSystem particles;
    private Vector3 previousPosition;
    public float movementSpeed = 10.0f;
    private float force;
    private Rigidbody rb;
    private Vector3 shotDirection, predictLineDirection;
    private GameManager gameManager;
    private int strokesCounter = 0;
    //private AudioSource audioSource;
    void Start(){
        rb = GetComponent<Rigidbody>();
        gameManager = GameObject.FindGameObjectWithTag("Interface").GetComponent<GameManager>();
        //audioSource = GetComponent<AudioSource>();
    }
    void Update(){
        if(Input.touchCount > 0){
            lineRenderer.gameObject.SetActive(true);
            lineRenderer.SetPosition(1, predictLineDirection);
            if (Input.GetTouch(0).phase == TouchPhase.Ended && horizontalInput != 0 && verticalInput != 0){
                previousPosition = transform.position;
                Debug.Log("Force aplicated");
                rb.AddForce((transform.parent.TransformDirection(shotDirection)) * forceModifier * movementSpeed, ForceMode.Acceleration);
                //audioSource.Play();
                lineRenderer.gameObject.SetActive(false);
                strokesCounter ++;
                gameManager.UpdateStrokeNumber(strokesCounter);
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
            StartCoroutine("NextHole", 0);
        }else if(other.tag == "HoleTwo"){
            Debug.Log("Won Hole Two");
            StartCoroutine("NextHole", 1);
        }else if(other.tag == "HoleThree"){
            Debug.Log("Won Hole Three");
            StartCoroutine("NextHole", 2);
        }else if(other.tag == "HoleOneArea" || other.tag == "HoleTwoArea" || other.tag == "HoleThreeArea"){
            ResetHole();
        }
    }
    public void ResetHole(){
        gameObject.transform.position = transform.parent.InverseTransformPoint(startPosition);
        strokesCounter = 0;
        gameManager.UpdateStrokeNumber(strokesCounter);
    }
    IEnumerator NextHole(int hole){
        yield return new WaitForSeconds(2.0f);
        gameManager.NextHole(hole);
    }
}