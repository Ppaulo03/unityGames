using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public GameObject dBox;
    public Text dText;
    public Signal interactionSignal;

    public bool dialogueActive;

    public string[] dialogueLines;

    public int currentLine = -1;



    void LateUpdate()
    {
        if(dialogueActive && Input.GetButtonUp("Interact")){
            
            currentLine++;
            if(currentLine >= dialogueLines.Length){
                dBox.SetActive(false);
                dialogueActive = false;
                currentLine = -1;
                interactionSignal.Raise();
            }
            else{
                dText.text = dialogueLines[currentLine];
            }
        
        }
    }

    public void showBox(string[] dialog){
        if(!dialogueActive){
            dialogueActive = true;
            dialogueLines = dialog;
            dBox.SetActive(true);
            currentLine = -1;
            interactionSignal.Raise();
        } 
    }
    

}
