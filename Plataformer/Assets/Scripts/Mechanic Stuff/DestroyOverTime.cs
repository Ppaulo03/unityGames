using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    public float lifeTime;
    void Start()
    {
        StartCoroutine(destroyCo());
    }

    private IEnumerator destroyCo()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

}
