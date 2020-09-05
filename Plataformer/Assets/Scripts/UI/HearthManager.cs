using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HearthManager : MonoBehaviour
{
    [SerializeField] private Image[] hearths = null;
    [SerializeField] private Sprite fullHearth = null;
    [SerializeField] private Sprite emptyHearth = null;
    [SerializeField] private FloatValue HearthContainers = null;
    [SerializeField] private FloatValue Health = null;
    
    void Start()
    {
        InitiHearths();
    }

    private void InitiHearths()
    {
        for(int i = 0; i < hearths.Length; i ++){
            if(i < HearthContainers.Value) hearths[i].gameObject.SetActive(true);
            else hearths[i].gameObject.SetActive(false);
            hearths[i].sprite = fullHearth;
        }
    }

    public void UpdateHearths()
    {
        for(int i = 0; i < HearthContainers.Value; i ++){
            if(i< Health.Value){
                hearths[i].sprite = fullHearth;
            }else{
                hearths[i].sprite = emptyHearth;
            }
        }
    }

}
