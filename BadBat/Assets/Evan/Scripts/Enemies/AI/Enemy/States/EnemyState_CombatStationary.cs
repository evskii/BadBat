using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using UnityEngine;

using Random = UnityEngine.Random;

public class EnemyState_CombatStationary : EnemyState
{
	//Use a range of distances so when the player moves around they dont automatically lose connection
	public float minCombatDistance;
	public float maxCombatDistance;
	private float myCombatDistance;
	private bool inCombatRange;
	
	private EnemyState previousState;
	private GameObject combatGameObject;
	private Vector3 combatLocation;
	private bool combatInitiated;
	private bool hasLineOfSight;

	// public float searchingViewAngleMulti;
	// private float normalViewingAngle;
	// private Coroutine searchDelay;

	//Weapon Variables
	private float lastShotFired = 0f;
	public float shotDelay;
	public float reloadTime;
	public int damage;
	[Range(0,100)]public float accuracy;
	public int maxAmmo;
	public int currentAmmo;
	private bool reloading;
	private Coroutine reloadCoroutine;
	public AudioClip gunshotSFX;
	public bool debugVisuals;
	
	public void InitCombat(GameObject combatGameObject, EnemyState currentState) {
		this.combatGameObject = combatGameObject;
		combatLocation = combatGameObject.transform.position;
		previousState = currentState;
		combatInitiated = true;
		Debug.Log("Combat State Entered Successfully");
	}
	
	public override void Enter() {
		myCombatDistance = Random.Range(minCombatDistance, maxCombatDistance);

		currentAmmo = maxAmmo;
	}
	
	public override void Think() {
		//Error checking
		if (!combatInitiated) {
			Debug.Log("<color=cyan>Enemy: " + gameObject.name + " is in State_Combat without combat being initiated! </color>");
			return;
		}

		if (!reloading) {
			//Check for Line of Sight
			RaycastHit hit;
			Vector3 raycastDir = combatGameObject.transform.position - enemy.enemyHead.position;
			if (Physics.Raycast(enemy.enemyHead.position, raycastDir, out hit, Mathf.Infinity, ~enemy.enemyLayer)) {
				hasLineOfSight = hit.transform == combatGameObject.transform; //LineOfSight bool
				// Debug.Log("Has LOS: " + hasLineOfSight);
			
				//If we have line of sight then constantly point towards player
				if (hasLineOfSight) {
					var lookDir = new Vector3(combatGameObject.transform.position.x, transform.position.y, combatGameObject.transform.position.z);
					transform.LookAt(lookDir);
				}
				combatLocation = hasLineOfSight ? combatGameObject.transform.position : combatLocation; //if LOS set location, otherwise use current location
			}
		
			//If we are in the combat range and we can see the player
			inCombatRange = Vector3.Distance(transform.position, combatLocation) <= myCombatDistance && hasLineOfSight;
			// Debug.Log("In combat range: " + inCombatRange);
			if (inCombatRange && hasLineOfSight) {
				//Start shooting
				if (Time.time >= (lastShotFired + shotDelay) * Random.Range(1, 2) && currentAmmo > 0) { //If shotDelay has passed we can shoot again
					//Shoot
					Shoot();
				}
			} 

		}
		
		
	}

	private void Shoot() {
		// Debug.Log("Shoot Called");
		//Look him in the fucking eyes before you start shooting you coward
		var lookDir = new Vector3(combatGameObject.transform.position.x, transform.position.y, combatGameObject.transform.position.z);
		transform.LookAt(lookDir);
		
		enemy.animController.SetTrigger("Shoot");
		enemy.PlaySFX(gunshotSFX);
		
		lastShotFired = Time.time;
		currentAmmo--;

		bool shotHit = Random.Range(0, 100) <= accuracy;
		if (shotHit) {
			// Debug.Log("Shot Hit");
			combatGameObject.GetComponent<IDamageable>().TakeDamage(damage);
		} else {
			// Debug.Log("Shot Missed");
		}


		if (currentAmmo <= 0) {
			//RELOAD
			reloadCoroutine = StartCoroutine(ReloadGun());
		}
	}

	private IEnumerator ReloadGun() {
		reloading = true;
		enemy.animController.SetTrigger("Reload");

		yield return new WaitForSeconds(reloadTime);

		currentAmmo = maxAmmo;
		reloading = false;
	}
	
	public override void Exit() {
		if (reloadCoroutine != null) {
			StopCoroutine(reloadCoroutine);
		}
		
		reloadCoroutine = null;
		previousState = null;
		combatLocation = Vector3.zero;
		combatInitiated = false;
	}

	private void OnDrawGizmos() {
		if (debugVisuals) {
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireSphere(transform.position, minCombatDistance);
			Gizmos.color = Color.magenta;
			Gizmos.DrawWireSphere(transform.position, maxCombatDistance);
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(transform.position, myCombatDistance);
		}
	}
}
