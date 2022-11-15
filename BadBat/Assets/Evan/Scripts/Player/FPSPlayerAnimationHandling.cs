using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class FPSPlayerAnimationHandling : MonoBehaviour
{
    [SerializeField] private Animator animController;
    private CharacterController characterController;

    private Vector3 lastFramePos;

    private void Start() {
        characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate() {
        //Basic movement
        var currentSpeed = GetHorizontalSpeed();
        animController.SetFloat("MoveSpeed", currentSpeed);
        
    }

    //Get the magnitude of how far the player has travelled since last check (I guess only really
    //works if we are checking every frame basically)
    private float GetHorizontalSpeed() {

        var currentPos = new Vector3(transform.position.x, 0f, transform.position.z);
        var lastPos = new Vector3(lastFramePos.x, 0f, lastFramePos.z);

        lastFramePos = currentPos;
        
        // return Mathf.Abs(Vector3.Distance(lastPos, currentPos)) / Time.deltaTime; 
        return (currentPos - lastPos).magnitude;
    }


}
