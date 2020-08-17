using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Loot{
    public Collectable thisLoot;
    public float lootChance;
}

[CreateAssetMenu(fileName = "LootTable", menuName = "ScriptableObject/LootTable", order = 0)]
public class LootTable : ScriptableObject {
    public Loot[] loots;

    public Collectable LootItem(){

        float cumProb = 0;
        float currentProb = Random.Range(0,100);
        for(int i = 0; i < loots.Length; i ++){
            cumProb += loots[i].lootChance;
            if(currentProb <= cumProb){
                return loots[i].thisLoot;
            }
        }
        return null;
    }

}
