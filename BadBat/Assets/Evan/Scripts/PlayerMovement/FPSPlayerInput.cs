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
            fpsPlayer.Jump();
        }

        public void OnCrouch(InputValue context) {
            fpsPlayer.Crouch(context);
        }

        public void OnSprint(InputValue context) {
            fpsPlayer.Sprint(context);
        }

        public void OnFireLeft() {
            leftGauntlet.Fire();
        }

        public void OnFireRight() {
            rightGauntlet.Fire();
        }
    }
}
