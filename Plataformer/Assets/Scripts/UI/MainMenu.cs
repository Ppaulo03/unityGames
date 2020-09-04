using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour{
    [Serializable] public struct KeyButton {
        public string name;
        public TMPro.TMP_Text button;
    }

    [Header("Mouse Texturee Settings")]
    [SerializeField] private Texture2D cursorTexture = null;
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;
    [SerializeField] private bool CenterMouse = false;
    private Vector2 hotSpot = Vector2.zero;


    [Header("Control Settings")]
    [SerializeField] private InputManager inputManager = null;
    [SerializeField] private KeyButton[] keyButtons = null;
    private Dictionary<string, TMPro.TMP_Text> buttonKeys;


    [Header("Audio Setting")]
    [SerializeField] private Slider slider = null;
    [SerializeField] private AudioMixer mixer = null;
    [SerializeField] private AudioMixerGroup mixerGroup = null;
    private AudioSource[] audioSources;


    [Header("Graphic Settings")]
    [SerializeField] private Toggle fullScreen = null;
    [SerializeField] private TMPro.TMP_Dropdown resolutionDropdown = null;
    [SerializeField] private TMPro.TMP_Dropdown qualityDropdown = null;
    private Resolution[] resolutions;


    private void Start(){
        Time.timeScale = 1;
        if(CenterMouse) hotSpot = new Vector2(cursorTexture.width / 2 , cursorTexture.height / 2 );
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        slider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        audioSources =  FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in audioSources) audio.outputAudioMixerGroup = mixerGroup;

        buttonKeys = new Dictionary<string, TMPro.TMP_Text>();
        foreach(KeyButton button in keyButtons) buttonKeys[button.name] = button.button;
        foreach(KeyValuePair<string, KeyCode> entry in inputManager.GetKeys())
            buttonKeys[entry.Key].text = entry.Value.ToString();


        fullScreen.isOn = PlayerPrefs.GetInt("FullScreen", 1) == 1 ? true:false;
        
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Quality", 3));
        qualityDropdown.value = PlayerPrefs.GetInt("Quality", 3);
        qualityDropdown.RefreshShownValue();
        
        resolutions =  Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for(int i = 0; i < resolutions.Length; i++){
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if(!PlayerPrefs.HasKey("Resolution")){
                if(resolutions[i].width == Screen.currentResolution.width && 
                resolutions[i].height == Screen.currentResolution.height){
                    currentResolutionIndex = i;
                    PlayerPrefs.SetInt("Resolution", i);
                }
            }else currentResolutionIndex = PlayerPrefs.GetInt("Resolution");
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        Resolution resolution = resolutions[currentResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

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

    public void setFullScreen(bool isFullscreen){
        Screen.fullScreen = isFullscreen;

        if(isFullscreen) PlayerPrefs.SetInt("FullScreen", 1);
        else PlayerPrefs.SetInt("FullScreen", 0);

    }

    public void setResolution(int resolutionIndex){
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("Resolution", resolutionIndex);
    }

    public void setQuality(int qualityIndex){
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("Quality", qualityIndex);
    }

    public void SetVolumeLevel (float sliderValue){
        mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }

    public void RebindKey(string ButtonName){
        StartCoroutine(RebindKeyCo(ButtonName, buttonKeys[ButtonName].text));
    }

    private IEnumerator RebindKeyCo(string ButtonName, string oldValue){
        WaitForSecondsRT wait = new WaitForSecondsRT(1);
        string keyName = "N/A";
        do{
            if(Input.anyKeyDown){
                foreach(KeyCode kc in Enum.GetValues(typeof(KeyCode))){      
                    if(Input.GetKeyDown(kc)){
                        keyName = kc.ToString();
                        break;
                    }
                }
                break;
            }
            yield return wait.NewTime(0.0001f);
        }while(true);

        yield return new WaitForSecondsRealtime(0.1f);

        foreach(KeyValuePair<string, TMPro.TMP_Text> entry in buttonKeys)
        {
            if(entry.Key != ButtonName && entry.Value.text == keyName){
                entry.Value.text = oldValue;
                inputManager.SetKey(entry.Key, (KeyCode) System.Enum.Parse(typeof(KeyCode), oldValue));
                break;
            }
        }
        buttonKeys[ButtonName].text = keyName;
        inputManager.SetKey(ButtonName, (KeyCode) System.Enum.Parse(typeof(KeyCode), keyName));
    }
    
    public void BindKeysDefault(){

        foreach(KeyValuePair<string, KeyCode> entry in inputManager.GetDefault())
        {
            buttonKeys[entry.Key].text = entry.Value.ToString();
        }

    }


}
