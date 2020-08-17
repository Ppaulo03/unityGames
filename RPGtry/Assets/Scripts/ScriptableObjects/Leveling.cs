using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Leveling", menuName = "ScriptableObject/Leveling", order = 0)]
public class Leveling : ScriptableObject {
    
    public int[] toLevel;
    public int[] maxHealth;
    public int[] attack;
    public int[] defense;


}
