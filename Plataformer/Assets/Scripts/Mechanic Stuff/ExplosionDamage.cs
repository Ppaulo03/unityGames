using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{   
    [SerializeField] private string damagetag = "";
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag(damagetag)){
            if(damagetag == "Player") other.gameObject.GetComponent<PlayerController>().Hurt(Vector3.zero);
            else other.gameObject.GetComponent<Enemy>().Hurt(Vector3.zero);
        }
    }
}
