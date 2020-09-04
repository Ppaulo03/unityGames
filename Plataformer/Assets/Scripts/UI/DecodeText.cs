using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecodeText : MonoBehaviour
{   
    [SerializeField] private InputManager inputManager = null;
    [SerializeField] private TMPro.TMP_Text textBox = null;
    private string originalMessage;
    private void OnEnable() {
        originalMessage = textBox.text;
        UptdateMessage();
    }

    private void OnDisable() {
        textBox.text = originalMessage ;
    }

    private string checkSpecial(string message){
        if(message.Contains("$")){
            
            string newString = "";
            string[] cutString = message.Split(' ');
            for(int i = 0; i < cutString.Length; i ++){
                if(cutString[i].Contains("$")){
                    cutString[i] = inputManager.GetKeyName(cutString[i].Substring(1));
                }
                newString += " " + cutString[i];
            }
            return newString;

        }else return message;
    }

    public void UptdateMessage(){
        textBox.text = checkSpecial(originalMessage);
    }

}
