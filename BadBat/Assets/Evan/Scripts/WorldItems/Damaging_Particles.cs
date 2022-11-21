using System;
using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;

public class Damaging_Particles : MonoBehaviour
{
    private List<IDamageable> itemsToDamage = new List<IDamageable>();
    public float damageRate;
    public int damage;
    private void OnParticleCollision(GameObject other) {
        Debug.Log(other.name);
        // Debug.Log("PARTICLE COLLISION" + other.name);
        if (other.TryGetComponent(out IDamageable damageable)) {
            if (!itemsToDamage.Contains(damageable)) {
                itemsToDamage.Add(damageable);
            }
        }
    }
    

    private void Start() {
        StartCoroutine(DamageTick());
    }

    private IEnumerator DamageTick() {
        if (itemsToDamage.Count > 0) {
            foreach (var item in itemsToDamage) {
                if (!item.IsUnityNull()) {
                    item.TakeDamage(damage);
                }
                
            }
        }
        // itemsToDamage.Clear();
        yield return new WaitForSeconds(damageRate);
        StartCoroutine(DamageTick());
    }

    private void OnDestroy() {
        StopAllCoroutines();
    }
}
