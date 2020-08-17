using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObject/Item", order = 0)]
public class Item : ScriptableObject {
    public Sprite itemSprite;
    public string[] ItemDescription;
    public bool isKey;
}