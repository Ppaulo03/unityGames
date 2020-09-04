using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{   
    [SerializeField] private string damagetag = "";
    [SerializeField] private LayerMask groundLayer = ~0;
    [SerializeField] private float Radius = 0f;

    private void OnTriggerEnter2D(Collider2D other) {
        if(!other.isTrigger){
            if(other.gameObject.CompareTag(damagetag)){
                Vector2 Direction = other.ClosestPoint(transform.position) - new Vector2(transform.position.x, transform.position.y);
                RaycastHit2D block = Physics2D.Raycast(transform.position, Direction.normalized, Radius,  groundLayer);
                if(block.collider == null){
                    if(damagetag == "Player") other.gameObject.GetComponent<PlayerController>().Hurt(Vector3.zero);
                    else other.gameObject.GetComponent<Enemy>().Hurt(Vector3.zero);
                }
                
            }
        }
    }
}
