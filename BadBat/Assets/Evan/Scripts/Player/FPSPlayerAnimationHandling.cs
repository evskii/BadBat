using System;
using System.Collections;
using System.Collections.Generic;

using Evan.Scripts.PlayerMovement;

using UnityEngine;

public class FPSPlayerAnimationHandling : MonoBehaviour
{
    [SerializeField] private Animator leftGauntletAnimController;
    [SerializeField] private Animator rightGauntletAnimController;
    private CharacterController characterController;
    private FPSPlayerMovementCharacterController playerScript;

    private Vector3 lastFramePos;

    private void Start() {
        characterController = GetComponent<CharacterController>();
        playerScript = GetComponent<FPSPlayerMovementCharacterController>();
    }

    private void FixedUpdate() {
        //Basic movement
        // var currentSpeed = GetHorizontalSpeed();
        // leftGauntletAnimController.SetFloat("MoveSpeed", currentSpeed);
        // rightGauntletAnimController.SetFloat("MoveSpeed", currentSpeed);

        bool walking = playerScript.isMoving;
        bool sprinting = playerScript.isSprinting;
        
        leftGauntletAnimController.SetBool("Sprinting", sprinting);
        leftGauntletAnimController.SetBool("Moving", walking);
        
        rightGauntletAnimController.SetBool("Sprinting", sprinting);
        rightGauntletAnimController.SetBool("Moving", walking);

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
