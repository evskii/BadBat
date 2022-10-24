using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEditor.UIElements;

using UnityEngine;

public class Projectile_ConcussionGrenade : MonoBehaviour
{
	public float blastRadius;
	public int damage;
	public float explosionDelay;
	public GameObject explosionParticle;
	public float concussionLength;

	// private void OnCollisionEnter(Collision other) {
	// 	Explode();
	// }

	private void Start() {
		StartCoroutine(DelayExplosion());
	}

	private IEnumerator DelayExplosion() {
		yield return new WaitForSeconds(explosionDelay);
		Explode();
	}

	private void Explode() {
		GetComponent<Rigidbody>().isKinematic = false;
		GetComponent<Collider>().enabled = false;
		
		var itemsInRange = Physics.OverlapSphere(transform.position, blastRadius).Where(obj => obj.TryGetComponent(out IConcuss concuss));
		
		foreach (var item in itemsInRange) {

			RaycastHit hit;
			if (Physics.Raycast(transform.position, item.transform.position - transform.position, out hit)) {
				
				if (hit.collider == item) {
					item.GetComponent<IConcuss>().Concuss(concussionLength);
				}
			}

		}
		// Instantiate(explosionParticle, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}

	private void OnDrawGizmos() {
		Gizmos.color = new Color(0.25f, 0.25f, 0.25f, 0.25f);
		Gizmos.DrawSphere(transform.position, blastRadius);
	}
}
