using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Puzzle_OpenableDoor : MonoBehaviour
{
    public bool doorOpen = false;
    public Transform closedPos;
    public Transform openedPos;
    private Transform targetPos;
    private bool moving;
    public float openSpeed;

    private void Start() {
        transform.position = closedPos.position;
        doorOpen = false;
    }

    public void OpenDoor() {
        moving = true;
        targetPos = openedPos;
        doorOpen = true;
    }

    public void CloseDoor() {
        moving = true;
        targetPos = closedPos;
        doorOpen = false;
    }

    private void Update() {
        if (moving) {
            transform.position = Vector3.Lerp(transform.position, targetPos.position, openSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPos.position) <= 0.05f) {
                transform.position = targetPos.position;
                moving = false;
            }
        }
    }
}
