using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    onGround,
    crouching,
    jumping,
    attacking,
    stagger,
}

public class PlayerController : MonoBehaviour
{

    [SerializeField] private InputManager inputManager = null;
    [SerializeField] private ArrowManagement arrowManager = null;
    [SerializeField] private PlayerState currentState;
    [SerializeField] private Range numMudas = null;
    [SerializeField] private Range growOrder = null;


    [Header("UI Signals")]
    [SerializeField] private Signal PauseSignal = null;
    [SerializeField] private Signal UnPauseSignal = null;
    [SerializeField] private Signal Submit = null;


    [Header("Life Settings")]
    [SerializeField] private GameObject DamageEffect = null;
    [SerializeField] private float invunerableTime = 0f;
    [SerializeField] private int maxLife = 0;
    [SerializeField] private FloatValue currentLife = null;
    [SerializeField] private FloatValue herathContainers = null;
    [SerializeField] private Signal HealthChange = null;
    [SerializeField] private AudioClip HurtSound = null;


    [Header("Moviment Settings")]
    [SerializeField] private float speed = 0f;
    [SerializeField] private float jumpForce = 0f;
    [SerializeField] private float doubleJumpForce = 0f;
    [SerializeField] private bool doubleJump = false;
    [SerializeField] private float slideTime = 0f;


    [Header("AirTime")]
    [SerializeField] private float resistenciaDoAr = 0f;
    [SerializeField] private float gravidade = 0f;
    [SerializeField] private AudioClip JumpSound = null;
    

    [Header("Fire Settings")]
    [SerializeField] private Signal Fired = null;
    [SerializeField] private Transform firePoint = null;
    [SerializeField] private Transform BowPositon = null;
    [SerializeField] private float arrowSpeed = 0f;
    [SerializeField] private float chargeTime = 0f;
    [SerializeField] private float minStrength = 0f;
    private float currentStrenght = 0f;
    private float arrowAngle;
    [SerializeField] private float attackTime = 0f;
    [SerializeField] private float arrowDelay = 0f;
    private bool attackRunning;
    [SerializeField] private AudioClip ArrowSound = null;
    [SerializeField] private AudioClip FailSound = null;
    

    [Header("Arrows Settings")]
    [SerializeField] private Signal changeArrow = null;
    [SerializeField] private IntValue chosenArrow = null;


    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer = ~0;
    [SerializeField] private float groundDistance = 0f;
    [SerializeField] private float raycastCorrection = 0f;


    [Header("Components")]
    private Rigidbody2D myRigidbody2D;
    private Animator anim;
    private SpriteRenderer mySpriteRenderer;
    private AudioSource myAudioSource;

    private bool deleyedAnim;
    private bool paused;

    private void Awake()
    {

        Physics2D.IgnoreLayerCollision(10, 11, false);

        myRigidbody2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myAudioSource = GetComponent<AudioSource>();

        currentLife.Value = maxLife;
        herathContainers.Value = maxLife;

        numMudas.currentValue = 0;
        growOrder.currentValue  = 0;
        
        currentStrenght = minStrength;
        paused = false;
        Time.timeScale = 1;
        StartCoroutine(delayCo(0.2f));

    }
    private void Update()
    {

        if(inputManager.GetButtonDown("Pause")) Pause();
        if(!paused){

            setAnimGround();
            if(inputManager.GetButtonDown("Submit")) Submit.Raise();
            
            if(inputManager.GetButtonDown("Effect")) arrowManager.ActiveEffect(chosenArrow.Value);

            if(currentState != PlayerState.stagger){
                
                if(currentState != PlayerState.attacking){
                    
                    SetGround();
                    if(inputManager.GetButtonDown("Jump")) Jump();
                    else if(inputManager.GetButtonDown("Crouch")) Crouch();
                    else if(inputManager.GetButtonUp("Crouch")) Stand();
                    else if(inputManager.GetButtonDown("Fire")) PreparFire();
                
                }
                
                else{
                    
                    posBow();
                    chargeBow();
                    if( inputManager.GetButtonUp("Fire") || !inputManager.GetButton("Fire") ) Fire();
                    if( Horizontal() == 0) myRigidbody2D.velocity = new Vector2(0, myRigidbody2D.velocity.y);
                
                }

            }
            if(inputManager.GetButtonDown("ChangeArrowLeft")) ChangeArrow(-1);
            else if(inputManager.GetButtonDown("ChangeArrowRight")) ChangeArrow(1);

        }

    }
    private void FixedUpdate()
    {   
        
        if(currentState == PlayerState.jumping || currentState == PlayerState.onGround) Run();

    }
    

    //=========================================== Settings Function ====================================//

