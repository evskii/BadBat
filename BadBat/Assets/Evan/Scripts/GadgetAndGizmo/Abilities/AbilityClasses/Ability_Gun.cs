using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ability_Gun : AbilityClass
{

	public float gunRange;
	public int gunDamage;
	
	public override void Equip(GameObject player, GameObject gauntlet) {
		this.player = player;
		this.gauntlet = gauntlet;
	}
	public override void Fire(bool pressed) {
		if (pressed) {
			RaycastHit hit;
			//Debug.DrawRay(gauntlet.transform.position, gauntlet.transform.forward, Color.red, gunRange);
		
			if (Physics.Raycast(gauntlet.transform.position, gauntlet.transform.forward , out hit, gunRange)) {
				// Debug.Log(hit.collider.gameObject.name);
				if (hit.collider.TryGetComponent(out IDamageable damageable)) {
					damageable.TakeDamage(gunDamage);
				}
			}
		}
		
	}
	
	public override void AbilityUpdate() {
		//Not Used
	}
	
	public override void UnEquip() {
	}
}
