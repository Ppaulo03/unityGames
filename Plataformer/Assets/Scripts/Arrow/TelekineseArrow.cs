using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelekineseArrow : Arrow
{
    public float arrowForce;
    private bool controlling = true;
    private bool change = true;
    public float burstTime;

    private void FixedUpdate() {
        
        if(controlling) Control();
        DirectArrow();

    }

    private void Control(){
        if(change){
            change = false;  
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = (new Vector2(mousePos.x,mousePos.y) - new Vector2(transform.position.x, transform.position.y));

            myRigidbody.velocity = Vector2.zero;
            myRigidbody.AddForce(dir.normalized*arrowForce, ForceMode2D.Impulse);
            StartCoroutine(ChangeCo());
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Enemy")){
            Destroy(gameObject);
            other.gameObject.GetComponent<Enemy>().Hurt(myRigidbody.velocity.normalized * knockBackForce);
        }
        else{
            myRigidbody.velocity = Vector2.zero;
            myRigidbody.bodyType = RigidbodyType2D.Static;
            if(arrowCollider != null) arrowCollider.enabled = false;
            controlling = false;
        }
    }

    private IEnumerator ChangeCo(){
        yield return new WaitForSeconds(burstTime);
        change = true;
    }
}