    private void Pause()
    {

        if( Time.timeScale == 0)
        {
            Time.timeScale = 1;
            UnPauseSignal.Raise();
        }
        else
        {
            PauseSignal.Raise();
            Time.timeScale = 0;
        }
        
    }
    private float Horizontal()
    {

        float value = 0;
        if(inputManager.GetButton("Left")) value -= 1;
        if(inputManager.GetButton("Right")) value += 1;
        return value;

    }
    private void setAnimGround()
    {

        if(IsGrounded())
        {
                anim.SetBool("Jumping",false);
                anim.SetBool("DoubleJump",false);
                anim.SetBool("Falling",false);
        }
        else if(!deleyedAnim)
        { 
            anim.SetBool("Falling",true);
            anim.SetBool("Crouch",false);
        }
    
    }
    private void SetGround()
    {
        
        if(IsGrounded())
        {

                myRigidbody2D.drag = 0;
                myRigidbody2D.gravityScale = 1;
                if(currentState != PlayerState.crouching) currentState = PlayerState.onGround;
                doubleJump = true;

        }
        else
        {
            myRigidbody2D.drag = resistenciaDoAr;
            myRigidbody2D.gravityScale = gravidade;
            if(currentState == PlayerState.stagger) currentState = PlayerState.jumping;
            else StartCoroutine(fallDelay());

        }
  
    }
    private bool IsGrounded()
    {

        Vector2 position = transform.position;    
        for( float i = - raycastCorrection; i <= raycastCorrection; i += raycastCorrection){

            //Debug.DrawRay(position + Vector2.right*i, Vector2.down, Color.green);
            RaycastHit2D hit = Physics2D.Raycast( position + Vector2.right * i, Vector2.down, groundDistance,  groundLayer);
            if (hit.collider != null){          
                return true;
            }

        }
        return false;
    
    }
    private void Flip(float xValue)
    {

        if(xValue > 0) mySpriteRenderer.flipX = false;
        else if(xValue < 0) mySpriteRenderer.flipX = true;
            
    }
    private void Reset()
    {

        foreach(AnimatorControllerParameter parameter in anim.parameters) {            
                anim.SetBool(parameter.name, false);            
        }

        anim.SetTrigger("Idle");

        currentStrenght = minStrength;

        Flip(transform.localScale.x);
        transform.localScale = new Vector3(1,1,1);

        attackRunning = false;
        SetGround();

    }


    //=================================== Movement Functions ==========================================================//
    
    private void Crouch()
    {

        anim.SetBool("Crouch", true);
        currentState = PlayerState.crouching;
        StartCoroutine(slideCo());

    } 
    private void Stand()
    {

        anim.SetBool("Crouch", false);
        currentState = PlayerState.onGround;
        SetGround();
    
    }
    private void Run()
    {
        
        float xValue;
        xValue = Horizontal();

        if(xValue != 0){
            anim.SetBool("Running",true);
            myRigidbody2D.velocity = new Vector2(xValue*speed, myRigidbody2D.velocity.y);
            Flip(xValue);
        }
        else{
            if(IsGrounded()) myRigidbody2D.velocity = new Vector2(0, myRigidbody2D.velocity.y);
            anim.SetBool("Running",false);
        }
        
    }
    private void Jump()
    {
        if(IsGrounded()){
            
            myRigidbody2D.drag = resistenciaDoAr;
            myRigidbody2D.gravityScale = gravidade;
            currentState = PlayerState.jumping;

            anim.SetBool("Jumping",true);
            anim.SetBool("Crouch",false);
            myRigidbody2D.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
            if(JumpSound != null){
                myAudioSource.clip = JumpSound;
                myAudioSource.Play(0);
            }
        }

        else if(doubleJump){
            myRigidbody2D.drag = resistenciaDoAr;
            myRigidbody2D.gravityScale = gravidade;
            currentState = PlayerState.jumping;

            myRigidbody2D.velocity = new Vector3(myRigidbody2D.velocity.x, 0, myRigidbody2D.velocity.y);
            myRigidbody2D.AddForce(Vector3.up * doubleJumpForce, ForceMode2D.Impulse);
            anim.SetBool("DoubleJump", true);
            doubleJump = false;
            if(JumpSound != null){
                myAudioSource.clip = JumpSound;
                myAudioSource.Play(0);
            }
        }
    }
    

    //=================================== Arrow Functions ==========================================================//

