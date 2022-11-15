using UnityEngine;
using UnityEngine.InputSystem;
namespace Evan.Scripts.PlayerMovement
{
    [DefaultExecutionOrder(-1)]
    public class FPSPlayerInput : MonoBehaviour
    {
        [HideInInspector] public FPSPlayerInputActions playerInputActions;
        public GadgetAndGizmo leftGauntlet;
        public GadgetAndGizmo rightGauntlet;
        
        private void Awake() {
            playerInputActions = new FPSPlayerInputActions();

            playerInputActions.Player.Jump.started += ctx => Jump(true);
            playerInputActions.Player.Jump.canceled += ctx => Jump(false);

            playerInputActions.Player.Crouch.started += ctx => Crouch(true);
            playerInputActions.Player.Crouch.canceled += ctx => Crouch(false);

            playerInputActions.Player.Sprint.started += ctx => Sprint(true);
            playerInputActions.Player.Sprint.canceled += ctx => Sprint(false);

            playerInputActions.Player.FireLeft.started += ctx => LeftFire(true);
            playerInputActions.Player.FireLeft.canceled += ctx => LeftFire(false);
            
            playerInputActions.Player.FireRight.started += ctx => RightFire(true);
            playerInputActions.Player.FireRight.canceled += ctx => RightFire(false);

            //Moved to a whole new script [RadialWeaponWheel.cs]
            // playerInputActions.Player.SwapAbility.started += ctx => SwapAbility(true);
            // playerInputActions.Player.SwapAbility.canceled += ctx => SwapAbility(false);
        }

        
        //Method Delegates
        public delegate void DelegateJump(bool pressed);
        public DelegateJump Jump;

        public delegate void DelegateCrouch(bool pressed);
        public DelegateCrouch Crouch;

        public delegate void DelegateSprint(bool pressed);
        public DelegateSprint Sprint;

        public delegate void DelegateLeftFire(bool pressed);
        public DelegateLeftFire LeftFire;

        public delegate void DelegateRightFire(bool pressed);
        public DelegateRightFire RightFire;

        // public delegate void DelegateSwapAbility(bool pressed);
        // public DelegateSwapAbility SwapAbility;
        
    
        private void OnEnable() {
            playerInputActions.Enable();
        }

        private void OnDisable() {
            playerInputActions.Disable();
        }


        public void ToggleBasicMoves(bool enable) {
            if (enable) {
                playerInputActions.Player.Enable();
            } else {
                playerInputActions.Player.Disable();
            }
        }
    }
}
