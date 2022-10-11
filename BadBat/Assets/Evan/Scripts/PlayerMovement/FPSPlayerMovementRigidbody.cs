using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class FPSPlayerMovementRigidbody : MonoBehaviour
{
    [Header("Player Movement Settings")]
    [SerializeField] private float baseMoveSpeed;
    [SerializeField] private float sprintMultiplier; 
    private float currentMoveSpeed;
    [SerializeField] private float lookSpeed; //Sensitivity
    [SerializeField] private float lookRadius = 50;

    //Random Shit
    private Vector3 moveVector;
    private float rotY= 0;
    
    //References
    [Header("References")]
    [SerializeField] private Transform fpsCamera;
    private Rigidbody rigidbody;
    private FPSPlayerInputActions playerInputActions;

    private void Awake() {
        playerInputActions = new FPSPlayerInputActions();
    }

    private void OnDisable() {
        playerInputActions.Disable();
    }

    private void OnEnable() {
        playerInputActions.Enable();
    }

    private void Start() {
        //Get references
        rigidbody = GetComponent<Rigidbody>();
        fpsCamera = GetComponentInChildren<Camera>().transform;
        
        //Initialize variables
        currentMoveSpeed = baseMoveSpeed;
    }

    

    private void FixedUpdate() {
        //Lateral Movement
        var rawMoveData = playerInputActions.Player.Move.ReadValue<Vector2>();
        moveVector = new Vector3(rawMoveData.x, 0, rawMoveData.y);
        var finalMoveVector = transform.TransformDirection(new Vector3(rawMoveData.x, rigidbody.velocity.y, rawMoveData.y) * currentMoveSpeed);
        finalMoveVector = Vector3.Lerp(rigidbody.velocity, finalMoveVector, 0.25f);
        rigidbody.velocity = finalMoveVector;

        //Look Rotations
        var rawLookData = playerInputActions.Player.Look.ReadValue<Vector2>(); //Store input from mouse 
        rotY += rawLookData.y * (lookSpeed * Time.fixedDeltaTime); //Add to the Y rotation value from the mouse
        rotY = Mathf.Clamp(rotY, -lookRadius, lookRadius); //Clamp it between a vertical radius
        fpsCamera.localRotation = Quaternion.Euler(-rotY, 0f, 0f); //Set the cameras local rotation

        transform.Rotate(new Vector3(0, rawLookData.x, 0) * (lookSpeed * Time.fixedDeltaTime)); //Use the mouse to rotate the character so we turn
        
    }

}
