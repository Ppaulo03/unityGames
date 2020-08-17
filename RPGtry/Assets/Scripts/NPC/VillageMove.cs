using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageMove : MonoBehaviour
{

    public float moveSpeed;
    private Rigidbody2D rigidBody;
    private Animator anim;

    public bool isWalking;
    public float walkTime;
    public float waitTime;
    private float walkCounter;
    private float waitCounter;

    private Vector2 walkDirection;
    public bool canMove;
    private DialogManager dialogManager;

    // Start is called before the first frame update
    void Start()
    {   
        dialogManager = FindObjectOfType<DialogManager>();
        anim = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();

        canMove = true;
        choseDirection();

    }

    // Update is called once per frame
    void Update()
    {
        if(!canMove){
            
            if(!dialogManager.dialogueActive){
                canMove = true;
                anim.SetBool( "Walking", true);
            }
            
            else{
                anim.SetBool( "Walking", false);
                rigidBody.velocity = Vector2.zero;
                return;
            }

        }

        if(isWalking){
            walkCounter -= Time.deltaTime;
            rigidBody.velocity = (walkDirection*moveSpeed);

            if(walkCounter <= 0){
                
                isWalking = false;
                waitCounter = Random.Range(waitTime*0.75f, waitTime*1.25f);
                anim.SetBool( "Walking", false);

            }

        }else{
            rigidBody.velocity = Vector2.zero;
            waitCounter -= Time.deltaTime;
            if(waitCounter <=0 ) choseDirection();

        }
    }

    public void choseDirection(){
        
        switch(Random.Range(0,4)){
            case 0:
                walkDirection = new Vector2(0, 1);
                break;

            case 1:
                walkDirection = new Vector2(1, 0);
                break;

            case 2:
                walkDirection = new Vector2(0, -1);
                break;

            case 3:
                walkDirection = new Vector2(-1, 0);
                break;
        }

        walkCounter = Random.Range(walkTime * 0.75f, walkTime * 1.25f);
        isWalking = true;

        anim.SetFloat( "MoveX", walkDirection.x);
        anim.SetFloat( "MoveY", walkDirection.y);
        anim.SetBool( "Walking", true);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        walkDirection = -walkDirection;
        rigidBody.velocity = (walkDirection*moveSpeed);

        anim.SetFloat( "MoveX", walkDirection.x);
        anim.SetFloat( "MoveY", walkDirection.y);
    }
}
