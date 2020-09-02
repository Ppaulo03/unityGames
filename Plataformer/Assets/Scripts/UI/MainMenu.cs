using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour{


    [SerializeField] private AudioMixer mixer = null;
    [SerializeField] private Slider slider = null;
    [SerializeField] private AudioMixerGroup mixerGroup = null;
    private AudioSource[] audioSources = null;

    private void Start(){
        Time.timeScale = 1;
        slider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        audioSources =  FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in audioSources) audio.outputAudioMixerGroup = mixerGroup;
    }

    public void SetVolumeLevel (float sliderValue)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }

    public void PlayGame(){
        StartCoroutine(playGameCo());
    }
    
    public void BackToMainMenu(){
        StartCoroutine(MainMenuCo());
    }

    public void Unpause(){
        Time.timeScale = 1;
    }

    private IEnumerator playGameCo(){
        yield return new WaitForSeconds(2f);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Level1");
        while(!asyncOperation.isDone){
            yield return null;
        }
    }

    private IEnumerator MainMenuCo(){
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(0);
        while(!asyncOperation.isDone){
            yield return null;
        }
    }

    public void QuitGame(){
        Application.Quit();
    }

}
