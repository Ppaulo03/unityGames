using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{

    public Enemy enemy;
    public Collider2D lookArea;
    public Collider2D chaseArea;

    private void Start() {
        lookArea.enabled = true;
        chaseArea.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {

        if(other.CompareTag("Player")){
            enemy.inRange(true);
            lookArea.enabled = false;
            chaseArea.enabled = true;
        }

    }

    private void OnTriggerExit2D(Collider2D other) {

        if(other.CompareTag("Player")){
            enemy.inRange(false);
            lookArea.enabled = true;
            chaseArea.enabled = false;
        }

    }
}
