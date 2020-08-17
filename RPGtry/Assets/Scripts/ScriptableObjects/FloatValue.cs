using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FloatValue", menuName = "ScriptableObject/FloatValue", order = 0)]
public class FloatValue : ScriptableObject, ISerializationCallbackReceiver  {
    
    [SerializeField]
    private float initialValue;

    [System.NonSerialized]
    public float runtimeValue;

    public void OnAfterDeserialize(){
        runtimeValue = initialValue;
    }
    public void OnBeforeSerialize(){}
}