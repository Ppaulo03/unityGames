using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogHolder : Interactable{

    public string[] dialog;
    private DialogManager dManager;
    private bool isVilager;
    private VillageMove villageMove;

    void Start()
    {
        dManager = FindObjectOfType<DialogManager>();
        try
        {
            villageMove = transform.parent.GetComponent<VillageMove>();
        }
        catch (System.Exception)
        {    
            isVilager = false;
        }
        
        if(villageMove != null) isVilager = true;
    }

    private void Update() {
        if(inRange && Input.GetButtonUp("Interact")){
                dManager.showBox(dialog);
                if(isVilager){
                    villageMove.canMove = false;
                }
            }
    }


}
