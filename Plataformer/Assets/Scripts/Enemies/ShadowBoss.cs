using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBoss : GroundEnemy
{
    [SerializeField] private GameObject teleportArea = null;
    [SerializeField] private GameObject Tentacle = null;
    [SerializeField] private GameObject TentacleLine = null;
    [SerializeField] private Collider2D Vision = null;
    [SerializeField] private float attkCooldown = 0f;
    [SerializeField] private float lineCooldown = 0f;
    [SerializeField] private float TeleportTime = 0f;

    private Vector3 newPosition;
    private Transform playerPos;
    private bool teleporting = false;
    private bool LineAttkFlag = false;
    private int teleportCont;


    private void Start() {
        playerPos = GameObject.FindWithTag("Player").transform;
        newPosition = transform.position;
        teleportCont = 5;
    }

    private void Update() {
        if(playerPos.position.x - 0.5f > transform.position.x){
            if(direction == 1) Turn();
        }
        else if(playerPos.position.x + 0.5f < transform.position.x){
            if (direction == -1) Turn();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {    
        if(other.gameObject.CompareTag("Player")){
            Vision.enabled = false;
            if(!LineAttkFlag){
                LineAttkFlag = true;
                LineAttk();
                StartCoroutine(LineCooldown());
            }else TentacleAttk(other.gameObject.transform.position);
            StartCoroutine(Cooldown());
            
        }
    }

    private void TentacleAttk(Vector3 pos){
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.down, 20f, groundLayer);
        if (hit.collider != null){
            Vector2 dif  = new Vector2(1f ,0);
            Vector2 correct = new Vector2(0, 1.3f);
            Instantiate(Tentacle, hit.collider.ClosestPoint(pos)+correct, Quaternion.Euler (Vector3.zero));
            Instantiate(Tentacle, hit.collider.ClosestPoint(pos)+dif+correct, Quaternion.Euler (Vector3.zero));
            Instantiate(Tentacle, hit.collider.ClosestPoint(pos)-dif+correct, Quaternion.Euler (Vector3.zero));
            
            newPosition = hit.collider.ClosestPoint(pos)+new Vector2(0 ,1f);
            if( Random.Range(0, teleportCont) == 0){
                teleportCont = 5;
                StopAllCoroutines();
                StartCoroutine(teleportCo());
            }else teleportCont --;
        }
    }

    private IEnumerator LineAttackCo(Vector3 origin, Vector2 Direction){  
        RaycastHit2D hit = Physics2D.Raycast(origin + (Vector3) Direction + (Vector3) Direction, Vector2.down, 2f, groundLayer);
        if (hit.collider != null){
            Vector2 tentaclePos = hit.collider.ClosestPoint(origin) + new Vector2(0, 1.3f) + Direction;
            Instantiate(TentacleLine, tentaclePos , Quaternion.Euler (new Vector3(0,0,0)));     
            yield return new WaitForSeconds(0.25f);
            StartCoroutine(LineAttackCo(tentaclePos , Direction));  
        }
        yield break;
    }

    private IEnumerator LineCooldown(){
        yield return new WaitForSeconds(Random.Range(lineCooldown*0.5f, lineCooldown*1.5f));
        LineAttk();
        StartCoroutine(LineCooldown());
        yield break;
    }
    private void LineAttk(){
        float direct  = 0;
        if(playerPos.position.x < transform.position.x) direct = -1;
        else direct = 1;
        StartCoroutine( LineAttackCo( transform.position, new Vector2(direct,0) ) );
    }

    private IEnumerator Cooldown(){
        yield return new WaitForSeconds(Random.Range(attkCooldown*0.5f, attkCooldown*1.5f));
        Vision.enabled = true;
    }

    protected override void HurtAction(){
        if(!teleporting){
            Vision.enabled = false;
            teleporting = true;
            StopAllCoroutines();
            TentacleAttk(playerPos.position);
            LineAttk();
            StartCoroutine(LineCooldown());

            StartCoroutine(DamagedCo());
            StartCoroutine(teleportCo());
        }
    }
    
    private IEnumerator DamagedCo(){
        yield return null;
        for(int i = 0; i < 10; i ++){
            mySpriteRenderer.color = new Color(1,1,1,0.5f);
            yield return new WaitForSeconds(0.1f);
            mySpriteRenderer.color = new Color(1,1,1,1f);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator teleportCo(){
        yield return null;
        
        anim.SetBool("Dissapear", true);
        yield return new WaitForSeconds(TeleportTime);
        transform.position = newPosition;
        anim.SetBool("Dissapear", false);
        teleporting = false;
        StartCoroutine(Cooldown());
    }

    protected override void DieEffect()
    {
        if(teleportArea != null) teleportArea.SetActive(true);
    }
}
