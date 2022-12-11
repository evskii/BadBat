using System.Collections;
using System.Collections.Generic;

using UnityEditor.SceneManagement;

using UnityEngine;

public class EnemyState_Alert : EnemyState
{

	private EnemyState previousState;
	private Vector3 alertLocationTransform;
	private bool alertInitiated = false;

	public float searchDelay = 3f;
	private Coroutine alertSearch;

	public void InitAlert(Vector3 alertLocationTransform, EnemyState currentState) {
		this.alertLocationTransform = alertLocationTransform;
		previousState = currentState;
		alertInitiated = true;
	}
	
	public override void Enter() {
		
	}
	
	public override void Think() {
		//Error checking
		if (!alertInitiated) {
			Debug.Log("<color=blue>Enemy: " + gameObject.name + " is in State_Alert without alert being initiated! </color>");
			return;
		}

		//If we are searching for player, dont run next code
		if (alertSearch != null) {
			return;
		}
		
		enemy.navMeshAgent.SetDestination(alertLocationTransform);
		enemy.animController.SetBool("Walking", enemy.navMeshAgent.hasPath);
	
		//Slowing animation and speed when getting closer
		var distToTarget = Vector3.Distance(transform.position, alertLocationTransform);
		enemy.currentSpeedMulti = EvMath.Map(distToTarget, enemy.slowingRadius, 0, 1, 0.1f);
		enemy.currentSpeedMulti = Mathf.Clamp(enemy.currentSpeedMulti, 0, 1);
		enemy.navMeshAgent.speed = enemy.baseMoveSpeed * enemy.currentSpeedMulti;

		enemy.animController.SetFloat("WalkingAnimSpeed", enemy.currentSpeedMulti);

		if (Vector3.Distance(transform.position, alertLocationTransform) <= enemy.navMeshAgent.stoppingDistance) {
			//Start looking around then go back to what was doing
			if (alertSearch == null) {
				alertSearch = StartCoroutine(SearchDelay());
			}
		}
	}

	private IEnumerator SearchDelay() {
		enemy.animController.SetBool("Walking", false);
		enemy.animController.SetBool("Looking", true);
		enemy.navMeshAgent.ResetPath();
		
		yield return new WaitForSeconds(searchDelay);
		
		enemy.animController.SetBool("Looking", false);
		enemy.StateMachine(previousState);
	}
	
	public override void Exit() {
		previousState = null;
		alertLocationTransform = Vector3.zero;
		alertInitiated = false;
		alertSearch = null;
	}
}
