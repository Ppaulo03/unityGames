﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantArrow : Arrow
{
    [Header("Plant Stuff")]
    public GameObject plant;
    public Transform plantPosition;
    public float delayTime;

    [Header("Ground Identify")]
    public LayerMask groundLayer;
    public float groundDistance;

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Enemy")){
            Destroy(gameObject);
            other.gameObject.GetComponent<Enemy>().Hurt(myRigidbody.velocity.normalized * knockBackForce);
        }

        else{
            myRigidbody.velocity = Vector2.zero;
            StartCoroutine(plantCo());
           
        }

    }
    
    private bool plantDirection(Vector2 direction){
        Debug.DrawRay(plantPosition.position, direction, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(plantPosition.position, direction, groundDistance, groundLayer);
        if (hit.collider != null) return true;
        return false;
    }

    private IEnumerator plantCo(){
        yield return new WaitForSeconds(delayTime);
        if(plantDirection(Vector2.right)){
            GameObject clone = Instantiate(plant, plantPosition.position, Quaternion.Euler (new Vector3(0,0,90)));
            clone.GetComponent<PlantGrow>().isSideway = true;
            clone.GetComponent<PlantGrow>().sideCorrection = 1;
            Destroy(gameObject);
        }
        else if(plantDirection(Vector2.left)){
            GameObject clone = Instantiate(plant, plantPosition.position, Quaternion.Euler (new Vector3(0,0,-90)));
            clone.GetComponent<PlantGrow>().isSideway = true;
            clone.GetComponent<PlantGrow>().sideCorrection = -1;
            Destroy(gameObject);
        }
        else if(plantDirection(Vector2.down)){
            GameObject clone = Instantiate(plant, plantPosition.position, Quaternion.Euler (new Vector3(0,0,0)));
            clone.GetComponent<PlantGrow>().isSideway = false;
            clone.GetComponent<PlantGrow>().sideCorrection = 1;
            Destroy(gameObject);
        }
        else{
            myRigidbody.velocity = Vector2.zero;
        }
        
    }
}
