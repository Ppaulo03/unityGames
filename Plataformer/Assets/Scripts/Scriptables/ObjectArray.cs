using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Signal", menuName = "ScriptableObject/ObjectArray", order = 0)]
public class ObjectArray : ScriptableObject, ISerializationCallbackReceiver
{
    
    [SerializeField] private GameObject[] initialObjects = null;
    
    [System.NonSerialized] public GameObject[] currentObjects;

    public void OnBeforeSerialize(){}
    public void OnAfterDeserialize()
    {
        currentObjects = new GameObject[initialObjects.Length];
        for(int i = 0; i < initialObjects.Length; i++){
            currentObjects[i] = initialObjects[i];
        }
    }

}
