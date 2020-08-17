using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy 
{
    public GameObject damageNumber;
    public Transform target;

    private Animator anim;
    private Rigidbody2D myRigidBody;
    private Vector3 moveDirection;
    private EnemyAI aI;

    public float timeToMove;
    private float timeToMoveConter;
    public float timeBetweenMove;
    private float timeBetweenMoveConter;

    void Start(){

        aI = transform.GetComponent<EnemyAI>();
        target = GameObject.FindWithTag("Player").transform;
        anim = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();

        timeBetweenMoveConter = Random.Range(timeBetweenMove*0.75f, timeBetweenMove*1.25f);
        changeState(EnemyState.idle);

    }

    void FixedUpdate(){
        switch (state){
            case EnemyState.idle:
                wait();
                break;
            
            case EnemyState.walk:
                walk();
                break;

            case EnemyState.chase:
                Vector3 direction = target.position - transform.position;
                anim.SetFloat("MoveX",direction.x);
                anim.SetFloat("MoveY",direction.y);
                break;

            default:
                break;
        }
    }

    void walk(){
        timeToMoveConter -= Time.deltaTime;
        myRigidBody.velocity = moveDirection*status.RuntimeMoveSpeed;

        if(timeToMoveConter<0f){ 
            timeBetweenMoveConter = Random.Range(timeBetweenMove*0.75f, timeBetweenMove*1.25f);
            changeState(EnemyState.idle);
        }
    }

    private void wait(){
        timeBetweenMoveConter -= Time.deltaTime;
        myRigidBody.velocity = Vector2.zero;

        if(timeBetweenMoveConter < 0f){
            timeToMoveConter = Random.Range(timeToMove*0.75f, timeToMove*1.25f);

            do{
                moveDirection = new Vector3(Random.Range(-1,1),Random.Range(-1,1), 0);
            }while(moveDirection == Vector3.zero);
            moveDirection.Normalize();
                
            anim.SetFloat("MoveX",moveDirection.x);
            anim.SetFloat("MoveY",moveDirection.y);
            changeState(EnemyState.walk);

        }

    }
 
    private void OnCollisionEnter2D(Collision2D other) {
        
        if(!other.gameObject.CompareTag("Player")){  
            if(state == EnemyState.walk){
                moveDirection = -moveDirection;
                myRigidBody.velocity = moveDirection*status.RuntimeMoveSpeed;
                anim.SetFloat("MoveX",moveDirection.x);
                anim.SetFloat("MoveY",moveDirection.y);
            }
        }
    
        else{
            int damageGiven = other.gameObject.GetComponent<PlayerController>().TakeDamage((int) status.RuntimeAttack);
            if(damageGiven > 0){
                GameObject clone = Instantiate(damageNumber, other.transform.position, Quaternion.Euler ( Vector3.zero ));
                clone.GetComponent<FloatingNumbers>().damageNumber = damageGiven;
            }
                
            if(other.enabled){
                
                PlayerController player = other.gameObject.GetComponent<PlayerController>();
                player.Knock(transform, status.RuntimeThrust);
                myRigidBody.velocity = Vector2.zero;
                changeState(EnemyState.sttager);
                anim.SetBool("Moving", false);
                StartCoroutine(waitCo());

            }else{
                changeState(EnemyState.idle);
            }
        }
    }
    
    public override void die(){
        gameObject.SetActive(false);
        GameObject.FindObjectOfType<PlayerController>().addExp(status.RuntimeExperience);
        //GameObject.FindObjectOfType<QuestManager>().kill(enemyName);
    }
    
    public override void Knock(Transform player, float thrust, float knockTime){
        
        changeState(EnemyState.sttager);
        myRigidBody.velocity = Vector2.zero;
        
        Vector2 difference = transform.position -  player.position;
        difference = difference.normalized * thrust;
        myRigidBody.AddForce(difference, ForceMode2D.Impulse);
        if(gameObject.active) StartCoroutine(knockCo(knockTime));
    }

    private IEnumerator knockCo(float knockTime){
        yield return new WaitForSeconds(knockTime);
        myRigidBody.velocity = Vector2.zero;
        StartCoroutine(waitCo());
    }
    
    private IEnumerator waitCo(){
        yield return new WaitForSeconds(waitTime);
        if(state != EnemyState.interact){
            if(haveVision)  changeState(EnemyState.chase);
            else changeState(EnemyState.idle);
        }
        
    }
    public void Interacting(){
        if(state == EnemyState.interact){
            if(haveVision) changeState(EnemyState.chase);
            else changeState(EnemyState.idle);
        }
        else changeState(EnemyState.interact);
    }

    public override void changeState(EnemyState newState){
        state = newState;
        switch(state){
             case EnemyState.idle:
                aI.enabled = false;
                anim.SetBool("Moving",false);
                break;
            
            case EnemyState.walk:
                aI.enabled = false;
                anim.SetBool("Moving",true);
                break;
            
            case EnemyState.sttager:
                aI.enabled = false;
                anim.SetBool("Moving",false);
                break;

            case EnemyState.interact:
                aI.enabled = false;
                myRigidBody.velocity = Vector2.zero;
                anim.SetBool("Moving",false);
                break;

            case EnemyState.chase:
                aI.enabled = true;
                anim.SetBool("Moving",true);
                break;

            default:
                state = EnemyState.idle;
                aI.enabled = false;
                anim.SetBool("Moving",false);
                break;
        }
    }
    
}
