using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : GroundEnemy
{
    [Header("Hurt Settings")]
    [SerializeField] private float dieTime = 0f;
    
    [SerializeField] private Collider2D attackArea = null;

    [Header("Idle Settings")]
    [SerializeField] private float idleTime = 0f;

    [Header("Attack Settings")]
    [SerializeField] private float velocityAttack = 0f;
    [SerializeField] private float attackTime = 0f;
    [SerializeField] private float AttackCooldown = 0f;
    [SerializeField] private GameObject poison = null;
    [SerializeField] private float poisonTime = 0f;
    [SerializeField] private float posisonDelay = 0f;
    [SerializeField] private float hitDelay = 0f;
    [SerializeField] private float dieDelayTime = 0f;


    private void Start()
    {
        currentState = EnemyState.walking;
        attackArea.enabled = true;
        anim.SetBool("Walking", true);
    }

    private void FixedUpdate()
    {
        if(currentState == EnemyState.walking) Move();    
    }

    private void Move()
    {
        if(OnGround()){
            if(PlataformEnd() != direction){
                myRigidbody2D.velocity = new Vector2(direction*speed, myRigidbody2D.velocity.y);
            }else{
                Turn();
                currentState = EnemyState.idle;
                anim.SetBool("Walking", false);
                StartCoroutine(idleCo());
            }
        }
    }

    protected override void HurtAction()
    {
        anim.SetTrigger("Hit");
        StartCoroutine(hitDelayCo());
    }
    protected override void Die()
    {
        StartCoroutine(DieCo());
    }    
    private IEnumerator DieCo()
    {
        anim.SetTrigger("Die");
        yield return new WaitForSeconds(dieTime);
        poison.SetActive(true);
        myRigidbody2D.velocity = Vector2.zero;
        yield return new WaitForSeconds(dieDelayTime);
        poison.SetActive(false);
        gameObject.SetActive(false);
    }

    protected override void FrezzeBegin()
    {
        StopCoroutine(invunerableCo());
        attackArea.enabled = false;
    }
    protected override void FrezzeStop()
    {
        attackArea.enabled = true;
        StartCoroutine(idleCo());
    }
    private IEnumerator idleCo()
    {
        yield return new WaitForSeconds(idleTime);
        if(currentState == EnemyState.idle){
            if(Random.Range(0,10) == 1)
                StartCoroutine(poisonCo());
            else{
                currentState = EnemyState.walking;
                anim.SetBool("Walking", true);   
            }
             
        }
    }
    protected override void ColisionAction(Collision2D other){
        if(other.gameObject.transform.position.x > gameObject.transform.position.x){
            if(direction == -1) Turn();
        }else if(direction == 1) Turn();    
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player") && currentState != EnemyState.stagger){
            if(other.gameObject.transform.position.y - 1 <= gameObject.transform.position.y){
                attackArea.enabled = false;
                currentState = EnemyState.attacking;
                anim.SetBool("Walking", false); 
                
                
                if(other.gameObject.transform.position.x > gameObject.transform.position.x){
                    if(direction == -1) Turn();
                }else if(direction == 1) Turn();            

                switch(Random.Range(0,2)){
                    case 0:
                        anim.SetTrigger("AttackA");
                        break;
                    case 1:
                        anim.SetTrigger("AttackB");
                        break;
                }
                myRigidbody2D.velocity = Vector2.zero;
                myRigidbody2D.AddForce(new Vector3(direction*velocityAttack, 0, 0), ForceMode2D.Impulse);
                StartCoroutine(attackCo());
            }
        }
    }

    private IEnumerator attackCo()
    {
       yield return new WaitForSeconds(attackTime);
        myRigidbody2D.velocity = Vector2.zero;
        if(currentState != EnemyState.freeze){
            currentState = EnemyState.idle;
            StartCoroutine(idleCo());
            StartCoroutine(attackCooldownCo());
        }
    }
    private IEnumerator attackCooldownCo()
    {
        yield return new WaitForSeconds(AttackCooldown);
        attackArea.enabled = true;
    }

    private IEnumerator hitDelayCo(){
        StopCoroutine(poisonTimeCo());
        yield return new WaitForSeconds(hitDelay);
        StartCoroutine(poisonCo());
    }
    private IEnumerator poisonCo()
    {   
        anim.SetTrigger("AttackC");
        yield return new WaitForSeconds(posisonDelay);
        poison.SetActive(true);
        StartCoroutine(poisonTimeCo());
        StartCoroutine(idleCo());
    }

    private IEnumerator poisonTimeCo()
    {
        yield return new WaitForSeconds(poisonTime);
        poison.SetActive(false);
    }

}
