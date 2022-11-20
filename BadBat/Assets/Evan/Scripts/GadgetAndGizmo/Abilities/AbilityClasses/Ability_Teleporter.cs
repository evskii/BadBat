using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ability_Teleporter : AbilityClass
{
	public GameObject placedTeleporter;
	public LayerMask layerMask;

	private bool visualisationMode = false;
	

	public override void Equip(GameObject player, GameObject gauntlet, GadgetAndGizmo myGag) {
		this.player = player;
		this.gauntlet = gauntlet;
		this.myGag = myGag;
	}

	public override void Fire(bool pressed) {
		visualisationMode = pressed && !placedTeleporter;
		
		
		if (!placedTeleporter) { 
			
			if (pressed) {
				myGag.AnimWindUp();
			} else {
				myGag.AnimFire();
			}
			
			if (!visualisationMode) { 
				RaycastHit hit;
			
				if (Physics.Raycast(gauntlet.transform.position, gauntlet.transform.forward , out hit, Mathf.Infinity, layerMask)) {
					if (hit.normal.normalized == new Vector3(0, 1, 0)) { //If its a horizontal flat surface
						placedTeleporter = Instantiate(abilityProjectile, hit.point, Quaternion.identity);
					}
					if (visualizationTeleporter != null) {
						Destroy(visualizationTeleporter);
					}
				}
			}
		} else {
			if (!pressed) { //Teleport To Location
				myGag.AnimSnapFinger();
				Teleport();
			}
		}
	}


	private void Teleport() {
				
		var charController = player.GetComponent<CharacterController>();
		charController.enabled = false;
		// player.GetComponent<CharacterController>().Move(placedTeleporter.transform.position - player.transform.position);
		var rawPos = placedTeleporter.transform.position;
		player.transform.position = new Vector3(rawPos.x, rawPos.y + charController.height, rawPos.z);
		charController.enabled = true;

		var destroyMe = placedTeleporter;
		placedTeleporter = null;
		Destroy(destroyMe);
	}
	

	private GameObject visualizationTeleporter;
	
	public override void AbilityUpdate() {
		//Not Used
		if (visualisationMode) {
			RaycastHit teleporterPoint;
			
			//Raycast to where we are looking and spwan teleporter visualization here
			if (Physics.Raycast(gauntlet.transform.position, gauntlet.transform.forward , out teleporterPoint, Mathf.Infinity, layerMask)) {
				if (visualizationTeleporter != null) {
					visualizationTeleporter.transform.position = teleporterPoint.point;
				} else {
					visualizationTeleporter = Instantiate(abilityProjectile, teleporterPoint.point, Quaternion.identity);
				}
			}
			
			//Control the colour to show if it can be placed or not
			if (teleporterPoint.normal.normalized == new Vector3(0, 1, 0)) { //If its a horizontal flat surface
				visualizationTeleporter.GetComponentInChildren<ParticleSystem>().startColor = Color.green;
			} else {
				visualizationTeleporter.GetComponentInChildren<ParticleSystem>().startColor = Color.red;
			}
		} else {
			if (visualizationTeleporter != null) {
				Destroy(visualizationTeleporter);
			}
		}
	} 
	
	public override void UnEquip() {
		
	}

	
}
