using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Signal", menuName = "ScriptableObject/ArrowManagement", order = 0)]
public class ArrowManagement : ScriptableObject, ISerializationCallbackReceiver
{

    [System.Serializable] public struct ArrowStruct {
        public string name;
        public GameObject arrowObject;
        public Sprite arrowsSprite;
        public Signal effect;
        public int maxOfArrows;
        public int currentQtd;
        public float CooldownTime;
    }

    public ArrowStruct[] arrows = null;

    public void OnBeforeSerialize(){}
    public void OnAfterDeserialize(){
        for(int i = 0; i < arrows.Length; i ++){
            arrows[i].currentQtd = arrows[i].maxOfArrows;
        }
    }
    
    public void ActiveEffect(int index){
        if(arrows[index].effect != null)
            arrows[index].effect.Raise();
    }

}
