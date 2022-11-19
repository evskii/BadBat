using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{
    [HideInInspector]
    public AI_Enemy enemy;

    public void Awake()
    {
        enemy = GetComponent<AI_Enemy>();
    }
    
    public abstract void Enter();

    public abstract void Think();

    public abstract void Exit();
}
