using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubProjectile_DeployableCover : MonoBehaviour, IDamageable
{
    [SerializeField] private float deploySpeed;
    [SerializeField] private int maxHealth;
    private int health;

    [SerializeField] private GameObject destroyParticle;
    private void Start() {
        health = maxHealth;
        transform.localScale = new Vector3(transform.localScale.x, 0, transform.localScale.z);
        // transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.y, 0));
    }

    private void Update() {
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(transform.localScale.x, 1, transform.localScale.z), deploySpeed * Time.deltaTime);
    }
    
    public void TakeDamage(int amt) {
        health -= amt;
        if (health <= 0) {
            DestroyCover();
        }
    }

    private void DestroyCover() {
        Instantiate(destroyParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
