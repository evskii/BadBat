using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

using Evan.Scripts.PlayerMovement;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

using Button = UnityEngine.UI.Button;
using Cursor = UnityEngine.Cursor;
using Image = UnityEngine.UI.Image;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


//   (                                 _
//    )                               /=>
//   (  +____________________/\/\___ / /|
//    .''._____________'._____      / /|/\
//   : () :              :\ ----\|    \ )
//    '..'______________.'0|----|      \
//                     0_0/____/        \
//                         |----    /----\
//                        || -\\ --|      \
//                        ||   || ||\      \
//                         \\____// '|      \
//                                 .'/       |
//                                .:/        |
//                                :/_________|
//    USE THIS WEAPON TO SHOOT ME PLEASE X



public class RadialWeaponWheel : MonoBehaviour
{
    [HideInInspector] public FPSPlayerInputActions uiInputActions;
        
    private void Awake() {
        uiInputActions = new FPSPlayerInputActions();
        uiInputActions.UI.SwapAbility.started += ctx => ToggleMenu();
        uiInputActions.UI.Click.started += ctx => ClickSegment();
        
    }
	
    private void OnEnable() {
        uiInputActions.UI.Enable();
    }

    private void OnDisable() {
        uiInputActions.UI.Disable();
    }
    
    
    public int segmentsToCreate; //REDUNDANT MOTHER FUCKER

    public GameObject wheelUiParent;
    
    public Transform wheelParentLeft;
    public Transform wheelParentRight;
    public GameObject wheelButton;
    
    private GadgetAndGizmo leftGauntlet;
    private GadgetAndGizmo rightGauntlet;

    private List<GameObject> wheelLeftButtons = new List<GameObject>();
    private List<GameObject> wheelRightButtons = new List<GameObject>();

    private Vector2 lastMousePos;
    private float lastLeftMagnitude;
    private float lastRightMagnitude;
    private enum InputDevice
    {
        Mouse,
        Controller
    }

    private void Start() {
        //imageToTest.alphaHitTestMinimumThreshold = 0.5f;
        // SpawnSegments();
        
        var gauntlets = FindObjectsOfType<GadgetAndGizmo>();

        leftGauntlet = gauntlets[0].arm == GadgetAndGizmo.Arm.Left ? gauntlets[0] : gauntlets[1];
        rightGauntlet = gauntlets[0].arm == GadgetAndGizmo.Arm.Right ? gauntlets[0] : gauntlets[1];

        if (leftGauntlet == null) {
            Debug.Log("Left Gauntlet Not Referenced in Weapon Select");
        }
        if (rightGauntlet == null) {
            Debug.Log("Right Gauntlet Not Referenced in Weapon Select");
        }
    }
    
    public void ToggleMenu() {
        var menuStatus = wheelUiParent.gameObject.activeSelf;

        if (menuStatus) {
            CloseMenu();
        } else {
            OpenMenu();
        }
    }

    private void Update() {
        var menuStatus = wheelUiParent.gameObject.activeSelf;
        if (menuStatus) {
            HoverSegment();
        }
    }


    //Called to open the weapon wheel UI
    [ContextMenu("Open Menu")]
    public void OpenMenu() {
        FindObjectOfType<FPSPlayerInput>().ToggleBasicMoves(false);
        Cursor.lockState = CursorLockMode.None;
        wheelUiParent.gameObject.SetActive(true);
        FindObjectOfType<FPSPlayerInput>().enabled = false;
        Time.timeScale = 0.1f;
        
        SpawnSegments();

        foreach (var button in wheelLeftButtons) {
            WeaponSelectButtonInfo buttonInfo = button.GetComponent<WeaponSelectButtonInfo>();
            if (buttonInfo.IsEquipped()) {
                buttonInfo.SelectButton();
                currentSelectedLeftButton = buttonInfo;
            }
        }

        foreach (var button in wheelRightButtons) {
            WeaponSelectButtonInfo buttonInfo = button.GetComponent<WeaponSelectButtonInfo>();
            if (buttonInfo.IsEquipped()) {
                buttonInfo.SelectButton();
                currentSelectedRightButton = buttonInfo;
            }
        }
    }

