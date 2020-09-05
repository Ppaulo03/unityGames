using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : GroundEnemy
{

    [Header("Hurt Settings")]
    [SerializeField] private float dieTime = 0f;
    
    [Header("Attack Settings")]
    [SerializeField] private GameObject bomb = null;
    [SerializeField] private Transform bombPoint = null;
    [SerializeField] private Collider2D attackArea = null;

    [SerializeField] private float[] attackTimes = null;
    [SerializeField] private float bombTime = 0f;
    [SerializeField] private float forceModifier = 0f;
    [SerializeField] private float AttackCooldown = 0f; 
    [SerializeField] private float AttackDistance = 0f;
    [SerializeField] private float velocityAttack = 0f;


    [Header("Idle Settings")]
    [SerializeField] private float idleTime = 0f;


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
    }
    protected override void Die()
    {
        StartCoroutine(DieCo());
    }    
    private IEnumerator DieCo()
    {
        anim.SetTrigger("Die");
        yield return new WaitForSeconds(dieTime);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.gameObject.CompareTag("Player") && currentState != EnemyState.stagger){
            
            bool back = false;
            float time = 0f;
            float distance = Mathf.Abs(other.gameObject.transform.position.x - transform.position.x);
            
            attackArea.enabled = false;
            currentState = EnemyState.attacking;
            anim.SetBool("Walking", false); 
            
            if(other.gameObject.transform.position.x > gameObject.transform.position.x){
                transform.localScale = new Vector3(1,1,1);
                if(direction == -1) back = true;
                direction = 1;
            }else{
                transform.localScale = new Vector3(-1,1,1);;
                if(direction == 1) back = true;
                direction = -1;
            }

            if(distance <= AttackDistance){

                if(!back){
                    anim.SetTrigger("AttackA");
                    time = attackTimes[0];
                }
                else{
                    anim.SetTrigger("AttackB");
                    time = attackTimes[1];
                }
                myRigidbody2D.AddForce(new Vector3(direction*velocityAttack, 0, 0), ForceMode2D.Impulse);

            }
            else{
                anim.SetTrigger("AttackC");
                time = attackTimes[2];
                StartCoroutine(bombCo(distance*forceModifier));
            }
            StartCoroutine(attackCo(time));
        }

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

    private IEnumerator attackCo(float time)
    {
        yield return new WaitForSeconds(time);
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
    private IEnumerator bombCo(float force)
    {

        yield return new WaitForSeconds(bombTime);
        if(currentState == EnemyState.attacking){
            GameObject clone = Instantiate(bomb, bombPoint.position, Quaternion.Euler(Vector3.zero));
            clone.GetComponent<Rigidbody2D>().AddForce(new Vector3(direction,0.25f,0)*force, ForceMode2D.Impulse);
        }

    }
    private IEnumerator idleCo()
    {
        yield return new WaitForSeconds(idleTime);
        if(currentState == EnemyState.idle) {
            currentState = EnemyState.walking;
            anim.SetBool("Walking", true);    
        }
    }
}
