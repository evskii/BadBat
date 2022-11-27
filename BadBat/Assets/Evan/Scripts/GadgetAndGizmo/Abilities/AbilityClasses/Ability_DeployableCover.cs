using System.Collections;
using System.Collections.Generic;

using Evan.Scripts.PlayerMovement;

using UnityEngine;
using UnityEngine.InputSystem;

public class Ability_DeployableCover : AbilityClass
{

	public float projectileForce; //Used to say how much force to add to the grenade

	private float lastFire;
	private float fireDelay = 2f;

	private GameObject placedCover;
	
	public override void Equip(GameObject player, GameObject gauntlet, GadgetAndGizmo myGag) {
		// Debug.Log( abilityName + " Equipped");
		//Assign reference to the player and gauntlet (this.player and this.gauntlet refer
		//to the variables made in the AbilityClass script that this derives from)
		this.player = player; 
		this.gauntlet = gauntlet;
		this.myGag = myGag;
		lastFire = 0;
	}
	
	public override void Fire(bool pressed) {
		//Basic code to spawn a projectile and fire it.
		if (pressed && !placedCover) {
			var temp = Instantiate(abilityProjectile, gauntlet.transform.position, Quaternion.identity);
			var forceDir = gauntlet.transform.rotation * new Vector3(0f, 0f, projectileForce);
			temp.GetComponent<Rigidbody>().AddRelativeForce(forceDir, ForceMode.Impulse);
			temp.transform.rotation = Quaternion.Euler(new Vector3(0, player.transform.rotation.eulerAngles.y, 0));
			lastFire = Time.time;
			myGag.AnimImmediateFire();
			placedCover = temp;
		}
	}

	public override void AbilityUpdate() {
		//Such bad code but fuckit we ball
		var temp = FindObjectOfType<SubProjectile_DeployableCover>(); 
		if (temp) {
			placedCover = temp.gameObject;
		}
	}

	public override void UnEquip() {
	}

	public override void Clear() {
		if (placedCover) {
			Destroy(placedCover);
		} else {
			var subProj = FindObjectOfType<SubProjectile_DeployableCover>();
			if (subProj) {
				Destroy(subProj.gameObject);
			}
			
		}
		
		
	}
}
