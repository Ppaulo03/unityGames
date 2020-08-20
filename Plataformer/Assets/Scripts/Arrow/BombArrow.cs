using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombArrow : Arrow
{
    public GameObject explosion;
    public Collider2D explosionArea;

    private void OnCollisionEnter2D(Collision2D other) {
       Explode();
    }

    public void Explode(){
       GameObject clone = Instantiate(explosion, transform.position, Quaternion.Euler (Vector3.zero));
       explosionArea.enabled = true;
       Destroy(gameObject);
   }

   private void OnTriggerEnter2D(Collider2D other){
       if(other.gameObject.CompareTag("Enemy")){
           other.gameObject.GetComponent<Enemy>().Hurt(Vector3.zero);
       }
   }
}
