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

    private void OnDisable() {
        playerInputActions.Disable();
    }

    private void Start() {
        //Get references
        rigidbody = GetComponent<Rigidbody>();
        fpsCamera = GetComponentInChildren<Camera>().transform;
        playerInputActions = new FPSPlayerInputActions();
        playerInputActions.Enable();
        
        //Initialize variables
        currentMoveSpeed = baseMoveSpeed;
    }

    private float rotY= 0;

    private void FixedUpdate() {
        //Movement
        var rawMoveData = playerInputActions.Player.Move.ReadValue<Vector2>();
        moveVector = new Vector3(rawMoveData.x, 0, rawMoveData.y);
        var finalMoveVector = transform.TransformDirection(new Vector3(rawMoveData.x, rigidbody.velocity.y, rawMoveData.y) * currentMoveSpeed);
        finalMoveVector = Vector3.Lerp(rigidbody.velocity, finalMoveVector, 0.25f);
        rigidbody.velocity = finalMoveVector;

        //Look
        var rawLookData = playerInputActions.Player.Look.ReadValue<Vector2>();
        
        rotY += rawLookData.y * (lookSpeed * Time.fixedDeltaTime);
        rotY = Mathf.Clamp(rotY, -90f, 90f);
        fpsCamera.rotation = Quaternion.Euler(-rotY, fpsCamera.rotation.y, fpsCamera.rotation.z);

        transform.Rotate(new Vector3(0, rawLookData.x, 0) * (lookSpeed * Time.fixedDeltaTime));
        // fpsCamera.transform.Rotate(new Vector3(rawLookData.y, 0, 0) * (lookSpeed * Time.fixedDeltaTime));
        // var lookRotation = fpsCamera.transform.rotation.eulerAngles;
        // // fpsCamera.transform.rotation = Quaternion.Euler(lookRotation);
        // Debug.Log(lookRotation.x);
        
        
        
    }

}
