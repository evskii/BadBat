using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

using Unity.VisualScripting;

using UnityEngine;

using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;


public class Damageable_Wall : MonoBehaviour, IDamageable
{
    public int totalHealth;
    private int currentHealth;

    private void Start() {
        currentHealth = totalHealth;
    }


    public void TakeDamage(int amt) {
        currentHealth -= amt;
        if (currentHealth <= 0) {
            DestroyWall();
        }
    }

    private void DestroyWall() {
        GetComponent<Collider>().enabled = false;
        var wallPieces = GetComponentsInChildren<Transform>();
        foreach (var piece in wallPieces) {
            piece.AddComponent<Rigidbody>();
            // piece.GetComponent<Rigidbody>().AddExplosionForce(10f, Vector3.forward, 10f, 1.0f);
            var forceDir = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
            forceDir *= Random.Range(8f, 12f);
            piece.GetComponent<Rigidbody>().AddForce(forceDir, ForceMode.Impulse);
        }
    }
}
