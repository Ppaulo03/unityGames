using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState{
    idle,
    walk,
    sttager,
    attack,
    interact
}

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D myRigidBody;
    private Animator anim;
    private DialogManager dialogManager;

    public PlayerState state;
    public StatusValues status;
    public FloatValue currentHealth;
    public Signal UISignal, screenKick;
    public Leveling levelingUp;
    private static bool playerExists;
    private bool canBeHurt;
    private float attackTimeCounter;
    public float attackTime, knockTime, flashLength;
    public int flashBlinks;
    public string startPoint;


    void Start(){
        dialogManager = FindObjectOfType<DialogManager>();
        anim = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
        canBeHurt = true;

        if(!playerExists){
            playerExists = true;
            DontDestroyOnLoad(transform.gameObject);
        }else{
            Destroy(gameObject);
        }
        levelUp();

    }

    private void Update() {

        if(Input.GetButtonUp("Attack")){
            if((state == PlayerState.idle || state == PlayerState.walk)){
                changeState(PlayerState.attack);
            }
        }

    }

    void FixedUpdate(){

        if(state == PlayerState.idle || state == PlayerState.walk){     
            movement(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));           
        }

    }


    private void movement(float movementX, float movementY){
        
        Vector2 move = new Vector2(movementX, movementY);
        move.Normalize();
        move *= status.RuntimeMoveSpeed;
        
        myRigidBody.velocity = move;
        if(myRigidBody.velocity != Vector2.zero){
            anim.SetFloat( "LastMoveX", movementX);
            anim.SetFloat( "LastMoveY", movementY);
            anim.SetBool("PlayerMoving", true);
            if(state != PlayerState.walk) changeState(PlayerState.walk);
        }
        else{
            anim.SetBool("PlayerMoving", false);
            if(state != PlayerState.idle) changeState(PlayerState.idle);
        }
        anim.SetFloat( "MoveX", movementX);
        anim.SetFloat( "MoveY", movementY);
    }

    public int TakeDamage(int damageToGive){
        if(canBeHurt){
            screenKick.Raise();
            canBeHurt = false;
            float damageReceived = Mathf.Ceil(damageToGive*(100f/ (100f + status.RuntimeDefense)));
            currentHealth.runtimeValue -= damageReceived;
            UISignal.Raise();
            if(currentHealth.runtimeValue <= 0){
                currentHealth.runtimeValue = 0;
                gameObject.SetActive(false);
            }
            else StartCoroutine(flashingCo());
            return (int) damageReceived;
        }else return -1;
    }

    public void Knock(Transform enemy, float thrust){
        changeState(PlayerState.sttager);

        Vector2 difference = transform.position -  enemy.position;
        difference = difference.normalized * thrust;
        myRigidBody.AddForce(difference, ForceMode2D.Impulse);
        
        if(currentHealth.runtimeValue > 0) StartCoroutine(knockCo(knockTime));
    }
    public void addExp(int expToAdd){
        status.RuntimeExperience += expToAdd;
        if(status.RuntimeExperience >= levelingUp.toLevel[status.RuntimeLevel]) levelUp();
    }
    public void levelUp(){
        while(status.RuntimeExperience >= levelingUp.toLevel[status.RuntimeLevel]){
            status.RuntimeExperience -= levelingUp.toLevel[status.RuntimeLevel];
            status.RuntimeLevel++;
        }

        int currentHP = levelingUp.maxHealth[status.RuntimeLevel];
        status.RuntimeMaxHealth = currentHP;
        currentHealth.runtimeValue += currentHP - levelingUp.maxHealth[status.RuntimeLevel-1];

        status.RuntimeAttack = levelingUp.attack[status.RuntimeLevel];
        status.RuntimeDefense = levelingUp.defense[status.RuntimeLevel];
        UISignal.Raise();
    }
    
    public void InteractionManager(){
        if(state == PlayerState.interact){
            changeState(PlayerState.idle);
        }
        else changeState(PlayerState.interact);
    }

    public void changeState(PlayerState newState){
        state = newState;
        switch(state){

            case PlayerState.attack:
                myRigidBody.velocity = Vector2.zero;
                anim.SetBool("Attacking", true);
                StartCoroutine(attackCo());
                break;

            case PlayerState.sttager:
                myRigidBody.velocity = Vector2.zero;
                anim.SetBool("PlayerMoving",false);
                break;

            case PlayerState.interact:
                myRigidBody.velocity = Vector2.zero;
                anim.SetBool("PlayerMoving", false);
                break;

            default:
                anim.SetBool("PlayerMoving",false);
                break;

        }
    }

    private IEnumerator attackCo(){
        yield return new WaitForSeconds(attackTime);
        anim.SetBool("Attacking", false);
        if(state!= PlayerState.interact) changeState(PlayerState.idle);
    }
    private IEnumerator knockCo(float knockTime){
        yield return new WaitForSeconds(knockTime);
        myRigidBody.velocity = Vector2.zero;
        if(state!= PlayerState.interact) changeState(PlayerState.idle); 
    }

    private IEnumerator flashingCo(){
        SpriteRenderer playerSprite = GetComponent<SpriteRenderer>();
        for(int i = 0; i < flashBlinks; i++){
            playerSprite.color =  new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 0f);
            yield return new WaitForSeconds(flashLength/(flashBlinks*2f));
            playerSprite.color =  new Color(playerSprite.color.r, playerSprite.color.g, playerSprite.color.b, 1f);
            yield return new WaitForSeconds(flashLength/(flashBlinks*2f));
        }
        canBeHurt = true;
    }

}
