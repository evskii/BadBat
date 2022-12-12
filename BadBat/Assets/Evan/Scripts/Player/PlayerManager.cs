using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IDamageable
{
	//This may be strangely formatted code as it is split up into sections, dont hate me cause you aint me

	private void Start() {
		InitPlayer();
	}

	public void InitPlayer() {
		currentHealth = maxHealth;
	}

	//Player stats
	public int maxHealth = 10;
	public int currentHealth;
	// public int currentHealth { get; private set; }
	
	public void TakeDamage(int amt) {
		// Debug.Log(gameObject.name + " has taken damage!");
		currentHealth -= amt;

		if (currentHealth <= 0) {
			// Debug.Log(gameObject.name + " has perished...");
		}
	}
	
	//Players resources
	public int partsTotal { get; private set; }
	public int cashTotal { get; private set; }

	public void UpdatePartsTotal(int amt) {
		partsTotal -= amt;
	}

	public void UpdateCashTotal(int amt) {
		cashTotal -= amt;
	}
	
}
