using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    
    public void Change(string Scene){
        Time.timeScale = 1;
        StartCoroutine(playGameCo(Scene));
    }

    private IEnumerator playGameCo(string Scene){
        yield return new WaitForSeconds(2f);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(Scene);
        while(!asyncOperation.isDone){
            yield return null;
        }
    }

}
