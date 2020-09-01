using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinBomb : MonoBehaviour
{
    [SerializeField] private float explosionTime = 0f;
    [SerializeField] private GameObject explosion = null;
    void Start()
    {
        StartCoroutine(ExplosionCo());
    }  

    private IEnumerator ExplosionCo(){
        yield return new WaitForSeconds(explosionTime);
        Instantiate(explosion, transform.position, Quaternion.Euler (Vector3.zero));
        Destroy(gameObject);

    }

}
