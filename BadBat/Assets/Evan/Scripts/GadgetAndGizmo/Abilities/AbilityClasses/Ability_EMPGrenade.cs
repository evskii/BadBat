using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ability_EMPGrenade : AbilityClass
{

	public float projectileForce; //Used to say how much force to add to the grenade
	
	public override void Equip(GameObject player, GameObject gauntlet, GadgetAndGizmo myGag) {
		Debug.Log( abilityName + " Equipped");
		//Assign reference to the player and gauntlet (this.player and this.gauntlet refer
		//to the variables made in the AbilityClass script that this derives from)
		this.player = player; 
		this.gauntlet = gauntlet;
		this.myGag = myGag;
	}
	
	public override void Fire(bool pressed) {
		if (pressed) {
			//Basic code to spawn a projectile and fire it.
			var projectile = Instantiate(abilityProjectile, gauntlet.transform.position, Quaternion.identity);
			var forceDir = gauntlet.transform.rotation * new Vector3(0f, 0f, projectileForce);
			projectile.GetComponent<Rigidbody>().AddRelativeForce(forceDir, ForceMode.Impulse);

			//Animation Code
			myGag.AnimImmediateFire();
		}
		
	}
	
	public override void AbilityUpdate() {
		//Not Used
	}
	
	public override void UnEquip() {
	}
}
