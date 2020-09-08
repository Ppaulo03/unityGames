using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionHead : FlyingEnemy
{
    private Transform playerPos;
    private void Start()
    {
        playerPos = GameObject.FindWithTag("Player").transform;
        currentSpeed = Random.Range( speed*0.5f, speed*1.5f );
        currentState = EnemyState.walking;
    
    }
    private void Update() {
        
        if(playerPos.position.x - 0.5f > transform.position.x){
            if(direction == -1){
                currentSpeed = Random.Range( speed*0.75f, speed*1.75f );
                Turn();
            }
        }
        else if(playerPos.position.x + 0.5f < transform.position.x)
            if (direction == 1){
                currentSpeed = Random.Range( speed*0.75f, speed*1.75f );
                Turn();
        }
   
    }
    
    private void FixedUpdate()
    {

        if( currentState == EnemyState.idle ) currentState = EnemyState.walking;
        if( currentState == EnemyState.walking ){
            Move();
            GetHeight();
        }

    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.CompareTag("Player")){
            if(other.transform.position.x > transform.position.x){
                if(direction == -1){
                    currentSpeed = Random.Range( speed*0.5f, speed*1.5f );
                    Turn();
                }
            }
            else if(direction == 1){
                currentSpeed = Random.Range( speed*0.5f, speed*1.5f );
                Turn();
            }
        }
    }



    private void OnCollisionEnter2D( Collision2D other )
    {
        if( other.gameObject.CompareTag("Player") ){

            Vector3 direction = (other.gameObject.transform.position - transform.position ).normalized;
            other.gameObject.GetComponent<PlayerController>().Hurt( direction * knockBackForce );
            
        }else if( !other.gameObject.CompareTag("Arrow") && turn ){
            Turn();
            turn = false;
            
            StartCoroutine(TurnCo());
        }
    }
}
