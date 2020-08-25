using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : FlyingEnemy
{
    protected float currentSpeed;
    [Header("Components")]
    protected SpriteRenderer mySpriteRenderer;


    private void Start() {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        currentSpeed = Random.Range(speed*0.5f, speed*1.5f);
        currentState = EnemyState.walking;
    }

    private void FixedUpdate() {
        if(currentState == EnemyState.walking)
            Move();
            GetHeight();
    }

    private void Move(){
        if(OnGround()){
            if(PlataformEnd() != direction){
                myRigidbody2D.velocity = new Vector2(direction*currentSpeed, myRigidbody2D.velocity.y);
            }else{
                myRigidbody2D.velocity = new Vector2(0, myRigidbody2D.velocity.y);
                direction = -direction;
                mySpriteRenderer.flipX = !mySpriteRenderer.flipX;
            }
        }
    }

    protected void Turn(){
        myRigidbody2D.velocity = new Vector2(0, myRigidbody2D.velocity.y);
        direction = -direction;
        mySpriteRenderer.flipX = !mySpriteRenderer.flipX;
        currentSpeed = Random.Range(speed*0.5f, speed*1.5f);
    }

    protected virtual void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player")){
            Vector3 direction = (other.gameObject.transform.position - transform.position ).normalized;
            other.gameObject.GetComponent<PlayerController>().Hurt(direction* knockBackForce);
        }
        else if(!other.gameObject.CompareTag("Arrow")){
            Turn();
        }
    }

}