    //Called to close the weapon wheel UI
    [ContextMenu("Close Menu")]
    public void CloseMenu() {
        wheelUiParent.gameObject.SetActive(false);
        
        wheelLeftButtons.Clear();
        wheelRightButtons.Clear();
        
        //Clear Menu
        foreach (Transform button in wheelParentLeft) {
            if (button.TryGetComponent(out WeaponSelectButtonInfo butt)) {
                Destroy(butt.gameObject);
            }
        }
        foreach (Transform button in wheelParentRight) {
            if (button.TryGetComponent(out WeaponSelectButtonInfo butt)) {
                Destroy(butt.gameObject);
            }
        }
		
		
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        
        FindObjectOfType<FPSPlayerInput>().enabled = true;
        FindObjectOfType<FPSPlayerInput>().ToggleBasicMoves(true);
    }

    //Called when the player inputs that they want to select the button they are pointing at
    private void ClickSegment() {
        if (wheelUiParent.gameObject.activeSelf) { //Checks to see if the UI is actually open
            bool onLeft = false;
            if (GetActiveDevice() == InputDevice.Controller) {
                //Get our directional inputs from joysticks
                Vector2 leftJoystick = uiInputActions.UI.LeftJoystick.ReadValue<Vector2>();

                //Check if we are using the left or right one
                onLeft = leftJoystick.magnitude >= 0.1;
            } else {
                //Get input position from the mouse
                Vector2 mousePos = uiInputActions.UI.Point.ReadValue<Vector2>();

                //Check if we are on the left of the screen
                onLeft = mousePos.x < Screen.width / 2;
            }
            
            ChangeEquipped(onLeft, GetSegment().GetComponent<WeaponSelectButtonInfo>());
            // GetSegment().GetComponent<WeaponSelectButtonInfo>().Equip();
        }
    }

    //Used to store the currently selected buttons [active abilities] and allows change between them
    public AbilityClass lastEquippedAbility;
    private WeaponSelectButtonInfo currentSelectedLeftButton;
    private WeaponSelectButtonInfo currentSelectedRightButton;
    private void ChangeEquipped(bool changeLeft, WeaponSelectButtonInfo toChange) {
        //Not great code here but I just need it to work now
        if (changeLeft) {
            if (currentSelectedLeftButton) {
                currentSelectedLeftButton.DeselectButton();
            }
            currentSelectedLeftButton = toChange;
            currentSelectedLeftButton.Equip();
        } else {
            if (currentSelectedRightButton) {
                currentSelectedRightButton.DeselectButton();
            }
            currentSelectedRightButton = toChange;
            currentSelectedRightButton.Equip();
        }
        StartCoroutine(RegainControl());
        lastEquippedAbility = toChange.ability;
    }

    private IEnumerator RegainControl() {
        Time.timeScale = 1;
        yield return new WaitForSeconds(.1f);
        CloseMenu();
    }

    
    //Gets what input device is currently in use (If mouse is not moving it returns controller but this should be okay for now)
    private InputDevice currentDevice;
    private InputDevice GetActiveDevice() {
        Vector2 currentMousePos = uiInputActions.UI.Point.ReadValue<Vector2>();
        InputDevice deviceToReturn;
                    
        if (Vector2.Distance(currentMousePos, lastMousePos) <= 1f) { //If mouse hasn't moved
            float currentLeftMagnitude = uiInputActions.UI.LeftJoystick.ReadValue<Vector2>().magnitude;
            float currentRightMagnitude = uiInputActions.UI.RightJoystick.ReadValue<Vector2>().magnitude;
            if (currentLeftMagnitude != lastLeftMagnitude || currentRightMagnitude != lastRightMagnitude) { //If joysticks have moved
                lastLeftMagnitude = currentLeftMagnitude;
                lastRightMagnitude = currentRightMagnitude;
                deviceToReturn = InputDevice.Controller;
            } else { //If NOTHING has moved
                lastMousePos = currentMousePos;
                deviceToReturn = currentDevice;
            }
        } else { //If mouse has moved
            lastMousePos = currentMousePos;
            deviceToReturn =  InputDevice.Mouse;
        }

        currentDevice = deviceToReturn;
        return deviceToReturn;
    }

