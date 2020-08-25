using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeQueen : Bee
{
    [Header("Attack Settings")]
    [SerializeField] private float attackTime = 0f;
    [SerializeField] private GameObject bee = null;

    [Header("Components")]
    private Animator anim;

    private void Start() {
        anim = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        currentSpeed = Random.Range(speed*0.5f, speed*1.5f);
        currentState = EnemyState.walking;
    }

    private void SpawnBee(){
        int numBees = Random.Range(1,3);
        for(int i = 0; i < numBees; i++){
            Instantiate(bee, transform.position, Quaternion.Euler (Vector3.zero));
        }
    }

    protected override void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player")){
            Vector3 direction = (other.gameObject.transform.position - transform.position ).normalized;
            other.gameObject.GetComponent<PlayerController>().Hurt(direction* knockBackForce);
        }
        else if(other.gameObject.CompareTag("Arrow")){
            SpawnBee();
        }
        else{
            Turn();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){
            currentState = EnemyState.attacking;
            
            if(other.gameObject.transform.position.x > gameObject.transform.position.x){
                mySpriteRenderer.flipX = false;
                direction = 1;
            }else{
                mySpriteRenderer.flipX = true;
                direction = -1;
            }
            anim.SetTrigger("Attack");
            StartCoroutine(attackCo());
        }
    }

    private IEnumerator attackCo(){
        yield return new WaitForSeconds(attackTime);
        myRigidbody2D.velocity = Vector2.zero;
        currentState = EnemyState.walking;
    }

}
