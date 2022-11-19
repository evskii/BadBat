using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Death : EnemyState
{

	public override void Enter() {
		enemy.navMeshAgent.ResetPath();
		enemy.animController.SetTrigger("Death");
		enemy.enabled = false;
	}
	
	public override void Think() {
		
	}
	
	public override void Exit() {
		
	}
}
