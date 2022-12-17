using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;

using UnityEngine;
using UnityEngine.InputSystem;

public class Ability_Gun : AbilityClass
{

	public float gunRange;
	public int gunDamage;
	public float fireRate;
	private float lastShot = 0;

	public GameObject gunShotParticle;
	public GameObject bulletHitParticle;
	public GameObject bulletHoleQuad;

	public override void Equip(GameObject player, GameObject gauntlet, GadgetAndGizmo myGag) {
		this.player = player;
		this.gauntlet = gauntlet;

		this.myGag = myGag;
		this.myGag.AnimGunEquipped(true);
		lastShot = 0;
	}
	public override void Fire(bool pressed) {
		if (pressed && Time.time >= lastShot + fireRate) {
			RaycastHit hit;
			//Debug.DrawRay(gauntlet.transform.position, gauntlet.transform.forward, Color.red, gunRange);
		
			myGag.AnimFingerGunFire();

			AudioClip toPlay = sfxClips[Random.Range(0, sfxClips.Length)];
			myGag.PlaySFX(toPlay);

			Instantiate(gunShotParticle, myGag.indexFingerTip);

			if (Physics.Raycast(gauntlet.transform.position, gauntlet.transform.forward , out hit, gunRange)) {
				// Debug.Log(hit.collider.gameObject.name);
				if (hit.collider.TryGetComponent(out IDamageable damageable)) {
					damageable.TakeDamage(gunDamage);
				} else {
					var bulletHole = Instantiate(bulletHoleQuad, hit.point, Quaternion.LookRotation(hit.normal.normalized) * Quaternion.Euler(0, 180f, 0));
					bulletHole.transform.position -= bulletHole.transform.forward / 1000;
				}
				Instantiate(bulletHitParticle, hit.point, Quaternion.LookRotation(hit.normal.normalized));
			}

			lastShot = Time.time;
		}
		
	}
	
	public override void AbilityUpdate() {
		//Not Used
	}
	
	public override void UnEquip() {
		myGag.AnimGunEquipped(false);
	}
	
}
