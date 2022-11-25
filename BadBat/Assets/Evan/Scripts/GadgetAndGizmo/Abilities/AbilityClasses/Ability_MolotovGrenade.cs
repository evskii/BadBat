using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_MolotovGrenade : AbilityClass
{
	
	public float projectileForce; 

	public override void Equip(GameObject player, GameObject gauntlet, GadgetAndGizmo myGag) {
		this.player = player;
		this.gauntlet = gauntlet;
		this.myGag = myGag;
	}
	public override void Fire(bool pressed) {
		if (pressed) {
			var projectile = Instantiate(abilityProjectile, gauntlet.transform.position, Quaternion.identity);
			var forceDir = gauntlet.transform.rotation * new Vector3(0f, 0f, projectileForce);
			projectile.GetComponent<Rigidbody>().AddRelativeForce(forceDir, ForceMode.Impulse);
			
			myGag.AnimImmediateFire();
		}
	}
	
	public override void AbilityUpdate() {
		//Not Used
	}
	
	public override void UnEquip() {
		
	}

}
