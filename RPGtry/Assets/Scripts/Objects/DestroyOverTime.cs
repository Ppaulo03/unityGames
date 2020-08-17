using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{

    public float timeToDestroy;

    void Update()
    {
        timeToDestroy -= Time.deltaTime;
        if(timeToDestroy <= 0){
            Destroy(gameObject);
        }
    }
    
}
