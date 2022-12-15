using System;
using System.Collections;
using System.Collections.Generic;

using Evan.Scripts.PlayerMovement;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    public bool playerOnlyTrigger = false;
    private GameObject player;
    
    public UnityEvent triggerEvent;
    public UnityEvent leaveTriggerEvent;

    

    private void Start() {
        player = FindObjectOfType<FPSPlayerInput>().gameObject;
    }


    private void OnTriggerEnter(Collider other) {
        if (playerOnlyTrigger) {
            if (other.gameObject == player) {
                triggerEvent.Invoke();
            }
        } else {
            triggerEvent.Invoke();
        }
        
    }

    private void OnTriggerExit(Collider other) {
        if (playerOnlyTrigger) {
            if (other.gameObject == player) {
                leaveTriggerEvent.Invoke();
            }
        } else {
            leaveTriggerEvent.Invoke();
        }
        
    }
}
