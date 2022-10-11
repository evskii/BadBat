using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FPSPlayerMovementCharacterController : MonoBehaviour
{
    [HideInInspector] public FPSPlayerInputActions playerInputActions;
    private CharacterController characterController;

    [Header("Player Settings")]
    [SerializeField] private float baseMoveSpeed;
    [SerializeField] private float sprintMultiplier; 
    [SerializeField] private float currentMoveSpeed;
    private Vector2 rawMoveInput;
    private Vector3 velocity;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance;
    [SerializeField] private LayerMask groundMask;
    
    [Header("Player Status")]
    [SerializeField] private bool isMoving;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isSprinting;
    

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
    }

    private void Update() {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //Set the players downward velocity to something small when they land
        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }
        
        //Movement
        rawMoveInput = playerInputActions.Player.Move.ReadValue<Vector2>();

        if (Vector3.Magnitude(new Vector3(rawMoveInput.x, rawMoveInput.y, 0)) == 0) {
            isSprinting = false;
        }
        currentMoveSpeed = isSprinting ? baseMoveSpeed * sprintMultiplier : baseMoveSpeed;
        
        Vector3 movement = (rawMoveInput.y * transform.forward) + (rawMoveInput.x * transform.right);
        characterController.Move(movement * currentMoveSpeed * Time.deltaTime);

        //Tells us if the player is currently moving
        if (Mathf.Abs(rawMoveInput.y) > 0 || Mathf.Abs(rawMoveInput.x) > 0) {
            isMoving = true;
        } else {
            isMoving = false;
        }

        //Make gravity effect the player controller
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void OnJump() {
        if (isGrounded) {
            velocity.y = Mathf.Sqrt(jumpForce + -2f * gravity); 
        }
    }

    private void OnSprint() {
        isSprinting = true;
    }
}
