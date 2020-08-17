using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBreak : MonoBehaviour
{

    private Animator anim;
    public float timeToVanish;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Break(){
        anim.SetBool("Break",true);
        StartCoroutine(BreakeTime());
    }
    IEnumerator  BreakeTime(){
        yield return new WaitForSeconds(timeToVanish);
        this.gameObject.SetActive(false);
    }
}
