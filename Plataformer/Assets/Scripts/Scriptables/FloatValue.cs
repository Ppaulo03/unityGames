using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Signal", menuName = "ScriptableObject/FloatValue", order = 0)]
public class FloatValue : ScriptableObject, ISerializationCallbackReceiver
{
    
    [SerializeField] private float initialValue = 0;
    [System.NonSerialized] public float Value;

    public void OnBeforeSerialize(){}
    public void OnAfterDeserialize()
    {
        Value = initialValue;
    }
}