    //Tracks the mouse around the wheel to pick a segment
    public GameObject GetSegment() {
        // Debug.Log(GetActiveDevice());
        if (GetActiveDevice() == InputDevice.Controller) {
            //Get our inputs from joysticks
            Vector2 leftJoystick = uiInputActions.UI.LeftJoystick.ReadValue<Vector2>();
            Vector2 rightJoystick = uiInputActions.UI.RightJoystick.ReadValue<Vector2>();
            
            //Check if we are using the left or right one
            var leftWheel = leftJoystick.magnitude >= 0.1;
            
            //Get the position around the circle we are at
            Transform centerTrans = leftWheel ? wheelParentLeft : wheelParentRight;
            List<GameObject> listToUse = leftWheel ? wheelLeftButtons : wheelRightButtons;
            
            Vector2 centerPos = centerTrans.transform.position;
            Vector2 stickDir = leftWheel ? leftJoystick : rightJoystick;

            float angle = Vector2.Angle(Vector2.up, stickDir.normalized);

            float gapAngle = 360 / listToUse.Count;
            float finalAngle = stickDir.x < 0 ? 360 - angle : angle;

            int selectedButtonIndex = (int)MathF.Floor(finalAngle / gapAngle);
        
            return listToUse[selectedButtonIndex]; 

        } else {
            Vector2 mousePos = uiInputActions.UI.Point.ReadValue<Vector2>();

            var mouseOnLeft = mousePos.x < Screen.width / 2;

            //Decide if we are using the left or right side wheel
            Transform centerTrans = mouseOnLeft ? wheelParentLeft : wheelParentRight;
            List<GameObject> listToUse = mouseOnLeft ? wheelLeftButtons : wheelRightButtons;
        
            Vector2 centerPos = centerTrans.transform.position;
            Vector2 mouseDir = mousePos - centerPos;

            float angle = Vector2.Angle(Vector2.up, mouseDir.normalized);

            float gapAngle = 360 / listToUse.Count;
            float finalAngle = mousePos.x < centerPos.x ? 360 - angle : angle;

            int selectedButtonIndex = (int)MathF.Floor(finalAngle / gapAngle);
        
            return listToUse[selectedButtonIndex];   
        }
    }

    //Uses the GetSegment to highlight the button we are selecting
    private void HoverSegment() {
        GetSegment().GetComponent<Button>().Select();
    }

    //Spawns in a button for how many segments we need
    public void SpawnSegments() {
        //Spawn in left wheel
        float rotationAmt = 360 / leftGauntlet.availableAbilities.Count;

        for (int i = 0; i < leftGauntlet.availableAbilities.Count; i++) {
            GameObject newSegment = Instantiate(wheelButton, wheelParentLeft);
            // newSegment.transform.Rotate(0,0, -(i * rotationAmt));
            newSegment.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -(i * rotationAmt)));
            Image imageComponent = newSegment.GetComponent<Image>();
            imageComponent.fillAmount = EvMath.Map(rotationAmt, 0, 360, 0, 1);
            imageComponent.alphaHitTestMinimumThreshold = 0.5f;
            // imageComponent.color = Random.ColorHSV();
            newSegment.name = leftGauntlet.availableAbilities[i].abilityName;
            wheelLeftButtons.Add(newSegment);
            newSegment.GetComponent<WeaponSelectButtonInfo>().InitButtonInfo(leftGauntlet.availableAbilities[i], leftGauntlet);
        }
        
        //Spawn in right wheel
        rotationAmt = 360 / rightGauntlet.availableAbilities.Count;

        for (int i = 0; i < rightGauntlet.availableAbilities.Count; i++) {
            GameObject newSegment = Instantiate(wheelButton, wheelParentRight);
            // newSegment.transform.Rotate(0,0, -(i * rotationAmt));
            newSegment.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -(i * rotationAmt)));
            Image imageComponent = newSegment.GetComponent<Image>();
            imageComponent.fillAmount = EvMath.Map(rotationAmt, 0, 360, 0, 1);
            imageComponent.alphaHitTestMinimumThreshold = 0.5f;
            // imageComponent.color = Random.ColorHSV();
            newSegment.name = rightGauntlet.availableAbilities[i].abilityName;
            wheelRightButtons.Add(newSegment);
            newSegment.GetComponent<WeaponSelectButtonInfo>().InitButtonInfo(rightGauntlet.availableAbilities[i], rightGauntlet);
        }
    }
}
