using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidBall : MonoBehaviour
{
    [SerializeField] private GameObject acidPool = null;

    [SerializeField] private float knockBackForce = 0f;

    [SerializeField] private FloatValue numPools = null;
    [SerializeField] private float maxPools = 0f;

    [Header("Components")]
    protected Rigidbody2D myRigidbody;

    private void Awake() {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate(){
         DirectBall();
    }

    public virtual void DirectBall(){
        float angle = (Mathf.Atan2(myRigidbody.velocity.y, myRigidbody.velocity.x) * Mathf.Rad2Deg) + 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        
        if(other.gameObject.CompareTag("Player")){
            Vector3 direction = (other.gameObject.transform.position - transform.position ).normalized;
            other.gameObject.GetComponent<PlayerController>().Hurt(direction* knockBackForce);
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer ("Ground")){
            if(numPools.Value <= maxPools)
                Instantiate(acidPool, other.collider.ClosestPoint(transform.position), Quaternion.Euler (Vector3.zero));
        }

        Destroy(gameObject);
    }

}
