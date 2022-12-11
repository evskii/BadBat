using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_GlobalState_PlacidIdle: EnemyState
{
	//This state is used as a global state that runs regardless of state machine.
	//This is used to control the "Update" of an AI without cluttering the base code
	
	public override void Enter() { }
	
	public override void Think() {
		
	}
	
	public override void Exit() { }
	
	
}
