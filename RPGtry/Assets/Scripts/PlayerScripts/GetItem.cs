using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItem : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer receivedItem;
    public Inventory inventory;

    void Start()
    {
        anim = transform.parent.GetComponent<Animator>();
        receivedItem = GetComponent<SpriteRenderer>();
    }

    public void RaiseItem(){
        if(inventory.currentItem!= null){
            receivedItem.sprite = inventory.currentItem.itemSprite;  
        }
        anim.SetBool("Receive Item", !anim.GetBool("Receive Item"));
        receivedItem.enabled  = ! receivedItem.enabled;
    }

}
