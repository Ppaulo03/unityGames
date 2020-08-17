using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState{
    idle,
    walk,
    sttager,
    attack,
    chase,
    interact
}
public abstract class Enemy : MonoBehaviour
{
    public EnemyState state;
    public StatusValues status;
    private int currentHealth;
    public string enemyName;
    private Vector2 homePositon;

    public LootTable thisLoot;

    public float waitTime;
    public bool haveVision;
    

    private void Awake() {
        maximizeHealth();
        homePositon = transform.position;
    }

    private void OnEnable() {
        transform.position = homePositon;
        maximizeHealth();
        state = EnemyState.idle;
    }

    public abstract void Knock(Transform player, float thrust, float knockTime);
    public abstract void changeState(EnemyState newState);

    public abstract void die();
    public int TakeDamage(int damageToGive){
        float damageReceived = Mathf.Ceil(damageToGive*(100f/ (100f + status.RuntimeDefense)));
        currentHealth -= (int)damageToGive;
        if(currentHealth <= 0){
            die();
            MakeLoot();
        }
        return (int)damageToGive;
    }

    private void MakeLoot(){
        if(thisLoot != null){
            Collectable current = thisLoot.LootItem();
            if(current != null){
                Instantiate(current.gameObject, transform.position, Quaternion.identity);
            }
        }
    }

    public void maximizeHealth(){
        currentHealth = (int) status.RuntimeMaxHealth;
    }
    public void inRange(bool vision){
        if(vision){
            haveVision = true;
            changeState(EnemyState.chase);
        }else{
            haveVision = false;
            changeState(EnemyState.idle);
        }
    }
}


