using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roomMove : MonoBehaviour
{
    public Vector2 cameraChangeMin;
    public Vector2 cameraChangeMax;
    public Vector3 playerChange;
    private CamaraController camara;

    private void Start() {
        camara = FindObjectOfType<CamaraController>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            camara.minPosition += cameraChangeMin;
            camara.maxPosition += cameraChangeMax;
            other.transform.position += playerChange;
        }    
    }

}
