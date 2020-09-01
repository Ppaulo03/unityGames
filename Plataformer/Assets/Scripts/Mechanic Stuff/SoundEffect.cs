using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{   
    
    [Header("Sounds")]
    [SerializeField] private AudioClip Idle = null;
    [SerializeField] [Range(0,1)] private float idleVolume = 0f;

    private AudioSource myAudioSource;

    private void Start() {
        myAudioSource = GetComponent<AudioSource>();
    }
    
    public void changeSound(AudioClip newSound, float volume){
        myAudioSource.loop = false;
        myAudioSource.clip = newSound;
        myAudioSource.volume = volume;
        if(Idle != null) StartCoroutine(idleSoundCo());
    }

    private void IdleSound(){
        myAudioSource.clip = Idle;
        myAudioSource.loop = true;
         myAudioSource.volume = idleVolume;
        myAudioSource.Play();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){
            if(Idle != null) IdleSound();
        }
    }
    
    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){
            myAudioSource.Stop();
            StopCoroutine(idleSoundCo());
        }
    }

    private IEnumerator idleSoundCo(){
        yield return new WaitForSeconds(myAudioSource.clip.length + 0.2f);
            IdleSound();
    }
}
