using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class EnemyPlaytesting : MonoBehaviour, IConcuss
{

	[Header("Concussion Effect Bits")]
	public bool concussed = false;
	public Transform enemyHead;
	public GameObject concussionParticle;
	private GameObject currentConcussionEffect;
	private Coroutine concussionRemovalCoroutine;
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
