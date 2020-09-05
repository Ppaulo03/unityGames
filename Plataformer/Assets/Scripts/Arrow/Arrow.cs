using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{   

    [SerializeField] protected float knockBackForce = 0f;
    [SerializeField] protected Collider2D arrowCollider = null;

    [Header("Components")]
    protected Rigidbody2D myRigidbody;

    
    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        
        if(myRigidbody.velocity.y == 0){
            myRigidbody.bodyType = RigidbodyType2D.Static;
            if(arrowCollider != null) arrowCollider.enabled = false;
        }
        else DirectArrow();

    }

    protected virtual void DirectArrow()
    {
        float angle = (Mathf.Atan2(myRigidbody.velocity.y, myRigidbody.velocity.x) * Mathf.Rad2Deg) - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
        if(other.gameObject.CompareTag("Enemy")){
            Destroy(gameObject);
            other.gameObject.GetComponent<Enemy>().Hurt(myRigidbody.velocity.normalized * knockBackForce);
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer ("Ground")){
            myRigidbody.velocity = Vector2.zero;
        }else Destroy(gameObject);

    }

}
