using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusValues", menuName = "ScriptableObject/StatusValues", order = 0)]
public class StatusValues : ScriptableObject, ISerializationCallbackReceiver{


    [SerializeField]
    private float initialMaxHealth, initialMoveSpeed, initialThrust;

    [SerializeField]
    private int initialAttack, initialDefense, initialExperience, initialLevel;
   
    [System.NonSerialized]
    public float RuntimeMaxHealth, RuntimeMoveSpeed, RuntimeThrust;

    [System.NonSerialized]
    public int RuntimeAttack, RuntimeDefense, RuntimeExperience, RuntimeLevel;
    public void OnAfterDeserialize(){
        RuntimeMaxHealth = initialMaxHealth;
        RuntimeMoveSpeed = initialMoveSpeed;
        RuntimeThrust = initialThrust;
        RuntimeAttack = initialAttack;
        RuntimeDefense = initialDefense;
        RuntimeExperience = initialExperience;
        RuntimeLevel = initialLevel;
    }  
    public void OnBeforeSerialize(){}

}