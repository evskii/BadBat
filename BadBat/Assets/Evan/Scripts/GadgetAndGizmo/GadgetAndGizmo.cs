using System;
using System.Collections;
using System.Collections.Generic;

using Evan.Scripts.PlayerMovement;

using UnityEngine;

public class GadgetAndGizmo : MonoBehaviour
{
    public List<AbilityClass> availableAbilities = new List<AbilityClass>();
    public AbilityClass activeAbility;
    public GameObject player;
    
    public enum Arm
    {
        Left,
        Right
    }
    public Arm arm;
    
    private void Start() {
        player = GetComponentInParent<FPSPlayerMovementCharacterController>().gameObject;
        
        activeAbility = availableAbilities[0];
        activeAbility.Equip(player, gameObject);
    }

    public void Fire() {
        activeAbility.Fire();
    }

    public void Equip(AbilityClass ability) {
        activeAbility = ability;
        ability.Equip(player, gameObject);
    }
}
