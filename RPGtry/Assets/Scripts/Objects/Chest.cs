using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Chest : Interactable{

    public Item content;
    public Inventory playerInventory;
    public Signal raiseItem;
    public Signal interactionSignal;
    public Collider2D range;
    public bool isOpened;
    private Animator anim;
    private DialogManager dManager;
    public float openTime;


    void Start()
    {
        dManager = FindObjectOfType<DialogManager>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(inRange && Input.GetButtonUp("Interact")){

            if(!isOpened){
                openChest();
            }
            else{
                ChestAlreadyOpened();
            }

        }
    }
    
    public void openChest(){
        contextClue.Raise();
        anim.SetBool ("Open", true);
        interactionSignal.Raise();
        StartCoroutine(OpeningCo());
        
    }
    public void ChestAlreadyOpened(){
        raiseItem.Raise();
        range.enabled = false;
    }
    private IEnumerator OpeningCo(){
        yield return new WaitForSeconds(openTime);
        playerInventory.AddItem(content);
        playerInventory.currentItem = content;
        raiseItem.Raise();
        interactionSignal.Raise();

        dManager.showBox(content.ItemDescription);
        dManager.dText.text = content.ItemDescription[0];
        dManager.currentLine = 0;
        
        isOpened = true;
    }
     private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")){
            if(!isOpened) contextClue.Raise();
            inRange = false;
        }
    }

}
