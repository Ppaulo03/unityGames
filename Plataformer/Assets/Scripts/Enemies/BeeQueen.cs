using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeQueen : FlyingEnemy
{
    [SerializeField] private GameObject teleportArea = null;

    [Header("Attack Settings")]
    [SerializeField] private float attackTime = 0f;
    [SerializeField] private GameObject bee = null;

    private Vector3 InitialPos;
    private void Start()
    {
        currentSpeed = Random.Range(speed*0.5f, speed*1.5f);
        currentState = EnemyState.walking;
        InitialPos = transform.position;
    }
    private void FixedUpdate()
    {
        CheckDistance();
        if(currentState == EnemyState.walking){
            Move();
            GetHeight();
        }
    }

    private void CheckDistance()
    {
        if(Mathf.Abs(transform.position.y - InitialPos.y) > 5)
            transform.position =  new Vector3(transform.position.x,InitialPos.y,transform.position.z);
        if(Mathf.Abs(transform.position.x - InitialPos.x) > 22)
            transform.position =  new Vector3(InitialPos.x,transform.position.y,transform.position.z);
    }
    
    private void SpawnBee()
    {
        
        int numBees = Random.Range(1,5);
        for(int i = 0; i < numBees; i++){
            Instantiate(bee, transform.position, Quaternion.Euler (Vector3.zero));
        }

    }

    protected override void DieEffect()
    {
        if(teleportArea != null) teleportArea.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if( currentState == EnemyState.freeze ) UnFreeze();
        if( other.gameObject.CompareTag("Player")){
            Vector3 direction = (other.gameObject.transform.position - transform.position ).normalized;
            other.gameObject.GetComponent<PlayerController>().Hurt(direction* knockBackForce);
        }
        else if(other.gameObject.CompareTag("Arrow")){
            SpawnBee();
        }
        else if(turn && !other.gameObject.CompareTag("Arrow")){
            Turn();
            turn = false;
            currentSpeed = Random.Range(speed*0.5f, speed*1.5f);
            StartCoroutine(TurnCo());            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
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

    private IEnumerator attackCo()
    {
        yield return new WaitForSeconds(attackTime);
        myRigidbody2D.velocity = Vector2.zero;
        if(currentState != EnemyState.freeze) currentState = EnemyState.walking;
    }

}
