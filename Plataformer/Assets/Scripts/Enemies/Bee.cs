using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : FlyingEnemy
{

    [Header("Sounds")]
    [SerializeField] private AudioClip buzzSound = null;
    [SerializeField] private float buzzVolume = 0.05f;
    private AudioSource myAudioSource;

    private void Start()
    {

        myAudioSource = GetComponent<AudioSource>();
        currentSpeed = Random.Range( speed*0.5f, speed*1.5f );
        currentState = EnemyState.walking;
    
    }
    private void FixedUpdate()
    {

        if( currentState == EnemyState.idle ) currentState = EnemyState.walking;
        if( currentState == EnemyState.walking ){
            Move();
            GetHeight();
        }

    }

    private void BuzzSound()
    {
        myAudioSource.loop = true;
        myAudioSource.clip = buzzSound;
        myAudioSource.volume = buzzVolume;
        myAudioSource.Play();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if( other.gameObject.CompareTag("Player") ) BuzzSound();
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if( other.gameObject.CompareTag("Player") ) myAudioSource.Stop();
    }

    private void OnCollisionEnter2D( Collision2D other )
    {
        if( other.gameObject.CompareTag("Player") ){

            Vector3 direction = (other.gameObject.transform.position - transform.position ).normalized;
            other.gameObject.GetComponent<PlayerController>().Hurt( direction* knockBackForce );
            
        }else if( !other.gameObject.CompareTag("Arrow") && turn ){
            Turn();
            turn = false;
            currentSpeed = Random.Range( speed*0.5f, speed*1.5f );
            StartCoroutine(TurnCo());
        }
    }


}
