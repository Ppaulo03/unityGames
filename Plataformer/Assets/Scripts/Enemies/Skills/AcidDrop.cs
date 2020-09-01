using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidDrop : MonoBehaviour
{
    [SerializeField] private float fallTime = 0f;
    [SerializeField] private GameObject acidPool = null;

    [Header("Components")]
    private Rigidbody2D myRigidbody;
    private Animator myAnimator;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        StartCoroutine(DropCo());
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){
            other.gameObject.GetComponent<PlayerController>().Hurt(Vector3.zero);
        }
        else if(other.gameObject.layer == LayerMask.NameToLayer ("Ground")){
        
            Instantiate(acidPool, other.ClosestPoint(transform.position), Quaternion.Euler (Vector3.zero));
            myAnimator.SetTrigger("Collide");

            if(myRigidbody.bodyType == RigidbodyType2D.Kinematic){
                Destroy(gameObject);
            }else{
                myRigidbody.bodyType = RigidbodyType2D.Static;
                StartCoroutine(DestroyCo());
            }
            
        }

    }

    private IEnumerator DropCo(){
        yield return new WaitForSeconds(fallTime);
        myRigidbody.bodyType = RigidbodyType2D.Dynamic;
    }

    private IEnumerator DestroyCo(){
        yield return new WaitForSeconds(fallTime);
        Destroy(gameObject);
    }

}
