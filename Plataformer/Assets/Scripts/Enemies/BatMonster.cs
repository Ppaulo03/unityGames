using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatMonster : FlyingEnemy
{

    [Header("Hurt Settings")]
    [SerializeField] private float dieTime = 0f;

    [Header("Attack Settings")]
    [SerializeField] private GameObject projectile = null;
    [SerializeField] private Transform projectilePoint = null;
    [SerializeField] private float projectileDelay = 0f;
    [SerializeField] private Collider2D attackArea = null;

    [SerializeField] private float[] attackForce = null;
    [SerializeField] private float[] attackDistances = null;
    [SerializeField] private float attackTime = 0f;
    [SerializeField] private float attackCooldown = 0f;

    private void Start()
    {
        currentSpeed = Random.Range( speed*0.5f, speed*1.5f );
        currentState = EnemyState.walking;
    }

    private void FixedUpdate()
    {

        if( currentState == EnemyState.idle ) currentState = EnemyState.walking;
        if( currentState == EnemyState.walking ){
            Move();
            GetHeight();
        }

    }

    protected override void HurtAction()
    {
        anim.SetTrigger("Hit");
    }
    protected override void Die()
    {
        myRigidbody2D.velocity = Vector2.zero;
        StartCoroutine(DieCo());
    }    
    private IEnumerator DieCo()
    {
        anim.SetTrigger("Die");
        yield return new WaitForSeconds(dieTime);
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
    }


    private void OnCollisionEnter2D( Collision2D other )
    {
        if( other.gameObject.CompareTag("Player") ){
            Vector3 direction = (other.gameObject.transform.position - transform.position ).normalized;
            other.gameObject.GetComponent<PlayerController>().Hurt( direction* knockBackForce );
        }
        else if( !other.gameObject.CompareTag("Arrow")){
            if(turn){
                Turn();
                turn = false;
                currentSpeed = Random.Range( speed*0.5f, speed*1.5f );
                StartCoroutine(TurnCo());
            }
        }
        else{
            if(other.transform.position.x < transform.position.x)
                if(direction == 1) Turn();
            else if(direction == -1) Turn();
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.gameObject.CompareTag("Player") && currentState != EnemyState.stagger){
            
            float distance = Mathf.Abs(other.gameObject.transform.position.x - transform.position.x);
            Vector3 direct = (other.gameObject.transform.position - transform.position).normalized;

            attackArea.enabled = false;
            currentState = EnemyState.attacking;
            
            if(other.gameObject.transform.position.x > gameObject.transform.position.x){
                if(direction == -1) Turn();
            }else if(direction == 1) Turn();
            
            if(distance <= attackDistances[0]){

                myRigidbody2D.AddForce(direct * attackForce[0], ForceMode2D.Impulse);
                anim.SetTrigger("AttackA");

            }else if(distance <= attackDistances[1]){
                
                myRigidbody2D.AddForce(direct * attackForce[1], ForceMode2D.Impulse);
                anim.SetTrigger("AttackB");
            }else{
                anim.SetTrigger("AttackC");
                StartCoroutine(projectileCo(direct));
            }

            StartCoroutine(attackCo());
        }

    }

    private IEnumerator attackCo()
    {
        yield return new WaitForSeconds(attackTime);
        myRigidbody2D.velocity = Vector2.zero;
        if(currentState != EnemyState.freeze){
            currentState = EnemyState.idle;
            StartCoroutine(attackCooldownCo());
        }
    }
    private IEnumerator attackCooldownCo()
    {
        yield return new WaitForSeconds(attackCooldown);
        attackArea.enabled = true;
    }

    private IEnumerator projectileCo(Vector3 direct)
    {

        yield return new WaitForSeconds(projectileDelay);
        if(currentState == EnemyState.attacking){
            GameObject clone = Instantiate(projectile, projectilePoint.position, Quaternion.Euler(Vector3.zero));
            clone.GetComponent<Rigidbody2D>().AddForce(direct*attackForce[2], ForceMode2D.Impulse);
        }

    }

}
