using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum DoorType{
    key,
    button,
    enemy
}

public class Door : Interactable{

    [Header("Door Variables")]
    public DoorType doorType;
    public bool open;
    public Inventory inventory;
    public GameObject roomPassage;

    private SpriteRenderer sprite;
    private BoxCollider2D collid;
    
    private void Start() {
        sprite = GetComponent<SpriteRenderer>();
        collid = GetComponent<BoxCollider2D>();
    }
    private void Update() {
        if(Input.GetButtonUp("Interact")){
            if(inRange && doorType == DoorType.key){
                if(inventory.numberOfKeys > 0){
                    inventory.numberOfKeys --;
                    Open();
                }
            }
        }
    }

    public void Open(){
        sprite.enabled = false;
        collid.enabled = false;
        roomPassage.SetActive(true);
        open = true;
    }
    public void Close(){

    }


}
