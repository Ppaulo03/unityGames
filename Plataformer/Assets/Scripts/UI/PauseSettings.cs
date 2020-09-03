using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSettings : MonoBehaviour
{   [SerializeField] private GameObject[] disables = null;
    [SerializeField] private GameObject[] enables = null;
    private void OnDisable() {
        foreach( GameObject i in disables) i.SetActive(false);
        foreach( GameObject i in enables) i.SetActive(true);
    }
}
