using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeExplosion : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(!other.isTrigger){
            if(other.gameObject.CompareTag("Enemy")){
                other.gameObject.GetComponent<Enemy>().Freeze();
            }
        }
    }
}
