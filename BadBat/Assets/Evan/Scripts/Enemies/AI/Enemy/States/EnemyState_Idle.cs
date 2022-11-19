using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Idle : EnemyState
{
    
    public override void Enter() {
        enemy.animController.SetBool("Walking", false);
    }
    
    public override void Think() {
        
    }
    
    public override void Exit() {
        
    }
}
