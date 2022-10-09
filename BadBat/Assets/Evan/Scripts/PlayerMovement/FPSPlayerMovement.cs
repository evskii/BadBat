using System;
using System.Collections;
using System.Collections.Generic;

using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class FPSPlayerMovement : MonoBehaviour
{
    [Header("Player Movement Settings")]
    [SerializeField] private float baseMoveSpeed;
    [SerializeField] private float sprintMultiplier; 
    private float currentMoveSpeed;
    [SerializeField] private float lookSpeed; //Sensitivity

    //Random Shit
    private Vector3 moveVector;
    private Vector3 lookVector;
    
    //References
    [Header("References")]
    [SerializeField] private Transform fpsCamera;
    private Rigidbody rigidbody;
    private FPSPlayerInputActions playerInputActions;
    
    

    private void Start() {
        //Get references
        rigidbody = GetComponent<Rigidbody>();
        fpsCamera = GetComponentInChildren<Camera>().transform;
        playerInputActions = new FPSPlayerInputActions();
        playerInputActions.Player.Enable();
        
        //Initialize variables
        currentMoveSpeed = baseMoveSpeed;
    }
    
    //Get our directional movement input
    
    // public void OnMove(InputValue value) {
    //     //Debug.Log(value.Get<Vector2>());
    //     var rawInput = value.Get<Vector2>();
    //     Vector3 finalInput = new Vector3(rawInput.x, 0, rawInput.y);
    //
    //     // rigidbody.velocity = finalInput * currentMoveSpeed;
    //     moveVector = finalInput;
    // }

    private void FixedUpdate() {
        var rawMoveData = playerInputActions.Player.Move.ReadValue<Vector2>();
        moveVector = new Vector3(rawMoveData.x, 0, rawMoveData.y);
        var finalMoveVector = Vector3.Scale(transform.forward, moveVector);
        rigidbody.velocity = Vector3.Normalize(finalMoveVector * (currentMoveSpeed * Time.fixedDeltaTime));

        var rawLookData = playerInputActions.Player.Look.ReadValue<Vector2>();
        transform.Rotate(new Vector3(0, rawLookData.x, 0) * (lookSpeed * Time.fixedDeltaTime));
        fpsCamera.transform.Rotate(new Vector3(rawLookData.y, 0, 0) * (lookSpeed * Time.fixedDeltaTime));
    }

    // public void OnLook(InputValue value) {
    //     var rawInput = value.Get<Vector2>();
    //     transform.Rotate(new Vector3(0, rawInput.x, 0) * lookSpeed);
    //     fpsCamera.transform.Rotate(new Vector3(rawInput.y, 0, 0) * lookSpeed);
    // }
}
