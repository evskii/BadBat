using System;
using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.AI;

using Random = UnityEngine.Random;

public class AI_EnemyBase : MonoBehaviour, IConcuss
{
	[Header("Enemy Settings")]
	[SerializeField] private float baseMoveSpeed;

	[Header("Concussion Effect Bits")]
	private bool concussed = false;
	[SerializeField] private Transform enemyHead;
	[SerializeField] private GameObject concussionParticle;
	private GameObject currentConcussionEffect;
	private Coroutine concussionRemovalCoroutine;
	
	//States and other precarious behaviours
	private Animator animController;
	public Transform primaryTarget;
	private NavMeshAgent navMeshAgent;
	[SerializeField] private float wanderDistance;

	private void Start() {
		navMeshAgent = GetComponent<NavMeshAgent>();
		animController = GetComponent<Animator>();
	}
	
	//--------------------- Actual STATE of ya ----------------------------------------------------------

	private Vector3 GetRandomNavMeshPosition() {
		Vector3 randomDirection = Random.insideUnitSphere * wanderDistance;
		
		randomDirection += transform.position;
		NavMeshHit hit;
		NavMesh.SamplePosition(randomDirection, out hit, wanderDistance, 1);
		Vector3 finalPosition = hit.position;
		return finalPosition;
	}

	private void Update() {
		Movement();
	}

	private void Movement() {
		if (primaryTarget) {
			navMeshAgent.SetDestination(primaryTarget.position);
			Debug.Log("Distance to Target: " + Vector3.Distance(transform.position, primaryTarget.position));
			if (Vector3.Distance(transform.position, primaryTarget.position) <= navMeshAgent.stoppingDistance) {
				primaryTarget.position = GetRandomNavMeshPosition();
			}
			animController.SetBool("Walking", true);
		}
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
