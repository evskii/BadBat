using System;
using System.Collections;
using System.Collections.Generic;

using Unity.Collections.LowLevel.Unsafe;

using UnityEditor;

using UnityEngine;
using UnityEngine.InputSystem;

public class FPSPlayerMovementCharacterController : MonoBehaviour
{
    [HideInInspector] public FPSPlayerInputActions playerInputActions;
    private CharacterController characterController;

    [Header("Player Settings")]
    [SerializeField] private float baseMoveSpeed;
    [SerializeField] private float sprintMultiplier;
    [SerializeField] private float crouchMultiplier;
    [SerializeField] private float currentMoveSpeed;
    private Vector2 rawMoveInput;
    private Vector3 velocity;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundMask;
    private float currentHeight;
    [SerializeField] private float normalHeight;
    [SerializeField] private float crouchHeight;
    [SerializeField] private float slideHeight;
    [SerializeField] private float slideSpeedMulti;
    private float currentSlideSpeedMulti;
    [SerializeField] private float slideTime;
    [SerializeField] private float slideFalloff;
    private Vector3 slideDirection;
    private Coroutine slideCoroutine;
    
    [Header("Player Status")]
    [SerializeField] private bool isMoving;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isSprinting;
    [SerializeField] private bool isCrouched;
    [SerializeField] private bool isSliding;
    

    private void Awake() {
        playerInputActions = new FPSPlayerInputActions();
        characterController = GetComponent<CharacterController>();
    }
    
    private void OnEnable() {
        playerInputActions.Enable();
    }

    private void OnDisable() {
        playerInputActions.Disable();
    }


    private void Start() {
        currentMoveSpeed = baseMoveSpeed; //Reset our players move speed to the walking speed
        currentHeight = normalHeight;
    }

    private void Update() {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //Set the players downward velocity to something small when they land
        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }
        
        //Movement
        if (!isSliding) {
            rawMoveInput = playerInputActions.Player.Move.ReadValue<Vector2>();
            if (Vector3.Magnitude(new Vector3(rawMoveInput.x, rawMoveInput.y, 0)) == 0) {
                isSprinting = false;
            }
            currentMoveSpeed = isSprinting ? baseMoveSpeed * sprintMultiplier : baseMoveSpeed;
            currentMoveSpeed = isCrouched ? baseMoveSpeed * crouchMultiplier : currentMoveSpeed;
            Vector3 movement = (rawMoveInput.y * transform.forward) + (rawMoveInput.x * transform.right);
            characterController.Move(movement * currentMoveSpeed * Time.deltaTime);

            //Tells us if the player is currently moving
            if (Mathf.Abs(rawMoveInput.y) > 0 || Mathf.Abs(rawMoveInput.x) > 0) {
                isMoving = true;
            } else {
                isMoving = false;
            }
        } else {
            
            characterController.Move(slideDirection * (baseMoveSpeed * currentSlideSpeedMulti) * Time.deltaTime);

            currentSlideSpeedMulti = Mathf.Lerp(currentSlideSpeedMulti, .25f, slideFalloff * Time.deltaTime);

            if ( currentSlideSpeedMulti <= 0.4f) {
                currentHeight = crouchHeight;
                isSliding = false;
                isCrouched = true;
            }
        }
        

        //Make gravity effect the player controller
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
        
        //Set cc height [crouch]
        characterController.height = Mathf.Lerp(characterController.height, currentHeight, 0.1f);
    }

    private void OnJump() {
        if (isGrounded) {
            velocity.y = Mathf.Sqrt(jumpForce + -2f * gravity);

            if (isSliding) {
                SlideCancel();
            }
        }
    }

    private void OnSprint() {
        if (!isSliding && !isCrouched) {
            isSprinting = true;
        }
    }

    private void OnCrouch(InputValue context) {
        
        if (isSprinting) {
            if (context.isPressed) {
                isSliding = true;

                var inputDirection = playerInputActions.Player.Move.ReadValue<Vector2>();
                slideDirection = (inputDirection.y * transform.forward) + (inputDirection.x * transform.right);

                currentSlideSpeedMulti = slideSpeedMulti;

                currentHeight = slideHeight;
                isSprinting = false;
            }
        } else if (!isSliding){
            isCrouched = context.isPressed;
            if (context.isPressed) {
                currentHeight = crouchHeight;
            } else {
                currentHeight = normalHeight;
            }
        }

    }

    private void SlideCancel() {
        currentHeight = normalHeight;
        isSliding = false;
    }
}
