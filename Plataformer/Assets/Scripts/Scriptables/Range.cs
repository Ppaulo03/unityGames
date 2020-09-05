using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Signal", menuName = "ScriptableObject/Range", order = 0)]
public class Range : ScriptableObject, ISerializationCallbackReceiver
{
    
    [SerializeField] private float maxValue = 0, minValue = 0, initialValue = 0;
    //[System.NonSerialized] 
    public float currentValue;

    public void OnBeforeSerialize(){}
    public void OnAfterDeserialize()
    {
        currentValue = initialValue;
    }
    
    public void Add(float value)
    {
        currentValue += value;
        if(currentValue > maxValue){
            currentValue = minValue;
        }
    }

}
