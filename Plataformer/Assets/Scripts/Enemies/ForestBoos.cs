using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestBoos : FlyingEnemy
{
    [SerializeField] private GameObject teleportArea = null;
    [Header("Attack Settings")]
    [SerializeField] private Collider2D VisionCollider = null;
    [SerializeField] private Transform attackTransform = null;
    [SerializeField] private GameObject minion = null;


    [Header("AcidBall")]
    [SerializeField] private GameObject acidBall = null;
    [SerializeField] private float acidBallCooldown = 0f;
    [SerializeField] private float acidBallForce = 0f;


    [Header("AcidDrop")]
    [SerializeField] private GameObject dropAcid = null;
    [SerializeField] private FloatValue numPools = null;
    [SerializeField] private float maxPools = 0f;
    [SerializeField] private float dropCooldown = 0f;
    [SerializeField] private float attackTime = 0f;
    
    
    private float maxLife;
    private Vector3 InitialPos;
    
    private void Start()
    {
        currentSpeed = Random.Range(speed*0.5f, speed*1.5f);
        currentState = EnemyState.walking;
        InitialPos = transform.position;
        maxLife = lifePoints;
        StartCoroutine(dropCo());
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

    protected override void DieEffect()
    {
        if(teleportArea != null)
            teleportArea.SetActive(true);
    }

    protected override void HurtAction()
    {
        if(lifePoints + 2 <= maxLife)
        {
            
            maxLife = lifePoints;
            acidBallForce = acidBallForce + 0.005f;
            acidBallCooldown = acidBallCooldown - 0.05f;
            speed = speed + 0.2f;
           
            Instantiate(minion, transform.position, Quaternion.Euler (Vector3.zero));

        }
    }

    protected override void FrezzeBegin()
    {
        StopCoroutine(invunerableCo());
        VisionCollider.enabled = false;
    }

    protected override void FrezzeStop()
    {
        VisionCollider.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player")){
            Vector3 direction = (other.gameObject.transform.position - transform.position ).normalized;
            other.gameObject.GetComponent<PlayerController>().Hurt(direction* knockBackForce);
        }

        else if( !other.gameObject.CompareTag("Arrow") ){
            Turn();
            currentSpeed = Random.Range(speed*0.5f, speed*1.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player") && currentState != EnemyState.attacking){
            
            StopAllCoroutines();
            VisionCollider.enabled = false;

            if(other.gameObject.transform.position.x < transform.position.x){
                direction = -1;
                transform.localScale = new Vector3(-1,1,1);
            }
            else{
                direction = 1;
                transform.localScale = new Vector3(1,1,1);
            }

            Vector3 dir = other.gameObject.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;      
            float distance = Mathf.Abs(other.gameObject.transform.position.x - transform.position.x);
            if(currentState != EnemyState.attacking && currentState != EnemyState.freeze){
                GameObject clone = Instantiate(acidBall, attackTransform.position, Quaternion.Euler (new Vector3(0,0,angle)));
                clone.GetComponent<Rigidbody2D>().AddForce(dir*(distance*acidBallForce), ForceMode2D.Impulse);
            }

            anim.SetTrigger("Attack");
            StartCoroutine(cooldownCo(acidBallCooldown));
            StartCoroutine(dropCo());
        }
    }

    private void Drop()
    { 
        if(numPools.Value < maxPools && currentState != EnemyState.freeze){
            currentState = EnemyState.attacking;
            StartCoroutine(attackCo());
            Instantiate(dropAcid, attackTransform.position, Quaternion.Euler (Vector3.zero));
            anim.SetTrigger("Attack");
        }
        StartCoroutine(dropCo());
    }    

    private IEnumerator attackCo()
    {
        myRigidbody2D.velocity = Vector2.zero;
        yield return new WaitForSeconds(attackTime);
        if(currentState != EnemyState.freeze) currentState = EnemyState.walking;
    }

    private IEnumerator cooldownCo(float cooldown)
    {
        VisionCollider.enabled = false;
        yield return new WaitForSeconds(cooldown);
        if(currentState != EnemyState.freeze) VisionCollider.enabled = true;
        
    }

    private IEnumerator dropCo()
    {
        yield return new WaitForSeconds(dropCooldown);
        Drop();
    }

    
}
