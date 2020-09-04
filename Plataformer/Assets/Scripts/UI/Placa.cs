using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Placa : MonoBehaviour{

    [SerializeField] private InputManager inputManager = null;
    [SerializeField] private GameObject clue = null;
    [SerializeField] private GameObject dialogBox = null;
    [SerializeField] private TMPro.TMP_Text textBox = null;
    [SerializeField] private string[] message = null;
    [SerializeField] private bool StartAwake = false;
    private bool inRange = false;
    private bool active = false;
    private int TextIndex = 0;

    public void Submmited() {
        if(inRange){
            if(!active){
                active = true;
                textBox.text = checkSpecial(message[0]);
                clue.SetActive(false);
                dialogBox.SetActive(true);
            }

            else{
                TextIndex ++;
                if(TextIndex >= message.Length){
                    TextIndex = 0;
                    active = false;
                    dialogBox.SetActive(false);
                    clue.SetActive(true);
                }
                textBox.text = checkSpecial(message[TextIndex]);
            }
        }
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

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){
            if(message != null){        
                
                inRange = true;
                if(StartAwake){
                    active = true;
                    textBox.text = checkSpecial(message[0]);
                    dialogBox.SetActive(true);
                    clue.SetActive(false);

                }
                else{
                    active = false;
                    textBox.text = checkSpecial(message[0]);
                    dialogBox.SetActive(false);
                    clue.SetActive(true);
                }
            }   
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){
            inRange = false;
            active = false;
            TextIndex = 0;
            dialogBox.SetActive(false);
            clue.SetActive(false);
        }
    }
    
    public void UptdateMessage(){
         textBox.text = checkSpecial(message[TextIndex]);
    }


}
