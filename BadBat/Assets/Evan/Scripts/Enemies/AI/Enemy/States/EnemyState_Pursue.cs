using System;
using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(EnemyState_Combat))]
public class EnemyState_Pursue : EnemyState
{

	private EnemyState previousState;
	private GameObject pursueGameObject;
	private Vector3 pursueLocation;
	private bool pursueInitiated;
	private bool hasLineOfSight;

	public float searchingViewAngleMulti;
	private float normalViewingAngle;
	private Coroutine searchDelay;
	
	private float combatRange = 10f;
	public float pursueWaitDelay = 5f;

	public bool debugVisuals;

	public void InitPursue(GameObject pursueGameObject, EnemyState currentState) {
		this.pursueGameObject = pursueGameObject;
		pursueLocation = pursueGameObject.transform.position;
		previousState = currentState;
		pursueInitiated = true;
		Debug.Log("Pursue State Entered Successfully");
	}
	
	public override void Enter() {
		normalViewingAngle = enemy.viewingAngle;
		combatRange = enemy.GetComponent<EnemyState_Combat>().maxCombatDistance;
	}
	
	public override void Think() {
		//Error checking
		if (!pursueInitiated) {
			Debug.Log("<color=cyan>Enemy: " + gameObject.name + " is in State_Pursue without pursue being initiated! </color>");
			return;
		}
		
		//Check for Line of Sight
		RaycastHit hit;
		Vector3 raycastDir = pursueGameObject.transform.position - enemy.enemyHead.position;
		if (Physics.Raycast(enemy.enemyHead.position, raycastDir, out hit, Mathf.Infinity, ~enemy.enemyLayer)) {
			// Debug.Log(hit.transform.name);

			if (searchDelay != null) { //If we are in the "searching" phase then use viewing angle
				hasLineOfSight = hit.transform == pursueGameObject.transform && enemy.InsideViewingAngle(hit.transform.position);
			} else {
				hasLineOfSight = hit.transform == pursueGameObject.transform; //LineOfSight bool
			}
			
			
			pursueLocation = hasLineOfSight ? pursueGameObject.transform.position : pursueLocation; //if LOS set location, otherwise use current location

			//If we are currently searching and see player, stop searching
			if (searchDelay != null && hasLineOfSight) {
				// Debug.Log("Stopping Searching as player has been found");
				CancelSearchDelay(false);
			}
		}

		// Debug.Log("Has LOS: " + hasLineOfSight);

		if (searchDelay == null) {
			
			if (enemy.navMeshAgent.destination != pursueLocation) {
				// Debug.Log("Setting new navmesh destination");
				enemy.navMeshAgent.SetDestination(pursueLocation);
			}

			//Slowing animation and speed when getting closer
			var distToTarget = Vector3.Distance(transform.position, pursueLocation);
			enemy.currentSpeedMulti = EvMath.Map(distToTarget, enemy.slowingRadius, 0, 1, 0.1f);
			enemy.currentSpeedMulti = Mathf.Clamp(enemy.currentSpeedMulti, 0, 1);
			enemy.navMeshAgent.speed = enemy.baseMoveSpeed * enemy.currentSpeedMulti;
			enemy.animController.SetFloat("WalkingAnimSpeed", enemy.currentSpeedMulti);

			//If we are stopping and cant see player
			// Debug.Log(Vector3.Distance(transform.position, pursueLocation));
			if (Vector3.Distance(transform.position, pursueLocation) <= enemy.navMeshAgent.stoppingDistance * 2 && !hasLineOfSight) {
				//Start looking around then go back to what was doing
				// Debug.Log("Stopping as cannot see player D:");
				searchDelay = StartCoroutine(SearchDelay());
			}
		}
		

		//If we are in the combat range and we can see the player
		if (Vector3.Distance(transform.position, pursueLocation) <= combatRange && hasLineOfSight) {
			//Start combat phase
			EnemyState_Combat combatState = GetComponent<EnemyState_Combat>();
			combatState.InitCombat(pursueGameObject, GetComponent<EnemyState_Pursue>());
			enemy.StateMachine(combatState);
		}
		
		enemy.animController.SetBool("Walking", enemy.navMeshAgent.hasPath);
	}

	//Call this for when we cannot see player and we are looking for them
	 private IEnumerator SearchDelay() {
		 // Debug.Log("Search Delay Began");
		 enemy.animController.SetBool("Walking", false);
		 enemy.animController.SetBool("Looking", true);
		 enemy.navMeshAgent.ResetPath();

		 normalViewingAngle = enemy.viewingAngle;
		 enemy.viewingAngle = normalViewingAngle * searchingViewAngleMulti;
		 
		 yield return new WaitForSeconds(pursueWaitDelay);
		 
		 enemy.animController.SetBool("Looking", false);
		 
		 enemy.viewingAngle = normalViewingAngle;
		 
		 enemy.StateMachine(previousState);
		 // Debug.Log("Search Delay Ended");
		 searchDelay = null;
	 }

	 private void CancelSearchDelay(bool leaveState) {
		 StopCoroutine(searchDelay);
		 searchDelay = null;
		 enemy.viewingAngle = normalViewingAngle;
		 enemy.animController.SetBool("Looking", false);
		 if (leaveState) {
			 enemy.StateMachine(previousState);
		 }
	 }
	
	public override void Exit() {
		previousState = null;
		pursueLocation = Vector3.zero;
		pursueInitiated = false;
		searchDelay = null;
		// Debug.Log("Pursue State Left Successfully");
	}

	private void OnDrawGizmos() {
		if (debugVisuals) {
			if (pursueGameObject) {
            	Gizmos.color = hasLineOfSight ? Color.green : Color.red;
            	// Gizmos.DrawLine(enemy.enemyHead.position, pursueGameObject.transform.position);
            	var dir = pursueGameObject.transform.position - enemy.enemyHead.position;
            	Gizmos.DrawRay(enemy.enemyHead.position, dir * 10);
            }
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, combatRange);
		}
		
	}
}
