using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable_Barrel : MonoBehaviour, IDamageable
{
	public int health;
	public GameObject explosionParticle;
	public float blastRadius = 10f;

	public void TakeDamage(int amt) {
		health -= amt;

		if (health <= 0) {
			StartCoroutine(DelayDestroy());
		}
	}

	private IEnumerator DelayDestroy() {
		GetComponent<Collider>().enabled = false;
		yield return new WaitForSeconds(.05f);
		Destroy();
	}

	private void Destroy() {
		Instantiate(explosionParticle, transform.position, Quaternion.identity);
		
		var itemsInRange = Physics.OverlapSphere(transform.position, blastRadius);
		
		foreach (var item in itemsInRange) {
			if (item.TryGetComponent(out IDamageable damageable)) {
				if (item.gameObject != this.gameObject && item.gameObject.activeSelf) {
					damageable.TakeDamage(10);
				}
				
			}
		}

		Destroy(gameObject);
		
	}

	private IEnumerator DisposeGameObject() {
		gameObject.SetActive(false);
		yield return new WaitForSeconds(0.5f);
		Destroy(gameObject);
	}

	private void OnDestroy() {
		StopAllCoroutines();
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, blastRadius);
	}
}
