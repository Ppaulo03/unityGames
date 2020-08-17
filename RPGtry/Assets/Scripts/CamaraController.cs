using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraController : MonoBehaviour
{

    public Vector2 maxPosition;
    public Vector2 minPosition;
    public Transform target;
    public float moveSpeed;
    private static bool cameraExists;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        if(!cameraExists){
            cameraExists = true;
            DontDestroyOnLoad(transform.gameObject);
        }else{
            Destroy(gameObject);
        }
    }

    void LateUpdate()
    {
        if( transform.position != target.position ){
            Vector3 targetPos = new Vector3(target.position.x, target.position.y,  transform.position.z);

            targetPos.x = Mathf.Clamp(targetPos.x, minPosition.x, maxPosition.x);
            targetPos.y = Mathf.Clamp(targetPos.y, minPosition.y, maxPosition.y);
            transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed);
        }
    }

    public void BeginKick(){
        anim.SetBool("KickActive",true);
        StartCoroutine(KickCo());
    }
    private IEnumerator KickCo(){
        yield return null;
        anim.SetBool("KickActive",false);
    }
}
