using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ability_Teleporter : AbilityClass
{
	public GameObject placedTeleporter;
	public LayerMask layerMask;

	public override void Equip(GameObject player, GameObject gauntlet) {
		this.player = player; 
		this.gauntlet = gauntlet;
	}

	public override void Fire(bool pressed) {
		Debug.Log(pressed);
		if (pressed) {
			if (!placedTeleporter) {
				RaycastHit hit;
			
				if (Physics.Raycast(gauntlet.transform.position, gauntlet.transform.forward , out hit, Mathf.Infinity, layerMask)) {
					if (hit.normal.normalized == new Vector3(0, 1, 0)) { //If its a horizontal flat surface
						placedTeleporter = Instantiate(abilityProjectile, hit.point, Quaternion.identity);
					}
				
				}
			} else {
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
		}
		
	}
	public override void UnEquip() {
		
	}

	
}
