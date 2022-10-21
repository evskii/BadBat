using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electrical_ElectricBox : MonoBehaviour, IElectrical
{

    public GameObject explosionParticle;
    public int damage;
    public float blastRadius;
    
    public void Surge() {
        StartCoroutine(DelaySurge());
    }


    private IEnumerator DelaySurge() {
        yield return new WaitForSeconds(0.15f);
        var itemsInRange = Physics.OverlapSphere(transform.position, blastRadius);
		
        foreach (var item in itemsInRange) {
            if (item.TryGetComponent(out IDamageable damageable)) {
                damageable.TakeDamage(damage);
            }
        }
        Instantiate(explosionParticle, transform.position, Quaternion.identity);
        // Destroy(gameObject);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, blastRadius);
    }
}
