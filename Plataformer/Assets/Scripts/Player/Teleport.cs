using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Signal levelToLoad = null;

    [Header("Components")]
    private AudioSource myAudioSource;
    private Animator anim;

    private void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")){
            myAudioSource.Play();
            anim.SetTrigger("Teleport");
            StartCoroutine(TeleportCo());
        }
    }

    private IEnumerator TeleportCo()
    {
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0;
        levelToLoad.Raise();
    }
}
