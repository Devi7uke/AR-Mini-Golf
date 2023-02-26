using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeRotation : MonoBehaviour{
    public float speedRotation = 10;
    void Start(){
        
    }

    void Update(){
        transform.Rotate(Vector3.forward, speedRotation * Time.deltaTime);
    }
}
