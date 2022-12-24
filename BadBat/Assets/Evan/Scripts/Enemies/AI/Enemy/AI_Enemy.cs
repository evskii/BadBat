using System.Collections;
using System.Collections.Generic;

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
	public float viewingAngle = 75f;

	[Header("Enemy Health and Shit")]
	[SerializeField] private int maxHealth;
	[SerializeField] private int currentHealth;
	[SerializeField] private float hitStunDelay = 1f;
	public bool dead = false;
	private bool stunned = false;
	private Coroutine hitStun;
	
	
	//Random References
	[Header("Random References")]
	public Transform enemyHead;
	public LayerMask enemyLayer;
	public AudioSource audioSource;

	//States and other precarious behaviours
	[Header("AI and State Machine Stuff")]
	public AI_Room myRoom;
	[HideInInspector] public Animator animController;
	[HideInInspector] public NavMeshAgent navMeshAgent;

	public EnemyState globalState; //Used ontop of state machine state
	public EnemyState startingState;
	public EnemyState currentState;
	public EnemyState previousState;

	//Effects and Extra shite
	[Header("Concussion Effect Bits")]
	[SerializeField] private GameObject concussionParticle;
	private bool concussed = false;
	private GameObject currentConcussionEffect;
	private Coroutine concussionRemovalCoroutine;
	
	
	private void Start() {
		//Get References
		navMeshAgent = GetComponent<NavMeshAgent>();
		animController = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
		
		//error checking
		if (!myRoom) {
			Debug.Log("<color=cyan> No Room set on enemy: </color>" + gameObject.name);
			return;
		}
		
		if (!startingState) {
			Debug.LogError("<color=cyan> There is no starting EnemyState set for: </color>" + gameObject.name);
			enabled = false; //Disables script
		} else {
			currentState = startingState;
			currentState.Enter();
		}

		if (!globalState) {
			Debug.Log("<color=cyan> There is no global state for: </color>" + gameObject.name);
		} else {
			globalState.Enter();
		}
		
		
		//set values
		currentHealth = maxHealth;
	}
	
	//--------------------- Actual STATE of ya ----------------------------------------------------------
	
	private void Update() {
		if (!stunned) {
			currentState.Think();
		}
		
		if (globalState) {
			globalState.Think();
		}
	}

	public void StateMachine(EnemyState newState) {
		currentState.Exit();
		
		// Debug.Log("<color=red>Leaving State: </color>" + currentState);
		// Debug.Log("<color=green>Entering State: </color>" + newState);

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
		if (enabled) {
			currentHealth -= amt;
			
			// EnemyState stateToCall = currentHealth <= 0 ? GetComponent<EnemyState_Death>() : GetComponent<EnemyState_Hit>();
			// StateMachine(stateToCall);

			if (currentHealth <= 0) {
				StateMachine(GetComponent<EnemyState_Death>());
				dead = true;
			}

			if (hitStun != null) {
				StopCoroutine(hitStun);
			}
			hitStun = StartCoroutine(HitStun());
			
		}
	}

	private IEnumerator HitStun() {
		stunned = true;
		animController.SetTrigger("Hit");
		navMeshAgent.isStopped = true;
		
		yield return new WaitForSeconds(hitStunDelay);
		
		navMeshAgent.isStopped = false;
		stunned = false;
	}

	public void PlaySFX(AudioClip toPlay) {
		audioSource.clip = toPlay;
		audioSource.Play();
	}
	
	//--------------------- Handy AI Methods Effect --------------------------------------------------------------
	public bool InsideViewingAngle(Vector3 position) {
		float angle = Vector3.Angle(transform.forward, position - transform.position);
		return angle <= viewingAngle;
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