    public void ChangeArrow(int change)
    {
        
        chosenArrow.Value += change;
        if(chosenArrow.Value < 0) chosenArrow.Value = arrowManager.arrows.Length - 1;
        else if(chosenArrow.Value >= arrowManager.arrows.Length) chosenArrow.Value = 0;
        changeArrow.Raise();

    }
    private void posBow()
    {

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
    private void chargeBow()
    {
        if(chargeTime != 0 && currentStrenght < arrowSpeed) currentStrenght += arrowSpeed/chargeTime * Time.deltaTime;
        else currentStrenght = arrowSpeed;
    }
    private void PreparFire()
    {

        anim.SetBool("FiringArrow", true);
        anim.SetTrigger("Fire");

        currentState = PlayerState.attacking;

        myRigidbody2D.drag = 2f;
        if(mySpriteRenderer.flipX == true){

            mySpriteRenderer.flipX = false;
            transform.localScale = new Vector3(-1,1,1);
            BowPositon.localScale = new Vector3(-1,-1,1);

        }

    }
    private void Fire()
    {
        
        myRigidbody2D.drag = 0;
        anim.SetBool("Jumping", false);
        anim.SetBool("DoubleJump", false);
        anim.SetBool("FiringArrow", false);
        if(!attackRunning)
            StartCoroutine(attackCo());

    }
    

    //=================================== Damage Functions ===================================================//

    private void Die()
    {

        myRigidbody2D.velocity = Vector2.zero;
        transform.position = GameObject.FindWithTag("Respawn").transform.position;
        currentLife.Value = maxLife;
        HealthChange.Raise();
        Reset();

    }
    public void KnockBack(Vector3 knockBack)
    {

        currentState = PlayerState.stagger;
        myRigidbody2D.velocity = Vector2.zero;
        myRigidbody2D.AddForce(knockBack, ForceMode2D.Impulse);
        StartCoroutine(invunerableCo());

    }
    public void Hurt(Vector3 knockBack)
    {
        
        if(currentState != PlayerState.stagger){

            Physics2D.IgnoreLayerCollision(10, 11, true);
           
            if(HurtSound != null){
                myAudioSource.clip = HurtSound;
                myAudioSource.Play(0);
            }

            Reset();
            currentState = PlayerState.stagger;
            
            Instantiate(DamageEffect, transform.position, Quaternion.Euler(Vector3.zero));
            currentLife.Value -= 1;
            HealthChange.Raise();
            
            if(currentLife.Value <= 0) Die();
            else{

                if( knockBack != Vector3.zero ) KnockBack(knockBack);
                else StartCoroutine(invunerableCo());

            }
        
        }
        
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        
        if(other.CompareTag("LevelArea")){
            Die();
        }

    }


    //=================================== Coroutines ==========================================================//

    private IEnumerator attackCo()
    {

        attackRunning = true;

        myAudioSource.clip = ArrowSound;
        myAudioSource.Play();

        yield return new WaitForSeconds(0.05f);
        if(currentStrenght < (arrowSpeed / 2)) yield return new WaitForSeconds(arrowDelay);
        
        if(arrowManager.arrows[chosenArrow.Value].currentQtd > 0){

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = (new Vector2(mousePos.x,mousePos.y) - new Vector2(BowPositon.position.x, BowPositon.position.y)).normalized;

            GameObject clone = Instantiate(arrowManager.arrows[chosenArrow.Value].arrowObject, firePoint.position, Quaternion.Euler (new Vector3(0,0,arrowAngle)));
            clone.GetComponent<Rigidbody2D>().AddForce(dir*currentStrenght, ForceMode2D.Impulse);
            
            arrowManager.arrows[chosenArrow.Value].currentQtd --;
            if(arrowManager.arrows[chosenArrow.Value].currentQtd == 0) StartCoroutine(reloadCo(chosenArrow.Value));
            Fired.Raise();
        
        }
        else{

            myAudioSource.clip = FailSound;
            myAudioSource.Play();
        
        }
        yield return new WaitForSeconds(attackTime);
        Reset();

    }
    private IEnumerator reloadCo(int arrowIndex)
    {
        yield return new WaitForSeconds(arrowManager.arrows[arrowIndex].CooldownTime);
            arrowManager.arrows[arrowIndex].currentQtd = arrowManager.arrows[arrowIndex].maxOfArrows;
    }
    private IEnumerator delayCo(float delayTime)
    {

        deleyedAnim = true;
        yield return new WaitForSeconds(delayTime);
        deleyedAnim = false;

    }  
    private IEnumerator invunerableCo()
    {

        for(int i = 0; i < 4; i++ ){
            mySpriteRenderer.color = new Color(1f,1f,1f,0.5f);
            yield return new WaitForSeconds(invunerableTime/8);
            mySpriteRenderer.color = new Color(1f,1f,1f,1f);
            yield return new WaitForSeconds(invunerableTime/8);
        }
        
        Physics2D.IgnoreLayerCollision(10, 11, false);
        SetGround();
    }
    private IEnumerator slideCo()
    {

        yield return new WaitForSeconds(slideTime);
        myRigidbody2D.velocity = Vector2.zero;

    }
    private IEnumerator fallDelay()
    {

        yield return new WaitForSeconds(1f);
        if(!IsGrounded())
            if(currentState != PlayerState.attacking)
                 currentState = PlayerState.jumping;
        
    }
    
}
