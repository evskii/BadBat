using System;
using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.AI;

using Random = UnityEngine.Random;

public class AI_EnemyTest : MonoBehaviour, IConcuss
{
	[Header("Enemy Settings")]
	[SerializeField] private AI_Room myRoom;
	[SerializeField] private float baseMoveSpeed;
	private float currentSpeedMulti;
	[SerializeField] private float slowingRadius;

	[Header("Concussion Effect Bits")]
	[SerializeField] private Transform enemyHead;
	[SerializeField] private GameObject concussionParticle;
	private bool concussed = false;
	private GameObject currentConcussionEffect;
	private Coroutine concussionRemovalCoroutine;
	
	//States and other precarious behaviours
	private Animator animController;
	public Transform primaryTarget;
	private NavMeshAgent navMeshAgent;
	private Coroutine shortIdleCoroutine;

	private void Start() {
		//Get References
		navMeshAgent = GetComponent<NavMeshAgent>();
		animController = GetComponent<Animator>();
		
		
	}
	
	//--------------------- Actual STATE of ya ----------------------------------------------------------
	

	private void Update() {
		Movement();
	}

	private void Movement() {
		animController.SetBool("Walking", navMeshAgent.hasPath);
		if (primaryTarget) {
			if (shortIdleCoroutine == null) {
				navMeshAgent.SetDestination(primaryTarget.position);
				// Debug.Log("Distance to Target: " + Vector3.Distance(transform.position, primaryTarget.position));
				
				var distToTarget = Vector3.Distance(transform.position, primaryTarget.position);
				currentSpeedMulti = EvMath.Map(distToTarget, slowingRadius, 0, 1, 0.1f);
				currentSpeedMulti = Mathf.Clamp(currentSpeedMulti, 0, 1);
				navMeshAgent.speed = baseMoveSpeed * currentSpeedMulti;
				
				animController.SetFloat("WalkingAnimSpeed", currentSpeedMulti);
				
				if (Vector3.Distance(transform.position, primaryTarget.position) <= navMeshAgent.stoppingDistance) {
					shortIdleCoroutine = StartCoroutine(ShortIdle());
				}
			}
			
		}
	}

	private IEnumerator ShortIdle() {
		navMeshAgent.ResetPath();
		yield return new WaitForSeconds(Random.Range(1.5f, 3f));
		primaryTarget.position = myRoom.GetRandomPositionInRoom(true);
		shortIdleCoroutine = null;
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
