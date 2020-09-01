using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
   [Header("Sounds")]
    [SerializeField] private AudioClip Initial = null;
    [SerializeField] [Range(0,1)] private float InitialVolume = 0f;
    [SerializeField] private AudioClip NewAudio = null;
    [SerializeField] [Range(0,1)] private float NewAudioVolume = 0f;

    private AudioSource mainCameraAudio = null;

    private void Start() {
        mainCameraAudio = Camera.main.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){
            mainCameraAudio.clip = NewAudio;
            mainCameraAudio.volume = NewAudioVolume;
            mainCameraAudio.Play();
        }
    }
    
    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){
            mainCameraAudio.clip = Initial;
            mainCameraAudio.volume = InitialVolume;
            mainCameraAudio.Play();
        }
    }
}
