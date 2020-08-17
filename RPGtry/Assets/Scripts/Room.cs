using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject Virtcamera;
    public Enemy[] enemies;
    public ObjectBreak[] breakables;

    public virtual void OnTriggerEnter2D(Collider2D other) {
        if( other.CompareTag("Player")){
                Virtcamera.SetActive(true);
                for( int i = 0; i < enemies.Length; i++){
                    ChangeActivation(enemies[i], true);
                }
                for( int i = 0; i < breakables.Length; i++){
                    ChangeActivation(breakables[i], true);
                }
                
        }
    }

    public virtual void OnTriggerExit2D(Collider2D other) {
        if( other.CompareTag("Player")){
                Virtcamera.SetActive(false);
                for( int i = 0; i < enemies.Length; i++){
                    ChangeActivation(enemies[i], false);
                }
                for( int i = 0; i < breakables.Length; i++){
                    ChangeActivation(breakables[i], false);
                }
        }
    }

    void ChangeActivation(Component component, bool activation){
        if(component != null) component.gameObject.SetActive(activation);
    }

}
