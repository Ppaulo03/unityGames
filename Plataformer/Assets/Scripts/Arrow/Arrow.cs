using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{   

    public float knockBackForce;
    public Collider2D arrowCollider;
    [Header("Components")]
    [System.NonSerialized]
    public Rigidbody2D myRigidbody;

    
    private void Awake() {
        myRigidbody = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate() {
        if(myRigidbody.velocity.y == 0){
            myRigidbody.bodyType = RigidbodyType2D.Static;
            if(arrowCollider != null) arrowCollider.enabled = false;

        }
        else DirectArrow();
    }
    public virtual void DirectArrow(){
        float angle = (Mathf.Atan2(myRigidbody.velocity.y, myRigidbody.velocity.x) * Mathf.Rad2Deg) - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Enemy")){
            Destroy(gameObject);
            other.gameObject.GetComponent<Enemy>().Hurt(myRigidbody.velocity.normalized * knockBackForce);
        }
        else{
            myRigidbody.velocity = Vector2.zero;
        }

    }

}
