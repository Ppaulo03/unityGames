using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombArrow : Arrow
{
    [SerializeField] private GameObject explosion = null;

    private void OnCollisionEnter2D(Collision2D other)
    {
       Explode();
    }

    public void Explode()
    {
        Instantiate(explosion, transform.position, Quaternion.Euler (Vector3.zero));
        Destroy(gameObject);
    }
    
}
