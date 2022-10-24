using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class AbilityClass : MonoBehaviour
{ 
	public enum AbilityType
	{
		Tactical,
		Offensive
	}
	
	public string abilityName;
	public AbilityType abilityType;
	public GameObject abilityProjectile;
	[HideInInspector] public GameObject player;
	[HideInInspector] public GameObject gauntlet;

	public abstract void Equip(GameObject player, GameObject gauntlet);

	public abstract void Fire(bool pressed);

	public abstract void AbilityUpdate();

	public abstract void UnEquip();
}
