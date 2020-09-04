using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceArrow : Arrow
{
    [SerializeField] private GameObject particles = null;
    [SerializeField] private GameObject explosion = null;
    
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Enemy")){
            Destroy(gameObject);
            other.gameObject.GetComponent<Enemy>().Hurt(myRigidbody.velocity.normalized * knockBackForce);
            other.gameObject.GetComponent<Enemy>().Freeze();
            
        }
        else{
            myRigidbody.velocity = Vector2.zero;
            Destroy(particles);
        }
    }

    public void Explode(){
        Instantiate(explosion, transform.position, Quaternion.Euler (Vector3.zero));
        Destroy(gameObject);
    }
}
