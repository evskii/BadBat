using System.Collections;
using System.Collections.Generic;

using UnityEditor.Experimental.GraphView;

using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider), typeof(NavMeshAgent), typeof(EnemyState_Hit))]
[RequireComponent(typeof(EnemyState_Death))]
public class AI_Enemy : MonoBehaviour, IDamageable
{
    //  This script is the base script used for enemy AI. It will feed down to other 
    //  enemy type scripts. This will just be a state machine controller/stats base stats holder
    //  for the enemies in the game.
    
    //Base settings for the enemy
    [Header("Enemy Settings")]
    public float baseMoveSpeed;
	[HideInInspector] public float currentSpeedMulti;
	public float slowingRadius;

	[Header("Enemy Health and Shit")]
	[SerializeField] private int maxHealth;
	[SerializeField] private int currentHealth;
	

	//States and other precarious behaviours
	[HideInInspector] public Animator animController;
	[HideInInspector] public NavMeshAgent navMeshAgent;
	public AI_Room myRoom;
	
	public EnemyState startingState;
	[SerializeField] private EnemyState currentState;
	public EnemyState previousState;

	//Effects and Extra shite
	[Header("Concussion Effect Bits")]
	[SerializeField] private Transform enemyHead;
	[SerializeField] private GameObject concussionParticle;
	private bool concussed = false;
	private GameObject currentConcussionEffect;
	private Coroutine concussionRemovalCoroutine;
	
	
	private void Start() {
		//Get References
		navMeshAgent = GetComponent<NavMeshAgent>();
		animController = GetComponent<Animator>();

		//error checker for start state
		if (startingState == null) {
			Debug.LogError("There is no starting EnemyState set for: " + gameObject.name);
			enabled = false; //Disables script
		} else {
			currentState = startingState;
			currentState.Enter();
		}
		
		//set values
		currentHealth = maxHealth;
	}
	
	//--------------------- Actual STATE of ya ----------------------------------------------------------
	
	private void Update() {
		currentState.Think();
	}

	public void StateMachine(EnemyState newState) {
		currentState.Exit();

		previousState = currentState;
		currentState = newState;
		
		currentState.Enter();
	}
	
	//---------------------- Outside Factors [Not directly states] --------------------------------------

	[ContextMenu("Test Take Damage")]
	private void TakeDamageTest() {
		TakeDamage(1);
	}
	
	public void TakeDamage(int amt) {
		currentHealth -= amt;
		EnemyState stateToCall = currentHealth <= 0 ? GetComponent<EnemyState_Death>() : GetComponent<EnemyState_Hit>();
		StateMachine(stateToCall);
	}
	

	//--------------------- Concussion Effect --------------------------------------------------------------
	public void Concuss(float length) {
		concussed = true;
		if (currentConcussionEffect == null) {
			currentConcussionEffect = Instantiate(concussionParticle, enemyHead.position, Quaternion.identity, enemyHead);
			concussionRemovalCoroutine = StartCoroutine(ConcussionRemove(length));
		} else {
			StopCoroutine(concussionRemovalCoroutine);
			concussionRemovalCoroutine = StartCoroutine(ConcussionRemove(length));
		}
	}

	private IEnumerator ConcussionRemove(float length) {
		yield return new WaitForSeconds(length);
		concussed = false;
		Destroy(currentConcussionEffect);
	}
	
}
