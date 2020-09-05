using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    [SerializeField] private string levelToLoad = "";

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
        yield return new WaitForSeconds(0.3f);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(levelToLoad);
        while(!asyncOperation.isDone){
            yield return null;
        }
    }
}
