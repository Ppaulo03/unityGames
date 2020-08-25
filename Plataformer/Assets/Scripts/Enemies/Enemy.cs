using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyState{
    walking,
    idle,
    jumping,
    attacking,
    stagger
}

public class Enemy : MonoBehaviour
{
    [SerializeField] protected EnemyState currentState;

    [Header("Movement Settings")]
    [SerializeField] protected float speed = 0f;
    [SerializeField] protected int direction = 1;


    [Header("Hurt Settings")]
    [SerializeField] private GameObject DamageEffect = null;
    [SerializeField] private int lifePoints = 0;
    [SerializeField] private float invunerableTime = 0f;
    [SerializeField] protected float knockBackForce = 0f;

    [Header("Ground Check")]
    [SerializeField] protected LayerMask groundLayer = ~0;
    [SerializeField] protected float groundDistance = 0f;
    [SerializeField] protected float raycastCorrection = 0f;
    [SerializeField] protected Vector3 SizeCorrection = Vector3.zero;


    [Header("Components")]
    [System.NonSerialized] public Rigidbody2D myRigidbody2D;


    private void Awake() {
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void Hurt(Vector3 knockBack){
        if(currentState != EnemyState.stagger){
            currentState = EnemyState.stagger;
            Instantiate(DamageEffect, transform.position, Quaternion.Euler(Vector3.zero));
            myRigidbody2D.velocity = Vector2.zero;
            myRigidbody2D.AddForce(knockBack, ForceMode2D.Impulse);
            lifePoints -= 1;
            if (lifePoints <= 0){
                Die();
            }else{
                StartCoroutine(invunerableCo());
            }
        }
    }

    private void Die(){
        gameObject.SetActive(false);
    }    

    public float PlataformEnd() {
        Vector2 position = transform.position + SizeCorrection;
        Vector2 direction = Vector2.down;
        
        for(float i = -raycastCorrection; i <= raycastCorrection; i += 2*raycastCorrection){
            Debug.DrawRay(position + Vector2.right*i, direction, Color.green);
            RaycastHit2D hit = Physics2D.Raycast(position + Vector2.right*i, direction, groundDistance, groundLayer);
            if (hit.collider == null){
                return (i/raycastCorrection);
            }
        }
        return 0;
    }

    public bool OnGround() {
        Vector2 position = transform.position + SizeCorrection;
        Vector2 direction = Vector2.down;
        
        for(float i = -raycastCorrection; i <= raycastCorrection; i += raycastCorrection){
            Debug.DrawRay(position + Vector2.right*i, direction, Color.green);
            RaycastHit2D hit = Physics2D.Raycast(position + Vector2.right*i, direction, groundDistance, groundLayer);
            if (hit.collider != null){
                return true;
            }
        }
        return false;
    }

    private void OnCollisionEnter2D(Collision2D other) {
         if(other.gameObject.CompareTag("Player")){
            Vector3 direction = (other.gameObject.transform.position - transform.position ).normalized;
            other.gameObject.GetComponent<PlayerController>().Hurt(direction* knockBackForce);
        }
    }
    
    private IEnumerator invunerableCo()
    {
        yield return new WaitForSeconds(invunerableTime);
        currentState = EnemyState.walking;
    }


}
