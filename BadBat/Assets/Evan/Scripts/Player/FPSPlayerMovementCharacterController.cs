using System;

using UnityEngine;
using UnityEngine.InputSystem;

namespace Evan.Scripts.PlayerMovement
{
    public class FPSPlayerMovementCharacterController : MonoBehaviour
    {
        public FPSPlayerInputActions playerInputActions;
        private FPSPlayerInput playerInput;
        private CharacterController characterController;
        private Camera playerCamera;

        [Header("Player Settings")]
        [SerializeField] public float baseMoveSpeed = 8;
        [SerializeField] private float stoppingLerpPower = 5;
        [SerializeField] public float sprintMultiplier = 1.8f;
        [SerializeField] public float crouchMultiplier = 0.8f;
        [SerializeField] private float currentMoveSpeed;
        private Vector2 rawMoveInput;
        private Vector3 velocity;
        [SerializeField] public float gravity = -9.81f;
        [SerializeField] public float jumpForce = 79;
        [SerializeField] private float coyoteTime = 0.5f;
        private float lastTimeGrounded;
        private float lastTimeJumped;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundDistance = 0.35f;
        [SerializeField] private LayerMask groundMask;
        private float currentHeight;
        [SerializeField] private Vector3 cameraOffset;
        [SerializeField] private bool toggleSprint;
        [SerializeField] private bool toggleCrouch;
        [SerializeField] private float normalHeight;
        [SerializeField] private float crouchHeight;
        [SerializeField] private float slideHeight;
        [SerializeField] public float slideSpeedMulti;
        private float currentSlideSpeedMulti;
        [SerializeField] public float slideFalloff;
        private Vector3 slideDirection;
        private Coroutine slideCoroutine;
        [SerializeField] private float slideJumpMulti;
    
        [Header("Player Status")]
        [SerializeField] private bool isMoving;
        [SerializeField] private bool isGrounded;
        [SerializeField] private bool isSprinting;
        [SerializeField] private bool isCrouched;
        [SerializeField] private bool isSliding;
    

        private void Awake() {
           
        }

        

        private void Start() {
            playerInputActions = GetComponent<FPSPlayerInput>().playerInputActions;
            characterController = GetComponent<CharacterController>();
            playerCamera = GetComponentInChildren<Camera>();
            
            currentMoveSpeed = baseMoveSpeed; //Reset our players move speed to the walking speed
            currentHeight = normalHeight;
            characterController.height = normalHeight;
            lastTimeJumped = 0;

            //Assigning input delegates
            playerInput = GetComponent<FPSPlayerInput>();
            playerInput.Jump = Jump;
            playerInput.Crouch = Crouch;
            playerInput.Sprint = Sprint;
        }
        

        private void Update() {
            
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            if (isGrounded) {
                lastTimeGrounded = Time.time;
            }
        
            //Set the players downward velocity to something small when they land
            if (isGrounded && velocity.y < 0) {
                velocity.y = -2f;
            }
        
            //Movement
            if (!isSliding) {
                //Uses the immediate stop
                // rawMoveInput = playerInputActions.Player.Move.ReadValue<Vector2>();
                
                //Use the lerped move so there is no immediate stop
                var directInput = playerInputActions.Player.Move.ReadValue<Vector2>();
                rawMoveInput = Vector2.Lerp(rawMoveInput, directInput, stoppingLerpPower * Time.deltaTime);
                
                if (Vector3.Magnitude(new Vector3(rawMoveInput.x, rawMoveInput.y, 0)) <= 0.1f) {
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

                if ( currentSlideSpeedMulti <= 0.4f && isSliding) {
                    currentHeight = crouchPressed ? crouchHeight : normalHeight;
                    isSliding = false;
                    isCrouched = crouchPressed;
                }
            }
        

            //Make gravity effect the player controller
            velocity.y += gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
        
            //Set cc height [crouch]
            characterController.center = Vector3.down * (normalHeight - characterController.height) / 2.0f;
            characterController.height = Mathf.Lerp(characterController.height, currentHeight, 0.05f);
            playerCamera.transform.localPosition = new Vector3(0f, characterController.center.y + characterController.height / 2, 0f) - cameraOffset;
        }

        public void Jump(bool pressed) {
            if (pressed) {
                if ((isGrounded || Time.time < lastTimeGrounded + coyoteTime) && Time.time > lastTimeJumped + coyoteTime) {
                    var tempJumpForce = jumpForce;
            
                    if (isSliding) {
                        SlideCancel();
                        tempJumpForce *= slideJumpMulti;
                    }
            
                    velocity.y = Mathf.Sqrt(tempJumpForce + -2f * gravity);

                    lastTimeJumped = Time.time;
                }
            }
        }

        public void Sprint(bool pressed) {
            if (!isSliding) {
                if (isCrouched) {
                    //Cancel Crouch
                    SlideCancel();
                }
            
                if (!toggleSprint) {
                    isSprinting = pressed;
                } else {
                    if (pressed) {
                        isSprinting = !isSprinting;
                    }
                
                }
            }
        }

        private bool crouchPressed = false;
        public void Crouch(bool pressed) {
            crouchPressed = pressed;

            //Basic Crouch
            if (!isSliding && !isSprinting) {
                if (!toggleCrouch) {
                    isCrouched = pressed;
                } else {
                    if (pressed) {
                        isCrouched = !isCrouched;
                    }
                }

                if (isCrouched) {
                    currentHeight = crouchHeight;
                } else {
                    currentHeight = normalHeight;
                }
            } else {
                if (pressed) {
                    //Slide
                    if (isSprinting && !isSliding && isGrounded) {
                        isSliding = true;
                
                        var inputDirection = playerInputActions.Player.Move.ReadValue<Vector2>();
                        slideDirection = (inputDirection.y * transform.forward) + (inputDirection.x * transform.right);
                
                        currentSlideSpeedMulti = slideSpeedMulti;
                
                        currentHeight = slideHeight;
                        isSprinting = false;
                
                    }else if (isSliding) {
                        //Cancel slide
                        SlideCancel();
                    }
                }

            }

        }

        private void SlideCancel() {
            isCrouched = false;
            currentHeight = normalHeight;
            isSliding = false;
        }

        private void OnDrawGizmos() {
            Gizmos.color = new Color(1, 0, 0, 0.25f);
            Gizmos.DrawSphere(groundCheck.position, groundDistance);
        }
        
        //World Effects
        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Ice")) {
                stoppingLerpPower = 1;
                baseMoveSpeed = 12;
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("Ice")) {
                stoppingLerpPower = 5;
                baseMoveSpeed = 8;
            }
        }


    }
}
