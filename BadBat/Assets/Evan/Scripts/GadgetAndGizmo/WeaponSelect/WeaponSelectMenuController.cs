using System;
using System.Collections;
using System.Collections.Generic;

using Evan.Scripts.PlayerMovement;

using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectMenuController : MonoBehaviour
{
	[HideInInspector] public FPSPlayerInputActions uiInputActions;
        
	private void Awake() {
		uiInputActions = new FPSPlayerInputActions();
		uiInputActions.UI.SwapAbility.started += ctx => ToggleMenu();
	}
	
	private void OnEnable() {
		uiInputActions.UI.Enable();
	}

	private void OnDisable() {
		uiInputActions.UI.Disable();
	}
	
	
	
	public GameObject button;

	public Transform weaponSelectUi;
	
	public Transform leftGauntletTrans;
	public Transform rightGauntletTrans;

	private GadgetAndGizmo leftGauntlet;
	private GadgetAndGizmo rightGauntlet;

	private void Start() {
		
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
		var menuStatus = weaponSelectUi.gameObject.activeSelf;

		if (menuStatus) {
			CloseMenu();
		} else {
			OpenMenu();
		}
	}
	
	[ContextMenu("Open Menu")]
	public void OpenMenu() {
		FindObjectOfType<FPSPlayerInput>().ToggleBasicMoves(false);
		Cursor.lockState = CursorLockMode.None;
		weaponSelectUi.gameObject.SetActive(true);
		FindObjectOfType<FPSPlayerInput>().enabled = false;
		Time.timeScale = 0.1f;
		foreach (var ability in leftGauntlet.availableAbilities) {
			var newButton = Instantiate(button, leftGauntletTrans);
			newButton.GetComponent<WeaponSelectButtonInfo>().InitButtonInfo(ability, leftGauntlet);
		}
		foreach (var ability in rightGauntlet.availableAbilities) {
			var newButton = Instantiate(button, rightGauntletTrans);
			newButton.GetComponent<WeaponSelectButtonInfo>().InitButtonInfo(ability, rightGauntlet);
		}
	}

	[ContextMenu("Close Menu")]
	public void CloseMenu() {
		//Clear Menu
		foreach (Transform button in leftGauntletTrans) {
			if (button.TryGetComponent(out WeaponSelectButtonInfo butt)) {
				Destroy(butt.gameObject);
			}
		}
		foreach (Transform button in rightGauntletTrans) {
			if (button.TryGetComponent(out WeaponSelectButtonInfo butt)) {
				Destroy(butt.gameObject);
			}
		}
		
		
		Time.timeScale = 1f;
		Cursor.lockState = CursorLockMode.Locked;
		weaponSelectUi.gameObject.SetActive(false);
		FindObjectOfType<FPSPlayerInput>().enabled = true;
		FindObjectOfType<FPSPlayerInput>().ToggleBasicMoves(true);
	}
}
