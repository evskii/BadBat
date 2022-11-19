using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Hit : EnemyState
{

	
	public override void Enter() {
		enemy.animController.SetTrigger("Hit");
		StartCoroutine(ReturnToPreviousState(enemy.animController.GetCurrentAnimatorStateInfo(0).length));
	}
	
	public override void Think() {
		
	}

	private IEnumerator ReturnToPreviousState(float delay) {
		yield return new WaitForSeconds(delay);
		enemy.StateMachine(enemy.previousState);
	}
	
	public override void Exit() {
		
	}
}
