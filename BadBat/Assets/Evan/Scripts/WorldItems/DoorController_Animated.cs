using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorController_Animated : MonoBehaviour
{
    private enum  DoorState
    {
        Open, 
        Closed
    }
    [SerializeField] private DoorState doorState;

    private Animator animController;
    [SerializeField] private float doorOpenDelay = 0;

    public UnityEvent extraOpenEvent;
    public UnityEvent extraCloseEvent;
    
    
    private void Awake() {
        animController = GetComponent<Animator>();
        if (doorState == DoorState.Open) {
            OpenDoor();
        } else {
            CloseDoor();
        }
    }

    
    public void CloseDoor() {
        StartCoroutine(CloseDoorDelay());
    }

    private IEnumerator CloseDoorDelay() {
        yield return new WaitForSeconds(doorOpenDelay);
        doorState = DoorState.Closed;
        animController.SetBool("Closed", true);
        extraCloseEvent.Invoke();
    }
    
    
    public void OpenDoor() {
        StartCoroutine(OpenDoorDelay());
    }

    private IEnumerator OpenDoorDelay() {
        yield return new WaitForSeconds(doorOpenDelay);
        doorState = DoorState.Open;
        animController.SetBool("Closed", false);
        extraOpenEvent.Invoke();
    }
}
