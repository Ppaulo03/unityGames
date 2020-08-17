using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNewArea : MonoBehaviour
{

    [Header("Level Variables")]
    public string pointName;
    public string levelToLoad;
    private PlayerController player;

    [Header("Transition Variables")]
    public GameObject fadeInPanel;
    public GameObject fadeOutPanel;
    public float fadeAwait;

    private void Awake() {
        if(fadeInPanel != null){
            GameObject panel = Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity) as GameObject;
            Destroy(panel, 1);
        }
    }

    void Start(){
        player = FindObjectOfType<PlayerController>();
        if(player.startPoint == pointName){
            player.transform.position = transform.position;

            CamaraController camera = FindObjectOfType<CamaraController>();
            camera.transform.position = new Vector3 (transform.position.x,transform.position.y, camera.transform.position.z);
        } 
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.name == "Player"){
            player.startPoint = pointName;
            StartCoroutine(FadeCo());
        }
    }

    private IEnumerator FadeCo(){
        if(fadeOutPanel != null ){
            Instantiate(fadeOutPanel, Vector3.zero, Quaternion.identity);
        }
        yield return new WaitForSeconds(fadeAwait);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(levelToLoad);
        while(!asyncOperation.isDone){
            yield return null;
        }
       
        
    }
}
