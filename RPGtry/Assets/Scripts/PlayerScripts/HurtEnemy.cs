using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtEnemy : MonoBehaviour
{

    public int damageToGive;
    public GameObject damageBurst;
    public GameObject damageNumber;
    public Transform hitPoint;
    public StatusValues playerStats;
    public float knockTime;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Enemy")){
            int currentDamage = damageToGive + playerStats.RuntimeAttack;
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            int damageGiven = enemy.TakeDamage(currentDamage);
            
            Instantiate(damageBurst, hitPoint.position, hitPoint.rotation);
            GameObject clone = Instantiate(damageNumber, hitPoint.position, Quaternion.Euler ( Vector3.zero ) );
            clone.GetComponent<FloatingNumbers>().damageNumber = damageGiven;

            enemy.Knock(transform.parent.transform, playerStats.RuntimeThrust, knockTime);
            
        }
        else if(other.gameObject.tag == "Breakable"){
            other.GetComponent<ObjectBreak>().Break();
        }
    }



}
