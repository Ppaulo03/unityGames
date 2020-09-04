using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowManager : MonoBehaviour
{
    [SerializeField] private IntValue chosenArrow = null;
    [SerializeField] private ArrowManagement arrowManagement = null;

    [SerializeField] private Image chosenArrowImage = null;
    [SerializeField] private TMPro.TMP_Text arrowQtd = null;

    [SerializeField] private GameObject Countdown = null;
    [SerializeField] private TMPro.TMP_Text CountdownText = null;

    private int[] countDowns = {0, 0, 0, 0, 0, 0};

    private void Start() {
        ChangeArrow();
    }

    private void Update() {
        CountdownText.text = countDowns[chosenArrow.Value].ToString();
    }

    public void ChangeArrow(){
        chosenArrowImage.sprite = arrowManagement.arrows[chosenArrow.Value].arrowsSprite;
        arrowQtd.text = "x" + arrowManagement.arrows[chosenArrow.Value].currentQtd.ToString();
        if(arrowManagement.arrows[chosenArrow.Value].currentQtd == 0){
            Countdown.SetActive(true);     
        }
        else Countdown.SetActive(false);
    }

    public void ShootArrow(){
        
        arrowQtd.text = "x" + arrowManagement.arrows[chosenArrow.Value].currentQtd.ToString();
        if(arrowManagement.arrows[chosenArrow.Value].currentQtd == 0){
            StartCoroutine(countdownCo(chosenArrow.Value));
            Countdown.SetActive(true);
        }else Countdown.SetActive(false);
        
    }

    private IEnumerator countdownCo(int index){
        for(int i = Mathf.RoundToInt(arrowManagement.arrows[chosenArrow.Value].CooldownTime); i > 0 ; i--){
            countDowns[index] = i;
            yield return new WaitForSeconds(1f);
        }
        ShootArrow();
    }



}
