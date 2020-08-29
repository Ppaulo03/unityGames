using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowManager : MonoBehaviour
{
    [SerializeField] private IntValue chosenArrow = null;
    [SerializeField] private Sprite[] arrowsSprite = null;

    [SerializeField] private Image chosenArrowImage = null;

    private void Start() {
        chosenArrowImage.sprite = arrowsSprite[chosenArrow.Value];
    }

    public void ChangeArrow(){
        chosenArrowImage.sprite = arrowsSprite[chosenArrow.Value];
    }


}
