using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILerp : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;
    
    private Vector3 destinationPosition;

    public float moveSpeed;
    public float stoppingDistance;

    private bool active = false;

    public void LerpToEndPos() {
        transform.position = startPos.position;
        destinationPosition = endPos.position;
        active = true;
    }

    public void LerpToStartPos() {
        transform.position = endPos.position;
        destinationPosition = startPos.position;
        active = true;
    }

    private void Update() {
        if (active) {
            transform.position = Vector3.Lerp(transform.position, destinationPosition, moveSpeed * Time.deltaTime);
            active = Vector3.Distance(transform.position, destinationPosition) > stoppingDistance;
        }
    }
}
