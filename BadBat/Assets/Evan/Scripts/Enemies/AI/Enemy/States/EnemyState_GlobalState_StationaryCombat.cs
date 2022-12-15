using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;

using Evan.Scripts.PlayerMovement;

using UnityEngine;

public class EnemyState_GlobalState_StationaryCombat: EnemyState, IAlert
{
	//This state is used as a global state that runs regardless of state machine.
	//This is used to control the "Update" of an AI without cluttering the base code

	[SerializeField] private GameObject playerReference;
	[SerializeField] private bool hasLineOfSight;
	[SerializeField] private bool alwaysHostile; 

	public override void Enter() {
		//Get a reference to the player character in the scene
		playerReference = FindObjectOfType<FPSPlayerInput>().gameObject;
		if (!playerReference) {
			Debug.Log("<color=blue>Enemy: " + gameObject.name + " cannot retrieve a reference to Player!</color>");
		}
	}
	
	public override void Think() {
		//We use the below bool to show when we can call WatchForPlayer [Basically what states transition into puruse]
		bool stateCheck = enemy.currentState == enemy.GetComponent<EnemyState_Idle>() || enemy.currentState == enemy.GetComponent<EnemyState_Patrol>();
		if (stateCheck && alwaysHostile) {
			WatchForPlayer();
		}
	}
	
	public override void Exit() { }
	
	public void Alert(Vector3 alertLocationTransform) {
		Debug.Log( transform.name + " AT ALERT");
		EnemyState_Alert alertState = GetComponent<EnemyState_Alert>();
		alertState.InitAlert(alertLocationTransform, enemy.currentState);
		enemy.StateMachine(alertState);
	}

	public void WatchForPlayer() {
		//Check for Line of Sight
		RaycastHit hit;
		Vector3 raycastDir = playerReference.transform.position - enemy.enemyHead.position;
		if (Physics.Raycast(enemy.enemyHead.position, raycastDir, out hit, Mathf.Infinity, ~enemy.enemyLayer)) {
			hasLineOfSight = hit.transform == playerReference.transform && enemy.InsideViewingAngle(hit.transform.position); //LineOfSight bool
			if (hasLineOfSight) {
				CombatPlayer(playerReference);
			}
		}
	}
	
	public void CombatPlayer(GameObject player) {
		EnemyState_CombatStationary combatState = GetComponent<EnemyState_CombatStationary>();
		combatState.InitCombat(player, GetComponent<EnemyState_Idle>());
		enemy.StateMachine(combatState);
	}



	private void OnDrawGizmos() {
		if (enemy) {
			Gizmos.color = Color.cyan;
			Quaternion rightRot = Quaternion.AngleAxis(enemy.viewingAngle, Vector3.up);
			Vector3 rightDir = rightRot * transform.forward * 10;
			Quaternion leftRot = Quaternion.AngleAxis(-enemy.viewingAngle, Vector3.up);
			Vector3 leftDir = leftRot * transform.forward * 10;
			Gizmos.DrawRay(enemy.enemyHead.position, rightDir);
			Gizmos.DrawRay(enemy.enemyHead.position, leftDir);
		}
	}
}
