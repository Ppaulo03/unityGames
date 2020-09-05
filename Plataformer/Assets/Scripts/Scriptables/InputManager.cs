using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Signal", menuName = "ScriptableObject/InputManager", order = 0)]
public class InputManager : ScriptableObject
{

    [SerializeField] private Signal Rebind = null;
    [System.Serializable] public struct KeyButton
    {
        public string name;
        public KeyCode key;
    }
    [SerializeField] private KeyButton[] DefaultKeys = null;
    private Dictionary<string, KeyCode> DefaultKeysDict;
    private Dictionary<string, KeyCode> buttonKeys;

    private void OnEnable()
    {
        DefaultKeysDict = new Dictionary<string, KeyCode>();
        foreach(KeyButton button in DefaultKeys)DefaultKeysDict[button.name] = button.key;
        
        buttonKeys = new Dictionary<string, KeyCode>();

        foreach(KeyValuePair<string, KeyCode> entry in DefaultKeysDict){
            buttonKeys[entry.Key] = (KeyCode) PlayerPrefs.GetInt(entry.Key, (int) entry.Value);
        }

    }

    public bool GetButtonDown(string buttonName)
    {
        if(buttonKeys.ContainsKey(buttonName) == false){
            Debug.LogError("InputManager :: GetButtonDown -- no button named: " + buttonName);
            return false;
        }
        return Input.GetKeyDown( buttonKeys[buttonName]);
    }

    public bool GetButtonUp(string buttonName)
    {
        if(buttonKeys.ContainsKey(buttonName) == false){
            Debug.LogError("InputManager :: GetButtonDown -- no button named: " + buttonName);
            return false;
        }
        return Input.GetKeyUp(buttonKeys[buttonName]);
    }

    public bool GetButton(string buttonName)
    {
        if(buttonKeys.ContainsKey(buttonName) == false){
            Debug.LogError("InputManager :: GetButtonDown -- no button named: " + buttonName);
            return false;
        }
        return Input.GetKey(buttonKeys[buttonName]);
    }

    public void SetKey(string button, KeyCode key)
    {
        buttonKeys[button] = key;
        PlayerPrefs.SetInt(button, (int) key);
        Rebind.Raise();
    }

    public Dictionary<string, KeyCode> GetDefault()
    {

        foreach(KeyValuePair<string, KeyCode> entry in DefaultKeysDict){
            PlayerPrefs.SetInt(entry.Key,(int) entry.Value);
            buttonKeys[entry.Key] = entry.Value;
        }
        Rebind.Raise();
        return DefaultKeysDict;
    }

    public Dictionary<string, KeyCode> GetKeys()
    {
        return buttonKeys;
    }

    public string GetKeyName(string Button)
    {
        if(buttonKeys.ContainsKey(Button) == false){
            Debug.LogError("InputManager :: GetButtonDown -- no button named: " + Button);
            return "";
        }
        string KeyName = buttonKeys[Button].ToString();
        switch(KeyName){
            case "Mouse0":
                return "Right Mouse Button";
            case "Mouse1":
                return "Left Mouse Button";
            case "Return":
                return "Enter";
            case "Escape":
                return "Esc";
        }
        return KeyName;
        
    }
}
