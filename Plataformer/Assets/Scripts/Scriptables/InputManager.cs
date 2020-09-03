using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Signal", menuName = "ScriptableObject/InputManager", order = 0)]
public class InputManager : ScriptableObject{
    [System.Serializable] public struct KeyButton {
        public string name;
        public KeyCode key;
    }
    [SerializeField] private KeyButton[] DefaultKeys = null;
    private Dictionary<string, KeyCode> DefaultKeysDict;
    private Dictionary<string, KeyCode> buttonKeys;
    private void OnEnable(){
        DefaultKeysDict = new Dictionary<string, KeyCode>();
        foreach(KeyButton button in DefaultKeys)DefaultKeysDict[button.name] = button.key;
        
        buttonKeys = new Dictionary<string, KeyCode>();

        foreach(KeyValuePair<string, KeyCode> entry in DefaultKeysDict){
            buttonKeys[entry.Key] = (KeyCode) PlayerPrefs.GetInt(entry.Key, (int) entry.Value);
        }

    }

    public bool GetButtonDown(string buttonName){
        if(buttonKeys.ContainsKey(buttonName) == false){
            Debug.LogError("InputManager :: GetButtonDown -- no button named: " + buttonName);
            return false;
        }
        return Input.GetKeyDown( buttonKeys[buttonName]);
    }

    public bool GetButtonUp(string buttonName){
        if(buttonKeys.ContainsKey(buttonName) == false){
            Debug.LogError("InputManager :: GetButtonDown -- no button named: " + buttonName);
            return false;
        }
        return Input.GetKeyUp(buttonKeys[buttonName]);
    }

    public bool GetButton(string buttonName){
        if(buttonKeys.ContainsKey(buttonName) == false){
            Debug.LogError("InputManager :: GetButtonDown -- no button named: " + buttonName);
            return false;
        }
        return Input.GetKey(buttonKeys[buttonName]);
    }

    public void SetKey(string button, KeyCode key){
        buttonKeys[button] = key;
        PlayerPrefs.SetInt(button, (int) key);
    }

    public Dictionary<string, KeyCode> GetDefault(){

        foreach(KeyValuePair<string, KeyCode> entry in DefaultKeysDict){
            PlayerPrefs.SetInt(entry.Key,(int) entry.Value);
            buttonKeys[entry.Key] = entry.Value;
        }

        return DefaultKeysDict;
    }

    public Dictionary<string, KeyCode> GetKeys(){
        return buttonKeys;
    }
}
