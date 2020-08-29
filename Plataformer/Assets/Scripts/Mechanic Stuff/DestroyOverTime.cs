using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    [SerializeField] private float lifeTime = 0f;
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
