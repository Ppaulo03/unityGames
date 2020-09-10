using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingPlataform : MonoBehaviour
{

    [SerializeField] private float fallTime = 0f, respawnTime = 0f, syncDelay = 0f;
    [SerializeField] private BoxCollider2D myCollider2D = null;
    [SerializeField] private BoxCollider2D mytriggerCollider2D = null;
    private Rigidbody2D myRigidbody;
    private Vector3 StartPos;
    private Animator anim;
    private AudioSource myAudioSource;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAudioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        StartPos = transform.position;
        myRigidbody.bodyType = RigidbodyType2D.Static;
        StartCoroutine(DelayCo());
    }

    private void OnCollisionEnter2D(Collision2D other) {
        
        if(other.gameObject.CompareTag("Ground")){   
            if(myRigidbody.bodyType == RigidbodyType2D.Dynamic){
                myRigidbody.bodyType = RigidbodyType2D.Static;
                anim.SetBool("Broke",true);
                myCollider2D.enabled = false;
                mytriggerCollider2D.enabled = false;
                myAudioSource.Play();
                StartCoroutine(RespawnCo());
            }
        }
        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if( other.gameObject.CompareTag("Player") )
            other.gameObject.GetComponent<PlayerController>().Hurt( Vector3.zero );
    }

    private IEnumerator RespawnCo()
    {
        yield return new WaitForSeconds(respawnTime);
        anim.SetBool("Broke",false);
        transform.position = StartPos;
        myCollider2D.enabled = true;
        mytriggerCollider2D.enabled = true;
        StartCoroutine(FallCo());
    }
    private IEnumerator FallCo()
    {
        yield return new WaitForSeconds(fallTime);
        myRigidbody.bodyType = RigidbodyType2D.Dynamic;
    }
    private IEnumerator DelayCo(){
        yield return new WaitForSeconds(syncDelay);
        StartCoroutine(FallCo());
    }

}
