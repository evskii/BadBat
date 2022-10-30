using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_BreachCharge : MonoBehaviour
{
	public float delayExplodeTime = 3f;
	public int damage = 10;
	private ParticleSystem particleSystem;
	private List<IDamageable> damageableObjects = new List<IDamageable>();

	private void Awake() {
		particleSystem = GetComponent<ParticleSystem>();
	}

	private void Start() {
		StartCoroutine(Explode());
	}

	private void OnTriggerEnter(Collider other) {
		if (other.TryGetComponent(out IDamageable damageable)) {
			damageableObjects.Add(damageable);
		}
	}

	private IEnumerator Explode() {
		yield return new WaitForSeconds(delayExplodeTime);
		particleSystem.Play();
		foreach (var item in damageableObjects) {
			item.TakeDamage(damage);
		}
	}
}
