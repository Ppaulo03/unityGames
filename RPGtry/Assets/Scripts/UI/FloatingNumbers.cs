using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingNumbers : MonoBehaviour
{

    public float moveSpeed;
    public int damageNumber;
    public Text displayNumber;

    void Update()
    {
        displayNumber.text = damageNumber.ToString();
        transform.position = new Vector3(transform.position.x, transform.position.y + (moveSpeed*Time.deltaTime), transform.position.z);
    }
}
