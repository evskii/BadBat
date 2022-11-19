using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyState_Idle))]
public class EnemyState_Patrol : EnemyState
{
	public bool followPath;
	public AI_Path path;

	private Vector3 destinationTarget;
	private Coroutine shortIdleCoroutine;

	public override void Enter() {
		if (followPath && !path) {
			Debug.LogError("No path assigned to " + gameObject.name + " but follow path is active!");
		}
		
		destinationTarget = followPath ? path.pathPoints[path.currentPathPointIndex].position : enemy.myRoom.GetRandomPositionInRoom(true);
	}
	
	public override void Think() {
		enemy.animController.SetBool("Walking", enemy.navMeshAgent.hasPath);

		if (shortIdleCoroutine == null) {
			enemy.navMeshAgent.SetDestination(destinationTarget);
			// Debug.Log("Distance to Target: " + Vector3.Distance(transform.position, primaryTarget.position));

			var distToTarget = Vector3.Distance(transform.position, destinationTarget);
			enemy.currentSpeedMulti = EvMath.Map(distToTarget, enemy.slowingRadius, 0, 1, 0.1f);
			enemy.currentSpeedMulti = Mathf.Clamp(enemy.currentSpeedMulti, 0, 1);
			enemy.navMeshAgent.speed = enemy.baseMoveSpeed * enemy.currentSpeedMulti;

			enemy.animController.SetFloat("WalkingAnimSpeed", enemy.currentSpeedMulti);

			if (Vector3.Distance(transform.position, destinationTarget) <= enemy.navMeshAgent.stoppingDistance) {
				shortIdleCoroutine = StartCoroutine(ShortIdle());
			}
		}

	}
	
	private IEnumerator ShortIdle() {
		enemy.navMeshAgent.ResetPath();
		yield return new WaitForSeconds(Random.Range(1.5f, 3f));

		var lastDestinationTarget = destinationTarget;
		destinationTarget = followPath? path.GetNextPathPoint() : enemy.myRoom.GetRandomPositionInRoom(true);

		if (destinationTarget == lastDestinationTarget) {
			//Switch to full idle state
			enemy.StateMachine(GetComponent<EnemyState_Idle>());
		}
		
		shortIdleCoroutine = null;
	}
	
	public override void Exit() {
		enemy.navMeshAgent.ResetPath();
		enemy.animController.SetBool("Walking", false);
	}
}
