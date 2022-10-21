using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class Projectile_EMPGrenade : MonoBehaviour
{
	public float blastRadius;
	public int damage;
	public float explosionDelay;
	public GameObject explosionParticle;

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
		var itemsInRange = Physics.OverlapSphere(transform.position, blastRadius);
		
		foreach (var item in itemsInRange) {
			if (item.TryGetComponent(out IDamageable damageable)) {
				damageable.TakeDamage(damage);
			}
		}
		Instantiate(explosionParticle, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}

	private void OnDrawGizmos() {
		Gizmos.color = new Color(0.25f, 0.25f, 0.25f, 0.25f);
		Gizmos.DrawSphere(transform.position, blastRadius);
	}
}
