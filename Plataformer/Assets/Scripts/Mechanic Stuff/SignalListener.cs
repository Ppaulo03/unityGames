using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SignalListener : MonoBehaviour
{
    [SerializeField] private Signal signal = null;
    [SerializeField] private UnityEvent signalEvent = null;

    public void onSignalRaised(){
        signalEvent.Invoke();
    }

    private void OnEnable() {
        signal.RegisterListener(this);
    }
    
    private void OnDisable() {
        signal.DeRegisterListener(this);
    }

}
