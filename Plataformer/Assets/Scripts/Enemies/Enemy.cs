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
    public EnemyState currentState;

    [Header("Movement Settings")]
    public float speed;
    public int direction = 1;
    public float knockBackForce;

    [Header("Hurt Settings")]
    public int lifePoints;
    public float invunerableTime;
    public GameObject DamageEffect;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public float groundDistance;
    public float raycastCorrection;
    public Vector3 SizeCorrection;

    [Header("Components")]
    [System.NonSerialized]
    public Rigidbody2D myRigidbody2D;

    private void Awake() {
        myRigidbody2D = GetComponent<Rigidbody2D>();
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
