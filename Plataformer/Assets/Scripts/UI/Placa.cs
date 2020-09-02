using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Placa : MonoBehaviour{
    [SerializeField] private GameObject clue = null;
    [SerializeField] private GameObject dialogBox = null;
    [SerializeField] private Text textBox = null;
    [SerializeField] private string[] message = null;
    [SerializeField] private bool StartAwake = false;
    private bool inRange = false;
    private bool active = false;
    private int TextIndex = 0;

    private void Update() {
        if(inRange){
            if(Input.GetButtonDown("Submit")  && Time.timeScale != 0){

                if(!active){
                    active = true;
                    textBox.text = message[0];
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
                    textBox.text = message[TextIndex];
                }

            }

        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){
            if(message != null){
                inRange = true;
                
                if(StartAwake){
                    active = true;
                    textBox.text = message[0];
                    dialogBox.SetActive(true);
                    clue.SetActive(false);
                }
                else{
                    active = false;
                    textBox.text = message[0];
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
    


}
