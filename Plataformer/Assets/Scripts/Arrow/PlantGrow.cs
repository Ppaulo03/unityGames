using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlantGrow : MonoBehaviour
{
    public float LifeTime;
    public float DieAnimationTime;

    [Header("MudaState")]
    public Signal killExtras;
    public Range numMudas;
    public Range growOrder;
    private float currentMuda;
    public float mudaLifeTime;
    private bool grew;
    
    public bool isSideway;
    public float sideCorrection;

    [Header("Components")]
    private Animator anim;

    private void Awake() {
        anim = GetComponent<Animator>();
        currentMuda = numMudas.currentValue;
        numMudas.Add(1f);
        killExtras.Raise();
        StartCoroutine(DissapearCo());
    }

    public void Grow(){
        if(!grew && growOrder.currentValue == currentMuda){
            growOrder.Add(1f);
            transform.localScale = new Vector3(transform.localScale.x * sideCorrection, transform.localScale.y, transform.localScale.z);
            anim.SetBool("Grow",true);
            anim.SetBool("SideWay",isSideway);
            grew = true;
            StopCoroutine(DissapearCo());
            StartCoroutine(dieCo());
        }
    }

    public void Die(){
        if(currentMuda == numMudas.currentValue) 
            Destroy(gameObject);
    }
    private IEnumerator dieCo(){
        yield return new WaitForSeconds(LifeTime);
        anim.SetBool("Die",true);
        yield return new WaitForSeconds(DieAnimationTime);
        Destroy(gameObject);
    }
    private IEnumerator DissapearCo(){
        yield return new WaitForSeconds(mudaLifeTime);
        if(!grew){
            growOrder.Add(1f);
            Destroy(gameObject);
        }
    }

}
