using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidPool : MonoBehaviour
{
    [SerializeField] private float lifeTime = 0f;
    [SerializeField] private FloatValue numPools = null;
    [SerializeField] private Collider2D Mycollidder = null;
    [SerializeField] private float colldownTime = 0f;

    void Start()
    {
        if(numPools != null){
            StartCoroutine(destroyCo());
            numPools.Value ++;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player")){
            other.gameObject.GetComponent<PlayerController>().Hurt(Vector3.zero);
            Mycollidder.enabled = false;
            StartCoroutine(cooldownCo());
        }
    }

    private IEnumerator cooldownCo(){
        yield return new WaitForSeconds(colldownTime);
        Mycollidder.enabled = true;
    }

    private IEnumerator destroyCo(){
        yield return new WaitForSeconds(lifeTime);
        numPools.Value --;
        StopCoroutine(cooldownCo());
        Destroy(gameObject);
    }

}
