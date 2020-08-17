using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextClue : MonoBehaviour
{
    public SpriteRenderer contextClue;
    public void ContextManager() {
        contextClue.enabled = !contextClue.enabled;
    }

}
