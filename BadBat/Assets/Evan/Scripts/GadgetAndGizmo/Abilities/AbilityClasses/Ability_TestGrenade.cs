using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ability_TestGrenade : AbilityClass
{

	public float projectileForce; //Used to say how much force to add to the grenade
	
	public float fireRate = 1f;
	private float lastShot = 0;
	
	public override void Equip(GameObject player, GameObject gauntlet, GadgetAndGizmo myGag) {
		// Debug.Log( abilityName + " Equipped");
		//Assign reference to the player and gauntlet (this.player and this.gauntlet refer
		//to the variables made in the AbilityClass script that this derives from)
		this.player = player; 
		this.gauntlet = gauntlet;
		this.myGag = myGag;

		lastShot = 0f;
	}
	
	public override void Fire(bool pressed) {
		//Basic code to spawn a projectile and fire it.
		if (pressed && Time.time >= lastShot + fireRate) {
			var projectile = Instantiate(abilityProjectile, gauntlet.transform.position, Quaternion.identity);
			var forceDir = gauntlet.transform.rotation * new Vector3(0f, 0f, projectileForce);
			projectile.GetComponent<Rigidbody>().AddRelativeForce(forceDir, ForceMode.Impulse);
			
			myGag.AnimImmediateFire();
			lastShot = Time.time;
		}
	}
	public override void AbilityUpdate() {
		//Not Used
	}

	public override void UnEquip() {
	}
}
