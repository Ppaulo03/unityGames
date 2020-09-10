using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowTentacle : GroundEnemy
{
    [Header("Hurt Settings")]
    [SerializeField] private float dieTime = 0f;
    [SerializeField] private float lifeTime = 0f;
    [SerializeField] private float respawnTime = 0f;

    private void Start() {
        if(lifeTime>0)
            StartCoroutine(TimeUp());
    }
    protected override void Die()
    {
        StartCoroutine(DieCo());
    }    
    private IEnumerator DieCo()
    {
        anim.SetTrigger("Die");
        yield return new WaitForSeconds(dieTime);
        if(respawnTime > 0) StartCoroutine(respawnCo());
        else Destroy(gameObject);
    }
    private IEnumerator TimeUp()
    {
        yield return new WaitForSeconds(lifeTime);
        Die();
    }
    private IEnumerator respawnCo(){
        yield return new WaitForSeconds(respawnTime);
        anim.SetTrigger("Respawn");
    }

}
