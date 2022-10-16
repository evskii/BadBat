using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Teleporter : AbilityClass
{
	public GameObject placedTeleporter;
	public LayerMask layerMask;

	public override void Equip(GameObject player, GameObject gauntlet) {
		this.player = player; 
		this.gauntlet = gauntlet;
	}

	public override void Fire() {
		
		if (!placedTeleporter) {
			RaycastHit hit;
			
			if (Physics.Raycast(gauntlet.transform.position, gauntlet.transform.forward , out hit, Mathf.Infinity, layerMask)) {
				if (hit.normal.normalized == new Vector3(0, 1, 0)) { //If its a horizontal flat surface
					placedTeleporter = Instantiate(abilityProjectile, hit.point, Quaternion.identity);
				}
				
			}
		} else {
			player.GetComponent<CharacterController>().Move(placedTeleporter.transform.position - player.transform.position);
			var destroyMe = placedTeleporter;
			placedTeleporter = null;
			Destroy(destroyMe);
		}
	}
	public override void UnEquip() {
		throw new System.NotImplementedException();
	}

	
}
