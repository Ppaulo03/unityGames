using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{

    public Slider healthBar;
    public Text HPText;
    public Text LvlText;
    public StatusValues playerStats;
    public FloatValue currentHealth;
    private static bool UIExists;

    void Start()
    {
        if(!UIExists){
            UIExists = true;
            DontDestroyOnLoad(transform.gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

    public void UpdateUI(){
        healthBar.maxValue = playerStats.RuntimeMaxHealth;
        healthBar.value = currentHealth.runtimeValue;
        HPText.text = "HP: " + currentHealth.runtimeValue + "/" + playerStats.RuntimeMaxHealth;
        LvlText.text = "Lvl: " + playerStats.RuntimeLevel;
    }

}
