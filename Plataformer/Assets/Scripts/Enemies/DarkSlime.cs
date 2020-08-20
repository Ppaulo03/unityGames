using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkSlime : Enemy
{
    [Header("Components")]
    private Animator anim;
    private SpriteRenderer mySpriteRenderer;

    [Header("Attack Settings")]
    public float attackTime;
    public float velocityAttack;
    public float jumpAttack;

    [Header("Idle Settings")]
    public float idleTime;

    private void Start() {
        anim = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        currentState = EnemyState.walking;

    }

    private void FixedUpdate() {
        switch(currentState){
            case EnemyState.walking:
                Move();
                break;
            
        }
       
    }

    private void Move(){
        if(OnGround()){
            if(PlataformEnd() != direction){
                myRigidbody2D.velocity = new Vector2(direction*speed, myRigidbody2D.velocity.y);
            }else{
                myRigidbody2D.velocity = new Vector2(0, myRigidbody2D.velocity.y);
                direction = -direction;
                mySpriteRenderer.flipX = !mySpriteRenderer.flipX;
                currentState = EnemyState.idle;
                anim.SetBool("Walking", false);
                StartCoroutine(idleCo());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){
            currentState = EnemyState.attacking;
            if(other.gameObject.transform.position.x > gameObject.transform.position.x){
                mySpriteRenderer.flipX = true;
                direction = 1;
            }else{
                mySpriteRenderer.flipX = false;
                direction = -1;
            }

            switch(Random.Range(0, 3)){
                case 0:
                    anim.SetBool("AttackA",true);
                    break;
                case 1:
                    anim.SetBool("AttackB",true);
                    myRigidbody2D.AddForce(new Vector3(direction*velocityAttack, jumpAttack, 0), ForceMode2D.Impulse);
                    break;
                case 2:
                    anim.SetBool("AttackC",true);
                    break;
            }
            StartCoroutine(attackCo());

        }
    }

    private IEnumerator attackCo(){
        yield return new WaitForSeconds(attackTime);
        anim.SetBool("AttackA",false);
        anim.SetBool("AttackB",false);
        anim.SetBool("AttackC",false);
        anim.SetBool("Walking",false);
        myRigidbody2D.velocity = Vector2.zero;
        currentState = EnemyState.idle;
        StartCoroutine(idleCo());
    }

    private IEnumerator idleCo(){
        yield return new WaitForSeconds(idleTime);
        if(currentState == EnemyState.idle) {
            currentState = EnemyState.walking;
            anim.SetBool("Walking", true);    
        }
    }
}
