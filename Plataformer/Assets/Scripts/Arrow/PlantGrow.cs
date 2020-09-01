using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlantGrow : MonoBehaviour
{
    [SerializeField] private float LifeTime = 0f;
    [SerializeField] private float DieAnimationTime = 0f;

    [Header("MudaState")]
    [SerializeField] private Signal killExtras = null;
    [SerializeField] private Range numMudas = null;
    [SerializeField] private Range growOrder = null;
    [SerializeField] private float mudaLifeTime = 0f;
    private float currentMuda;    
    private bool grew;
    [System.NonSerialized] public bool isSideway;
    [System.NonSerialized] public float sideCorrection;

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

    public void Kill(){
        StopCoroutine(dieCo());
        if(!grew){
            anim.SetBool("Die",true);
            StartCoroutine(KillCo());
        }else Destroy(gameObject);
    }   
    
    private IEnumerator KillCo(){
        yield return new WaitForSeconds(DieAnimationTime);
        Destroy(gameObject);
    }

    private IEnumerator dieCo(){
        yield return new WaitForSeconds(LifeTime);
        Kill();
    }
    private IEnumerator DissapearCo(){
        yield return new WaitForSeconds(mudaLifeTime);
        if(!grew){
            growOrder.Add(1f);
            Destroy(gameObject);
        }
    }

}
