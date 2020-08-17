using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour{
    protected bool inRange;
    public Signal contextClue;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            contextClue.Raise();
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")){
            contextClue.Raise();
            inRange = false;
        }
    }
    
}
