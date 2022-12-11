using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingPlayerDamage : MonoBehaviour, IDamageable
{
    public int health;

    public void TakeDamage(int amt) {
        Debug.Log("TAKING DAMAGE");
        health -= amt;
    }
}
