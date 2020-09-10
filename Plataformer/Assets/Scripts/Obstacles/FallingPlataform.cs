using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlataform : MonoBehaviour
{
    [SerializeField] private float fallTime = 0f, respawnTime = 0f, shackEffect = 0f;
    [SerializeField] private int shackTimes = 0;

    [SerializeField] private BoxCollider2D myCollider2D = null;
    private Rigidbody2D myRigidbody;
    private Vector3 StartPos;
    
    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        StartPos = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player")) StartCoroutine(FallCo());
    }

    private IEnumerator FallCo()
    {
        for(int i = 0; i < shackTimes; i ++){
            shackEffect = -shackEffect;
            for(int j = 0; j < 2; j ++){
                transform.position = new Vector3(transform.position.x + shackEffect, transform.position.y - shackEffect, transform.position.z);
                yield return new WaitForSeconds(fallTime/(shackTimes*2));
            }
        }

        myRigidbody.bodyType = RigidbodyType2D.Dynamic;
        myCollider2D.enabled = false;
        StartCoroutine(RespawnCo());
        
    }

    private IEnumerator RespawnCo()
    {
           
        yield return new WaitForSeconds(respawnTime);
        transform.position = StartPos;
        myRigidbody.bodyType = RigidbodyType2D.Static;
        myCollider2D.enabled = true;
        
    }

}
