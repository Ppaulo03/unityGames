using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    
    [SerializeField] private float knockBackForce = 0f;
    private Animator anim;
    
    private void Awake() {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        anim.SetTrigger("Hit");
        if( other.gameObject.CompareTag("Player") )
        {

            Vector3 direction = ( other.gameObject.transform.position - transform.position ).normalized;
            other.gameObject.GetComponent<PlayerController>().Hurt( direction * knockBackForce );

        }
        StartCoroutine(disapearCo());
    }
    private IEnumerator disapearCo(){
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
