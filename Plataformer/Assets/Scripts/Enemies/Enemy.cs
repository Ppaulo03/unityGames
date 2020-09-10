using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    walking,
    idle,
    jumping,
    attacking,
    stagger,
    freeze,
}

public class Enemy : MonoBehaviour
{
    [SerializeField] protected EnemyState currentState;

    [Header ("Freeze Settings")]
    [SerializeField] protected Color freezeColor = new Color(0f, 1f, 1f, 1f);
    [SerializeField] protected float freezeTime = 0f;


    [Header("Movement Settings")]
    [SerializeField] protected float speed = 0f;
    [SerializeField] protected int direction = 1;


    [Header("Hurt Settings")]
    [SerializeField] protected GameObject DamageEffect = null;
    [SerializeField] protected int lifePoints = 0;
    [SerializeField] private float invunerableTime = 0f;
    [SerializeField] protected float knockBackForce = 0f;
    [SerializeField] protected float knockBackResistence = 0f;


    [Header("Ground Check")]
    [SerializeField] protected LayerMask groundLayer = ~0;
    [SerializeField] protected float groundDistance = 0f;
    [SerializeField] [Range(0.1f, 5)] protected float raycastCorrection = 0.1f;
    [SerializeField] protected Vector3 SizeCorrection = Vector3.zero;


    [Header("Components")]
    protected Rigidbody2D myRigidbody2D;
    protected SpriteRenderer mySpriteRenderer;
    protected Animator anim;

    private void Awake()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }


//============================================ Hurt Functions ===================================================//
    protected virtual void Die()
    {
        
        DieEffect();
        Destroy(gameObject);

    } protected virtual void DieEffect(){}
    public virtual void Hurt(Vector3 knockBack)
    {

        if(currentState != EnemyState.stagger){

            if(currentState == EnemyState.freeze) UnFreeze();
            currentState = EnemyState.stagger;
            Instantiate(DamageEffect, transform.position, Quaternion.Euler(Vector3.zero));
            
            if(myRigidbody2D.bodyType == RigidbodyType2D.Dynamic){
                myRigidbody2D.velocity = Vector2.zero;
                Vector3 RealKnockBack = knockBack - new Vector3(knockBackResistence, knockBackResistence, 0);
                if( RealKnockBack.x < 0) RealKnockBack = new Vector3( 0, RealKnockBack.y, 0);
                if( RealKnockBack.y < 0) RealKnockBack = new Vector3( RealKnockBack.x, 0, 0);
                myRigidbody2D.AddForce(RealKnockBack, ForceMode2D.Impulse);
            }
            
            lifePoints -= 1;
            if (lifePoints <= 0) Die();
            else{
                HurtAction();
                StartCoroutine(invunerableCo());
            }

        }

    }protected virtual void HurtAction(){}
    protected IEnumerator invunerableCo()
    {

        yield return new WaitForSeconds(invunerableTime);
        if(currentState != EnemyState.freeze) currentState = EnemyState.walking;
    
    }


//=============================================== Freeze Functions =============================================//
    public virtual void Freeze()
    {

        anim.speed = 0;
        mySpriteRenderer.color = freezeColor;
        currentState = EnemyState.freeze;
        myRigidbody2D.velocity = Vector2.zero;
        FrezzeBegin();
        if(lifePoints > 0) StartCoroutine(unFreezeCo());

    } protected virtual void FrezzeBegin(){ StopCoroutine(invunerableCo()); }
    protected virtual void UnFreeze()
    {

        anim.speed = 1;
        mySpriteRenderer.color = Color.white;
        currentState = EnemyState.idle;
        FrezzeStop();

    } protected virtual void FrezzeStop(){}
    protected IEnumerator unFreezeCo()
    {

        yield return new WaitForSeconds(freezeTime);
        UnFreeze();

    }


//================================================= Ground Check ================================================//

    protected float PlataformEnd()
    {
        
        Vector2 position = transform.position + SizeCorrection;
        Vector2 direction = Vector2.down;
        
        for(float i = -raycastCorrection; i <= raycastCorrection; i += 2*raycastCorrection){

            Debug.DrawRay(position + Vector2.right*i, direction, Color.green); 
            RaycastHit2D hit = Physics2D.Raycast(position + Vector2.right*i, direction, groundDistance, groundLayer);
            if (hit.collider == null) return ( i / raycastCorrection );
            
        }
        return 0;
        
    }
    protected bool OnGround()
    {

        Vector2 position = transform.position + SizeCorrection;
        Vector2 direction = Vector2.down;
        
        for(float i = -raycastCorrection; i <= raycastCorrection; i += raycastCorrection){
            
            Debug.DrawRay(position + Vector2.right*i, direction, Color.green);
            RaycastHit2D hit = Physics2D.Raycast(position + Vector2.right*i, direction, groundDistance, groundLayer);
            if (hit.collider != null) return true;
            
        }
        return false;
    }
//================================================================================================================//

    protected virtual void Turn()
    {
        
        direction = -direction;
        transform.localScale = new Vector3(-transform.localScale.x,1,1);
        if(myRigidbody2D.bodyType == RigidbodyType2D.Dynamic)
            myRigidbody2D.velocity = new Vector2(0, myRigidbody2D.velocity.y);
    
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        ColisionAction(other);
        if( other.gameObject.CompareTag("Player") )
        {

            Vector3 direction = ( other.gameObject.transform.position - transform.position ).normalized;
            other.gameObject.GetComponent<PlayerController>().Hurt( direction * knockBackForce );

        }

    }
    protected virtual void ColisionAction(Collision2D other){}
    
}
