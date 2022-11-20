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
		var coverInScene = FindObjectsOfType<SubProjectile_DeployableCover>();
		
		//Basic code to spawn a projectile and fire it.
		if (pressed && coverInScene.Length == 0 && Time.time > lastFire + fireDelay) {
			var projectile = Instantiate(abilityProjectile, gauntlet.transform.position, Quaternion.identity);
			var forceDir = gauntlet.transform.rotation * new Vector3(0f, 0f, projectileForce);
			projectile.GetComponent<Rigidbody>().AddRelativeForce(forceDir, ForceMode.Impulse);
			projectile.transform.rotation = Quaternion.Euler(new Vector3(0, player.transform.rotation.eulerAngles.y, 0));
			lastFire = Time.time;
			myGag.AnimImmediateFire();
		}
	}

	public override void AbilityUpdate() {
		//Not Used
	}

	public override void UnEquip() {
	}
}
