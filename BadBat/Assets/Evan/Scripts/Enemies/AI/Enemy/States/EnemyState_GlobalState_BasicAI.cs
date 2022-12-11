using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;

using Evan.Scripts.PlayerMovement;

using UnityEngine;

public class EnemyState_GlobalState_BasicAI : EnemyState, IAlert
{
	//This state is used as a global state that runs regardless of state machine.
	//This is used to control the "Update" of an AI without cluttering the base code

	[SerializeField] private GameObject playerReference;
	private bool hasLineOfSight;
	[SerializeField] private bool alwaysHostile; 

	public override void Enter() {
		//Get a reference to the player character in the scene
		playerReference = FindObjectOfType<FPSPlayerInput>().gameObject;
		if (!playerReference) {
			Debug.Log("<color=blue>Enemy: " + gameObject.name + " cannot retrieve a reference to Player!</color>");
		}
	}
	
	public override void Think() {
		//If we are not currently pursuing the player then we are looking for the player
		if (enemy.currentState != enemy.GetComponent<EnemyState_Pursue>() && enemy.currentState != enemy.GetComponent<EnemyState_Combat>() && alwaysHostile) {
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
				PursuePlayer(playerReference);
			}
		}
	}
	
	public void PursuePlayer(GameObject player) {
		EnemyState_Pursue pursueState = GetComponent<EnemyState_Pursue>();
		pursueState.InitPursue(player, GetComponent<EnemyState_Idle>());
		enemy.StateMachine(pursueState);
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
