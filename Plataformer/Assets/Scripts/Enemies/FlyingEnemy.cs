using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy
{
    [Header("Flight Settings")]
    [SerializeField] private float desiredHeight = 0f;
    [SerializeField] private float hoverForce = 0f;
    [SerializeField] private float gravityFall = 0f;
    [SerializeField] private float turnTime = 0f;
    protected bool turn = true;
    
    
    public void GetHeight(){
        Vector2 position = transform.position + SizeCorrection;
        Vector2 direction = Vector2.down;

        for(float i = -raycastCorrection; i <= raycastCorrection; i += raycastCorrection){
            Debug.DrawRay(position + Vector2.right*i, direction, Color.green);
            RaycastHit2D hit = Physics2D.Raycast(position + Vector2.right*i, direction, groundDistance, groundLayer);
        
            if (hit.collider != null){
                if(hit.distance > desiredHeight)
                    myRigidbody2D.gravityScale = gravityFall;
                
                else{
                    myRigidbody2D.gravityScale = 0; 
                    if(hit.distance < desiredHeight)
                        myRigidbody2D.AddForce(Vector3.up*hoverForce);
                }
                return;
            } 

        }
        myRigidbody2D.gravityScale = gravityFall;
        return;
    }

    protected IEnumerator TurnCo(){
        yield return new WaitForSeconds(turnTime);
        turn = true;
    }

}
