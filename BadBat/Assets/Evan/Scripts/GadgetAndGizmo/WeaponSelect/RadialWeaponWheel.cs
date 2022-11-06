using System;
using System.Collections;
using System.Collections.Generic;

using Evan.Scripts.PlayerMovement;

using UnityEngine;
using UnityEngine.UI;


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
    
    [ContextMenu("Open Menu")]
    public void OpenMenu() {
        FindObjectOfType<FPSPlayerInput>().ToggleBasicMoves(false);
        Cursor.lockState = CursorLockMode.None;
        wheelUiParent.gameObject.SetActive(true);
        FindObjectOfType<FPSPlayerInput>().enabled = false;
        Time.timeScale = 0.1f;
        
        SpawnSegments();
    }

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

    private void ClickSegment() {
        if (wheelUiParent.gameObject.activeSelf) {
            GetSegment().GetComponent<WeaponSelectButtonInfo>().Equip();
        }
    }

    //Tracks the mouse around the wheel to pick a segment
    public GameObject GetSegment() {
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
            imageComponent.fillAmount = Map(rotationAmt, 0, 360, 0, 1);
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
            imageComponent.fillAmount = Map(rotationAmt, 0, 360, 0, 1);
            imageComponent.alphaHitTestMinimumThreshold = 0.5f;
            // imageComponent.color = Random.ColorHSV();
            newSegment.name = rightGauntlet.availableAbilities[i].abilityName;
            wheelRightButtons.Add(newSegment);
            newSegment.GetComponent<WeaponSelectButtonInfo>().InitButtonInfo(rightGauntlet.availableAbilities[i], rightGauntlet);
        }
    }

    private float Map(float x, float a, float b, float c, float d) {
        return (x - a) / (b - a) * (d - c) + c;
    }
}
