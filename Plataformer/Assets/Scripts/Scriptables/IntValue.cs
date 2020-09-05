using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Signal", menuName = "ScriptableObject/IntValue", order = 0)]
public class IntValue : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] private int initialValue = 0;
    [System.NonSerialized] public int Value;

    public void OnBeforeSerialize(){}
    public void OnAfterDeserialize()
    {
        Value = initialValue;
    }
}
