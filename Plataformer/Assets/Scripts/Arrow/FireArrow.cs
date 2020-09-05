using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArrow : Arrow
{
    [SerializeField] private GameObject particles = null;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Enemy")){
            Destroy(gameObject);
            other.gameObject.GetComponent<Enemy>().Hurt(myRigidbody.velocity.normalized * knockBackForce);
        }
        else{
            myRigidbody.velocity = Vector2.zero;
            Destroy(particles);
        }
    }
    
}
