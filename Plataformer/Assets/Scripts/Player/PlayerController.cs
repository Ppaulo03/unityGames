using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState{
    onGround,
    jumping,
    attacking,
    stagger,
}

public class PlayerController : MonoBehaviour
{

    public PlayerState currentState;

    [Header("Life Settings")]
    public float invunerableTime;
    public int maxLife;
    private int currentLife;


    [Header("Moviment Settings")]
    public float speed;
    public float jumpForce;
    public float doubleJumpForce;
    public bool doubleJump;
    

    [Header("Fire Settings")]
    public Transform firePoint;
    public Transform BowPositon;
    public float arrowSpeed;
    public float chargeTime;
    public float currentStrenght = 0;
    private float arrowAngle;
    public float attackTime;
    private bool attackRunning;
    
    [Header("Arrows Settings")]
    public GameObject[] arrows;
    public Signal[] actionArrow;
    public int chosenArrow;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public float groundDistance;
    public float raycastCorrection;

    [Header("Components")]
    private Rigidbody2D myRigidbody2D;
    private Animator anim;
    private SpriteRenderer mySpriteRenderer;

    private bool deleyedAnim;


    private void Awake()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        currentLife = maxLife;
        StartCoroutine(delayCo(0.2f));
    }

    private void Update(){
        setAnimGround();
        if(currentState != PlayerState.stagger)
            if(currentState != PlayerState.attacking){
                SetGround();
                if(Input.GetButtonDown("Jump")) Jump();
                else if(Input.GetButtonDown("Fire1")) PreparFire();
                else if(Input.GetKeyDown(KeyCode.G)){
                    if( actionArrow[chosenArrow] != null) actionArrow[chosenArrow].Raise();
                }
            }
    
            else{
                posBow();
                if(chargeTime != 0 && currentStrenght < arrowSpeed) currentStrenght += arrowSpeed/chargeTime * Time.deltaTime;
                else currentStrenght = arrowSpeed;
                if(Input.GetButtonUp("Fire1") || !Input.GetButton("Fire1")) Fire();
            }
        
        if(Input.GetButtonDown("ChangeArrow")){
            chosenArrow += (int)Input.GetAxisRaw("ChangeArrow");
            if(chosenArrow >= arrows.Length) chosenArrow = 0;
            else if(chosenArrow < 0) chosenArrow = arrows.Length - 1;
        }

    }
    
    private void FixedUpdate()
    {   
        
        if(currentState != PlayerState.attacking && currentState != PlayerState.stagger){
            Run();
        } 
    }

    private void setAnimGround(){
        if(IsGrounded()){
                anim.SetBool("Jumping",false);
                anim.SetBool("DoubleJump",false);
                anim.SetBool("Falling",false);
        }
        else{
            if(!deleyedAnim) anim.SetBool("Falling",true);
        }
    }
    private void SetGround(){
        
        if(IsGrounded()){
                currentState = PlayerState.onGround;
                doubleJump = true;
        }
        else{
            currentState = PlayerState.jumping;
        }
  
    }

    private bool IsGrounded() {

        Vector2 position = transform.position;
        
        for(float i = -raycastCorrection; i <= raycastCorrection; i += raycastCorrection){

            RaycastHit2D hit = Physics2D.Raycast(position + Vector2.right*i, Vector2.down, groundDistance,  groundLayer);
            if (hit.collider != null){
                
                return true;
            }
        }

        
        return false;
    }

    private void Flip(float xValue){
            if(xValue > 0){
                mySpriteRenderer.flipX = false;
            }
            else if(xValue < 0){
                mySpriteRenderer.flipX = true;
            }
    }
    
    private void Run()
    {
        
        float xValue;
        xValue = Input.GetAxisRaw("Horizontal");

        if(xValue != 0){
            anim.SetBool("Running",true);
            Flip(xValue);
        }
        else{
            anim.SetBool("Running",false);
        }
        
        myRigidbody2D.velocity = new Vector2(xValue*speed, myRigidbody2D.velocity.y);
    }
    
    private void Jump()
    {
        if(IsGrounded()){
            anim.SetBool("Jumping",true);
            myRigidbody2D.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        }

        else if(doubleJump){
            myRigidbody2D.velocity = new Vector3(myRigidbody2D.velocity.x, 0, myRigidbody2D.velocity.y);
            myRigidbody2D.AddForce(Vector3.up * doubleJumpForce, ForceMode2D.Impulse);
            anim.SetBool("DoubleJump", true);
            doubleJump = false;
        }
    }

    private void posBow(){

        Vector3 mousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = mousPos - BowPositon.position;

        float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;      
        arrowAngle = angle - 90;

        BowPositon.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        if(BowPositon.rotation.z > -0.7f && BowPositon.rotation.z < 0.7f){
            transform.localScale = new Vector3(1,1,1);
            BowPositon.localScale = new Vector3(1,1,1);
        }
        else{
            transform.localScale = new Vector3(-1,1,1);
            BowPositon.localScale = new Vector3(-1,-1,1);
        }

    }

    private void PreparFire(){
        anim.SetBool("FiringArrow", true);
        anim.SetTrigger("Fire");
        currentState = PlayerState.attacking;
        myRigidbody2D.velocity = new Vector2( 0, myRigidbody2D.velocity.y);
        
        if(mySpriteRenderer.flipX == true){
            mySpriteRenderer.flipX = false;
            transform.localScale = new Vector3(-1,1,1);
            BowPositon.localScale = new Vector3(-1,-1,1);
        }

    }
    
    private void Fire()
    {

        anim.SetBool("Jumping", false);
        anim.SetBool("DoubleJump", false);
        anim.SetBool("FiringArrow", false);
        if(!attackRunning)
            StartCoroutine(attackCo());

    }
    
    public void Hurt(Vector3 knockBack){
        if(currentState != PlayerState.stagger){
            Reset();
            currentLife -= 1;
            if(currentLife <= 0){
                myRigidbody2D.velocity = Vector2.zero;
                SetGround();
                transform.position = GameObject.FindWithTag("Respawn").transform.position;
                currentLife = maxLife;
            }else{          
                currentState = PlayerState.stagger;
                myRigidbody2D.velocity = Vector2.zero;
                myRigidbody2D.AddForce(knockBack, ForceMode2D.Impulse);
                StartCoroutine(invunerableCo());
            }
            
        }
    }

    private void Reset(){
        foreach(AnimatorControllerParameter parameter in anim.parameters) {            
                anim.SetBool(parameter.name, false);            
        }
        anim.SetTrigger("Idle");
        currentStrenght = 0;
        Flip(transform.localScale.x);
        transform.localScale = new Vector3(1,1,1);
        attackRunning = false;
        SetGround();
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("LevelArea")){
            transform.position = GameObject.FindWithTag("Respawn").transform.position;
        }
    }
    
    private IEnumerator attackCo()
    {

        attackRunning = true;
        GameObject clone = Instantiate(arrows[chosenArrow], firePoint.position, Quaternion.Euler (new Vector3(0,0,arrowAngle)));
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (new Vector2(mousePos.x,mousePos.y) - new Vector2(BowPositon.position.x, BowPositon.position.y)).normalized;
        clone.GetComponent<Rigidbody2D>().AddForce(dir*currentStrenght, ForceMode2D.Impulse);
        yield return new WaitForSeconds(attackTime);
        Reset();

    }

    private IEnumerator delayCo(float delayTime){
        deleyedAnim = true;
        yield return new WaitForSeconds(delayTime);
        deleyedAnim = false;
    }

    private IEnumerator invunerableCo(){
        yield return new WaitForSeconds(invunerableTime);
        SetGround();
    }
}
