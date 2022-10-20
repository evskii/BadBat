using UnityEngine;
using UnityEngine.InputSystem;
namespace Evan.Scripts.PlayerMovement
{
    public class FPSPlayerInput : MonoBehaviour
    {
        [HideInInspector] public FPSPlayerInputActions playerInputActions;
        [HideInInspector] public FPSPlayerMovementCharacterController fpsPlayer;
        public GadgetAndGizmo leftGauntlet;
        public GadgetAndGizmo rightGauntlet;

        public bool canJump = true;
        public bool canCrouch = true;
        public bool canSprint = true;
        public bool canChangeAbility = true;
        public bool canFireLeft = true;
        public bool canFireRight = true;
        
        private void Awake() {
            playerInputActions = new FPSPlayerInputActions();
            fpsPlayer = GetComponent<FPSPlayerMovementCharacterController>();
        }
    
        private void OnEnable() {
            playerInputActions.Enable();
        }

        private void OnDisable() {
            playerInputActions.Disable();
        }

        public void OnJump() {
            if (canJump) {
                fpsPlayer.Jump();
            }
        }

        public void OnCrouch(InputValue context) {
            if (canCrouch) {
                fpsPlayer.Crouch(context);
            }
            
        }

        public void OnSprint(InputValue context) {
            if (canSprint) {
                fpsPlayer.Sprint(context);
            }
        }

        public void OnFireLeft() {
            if (canFireLeft) {
                leftGauntlet.Fire();
            }
            
        }

        public void OnFireRight() {
            if (canFireRight) {
                rightGauntlet.Fire();
            }
        }

        public void OnSwapAbility() {
            if (canChangeAbility) {
                FindObjectOfType<WeaponSelectMenuController>().ToggleMenu();
            }
            
        }

        public void ToggleBasicMoves(bool enable) {
            //Disables send message calls
            canJump = enable;
            canCrouch = enable;
            canSprint = enable;
            canFireLeft = enable;
            canFireRight = enable;

            //Disables walking and looking
            var fpsPlayer = GetComponent<FPSPlayerMovementCharacterController>();
            if (enable) {
                fpsPlayer.playerInputActions.Enable();
            } else {
                fpsPlayer.playerInputActions.Disable();
            }
        }
    }
}
