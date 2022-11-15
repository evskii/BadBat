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
    private FPSPlayerInput playerInput;
    public enum Arm
    {
        Left,
        Right
    }
    public Arm arm;
    
    private void Start() {
        playerInput = GetComponentInParent<FPSPlayerInput>();
        // Debug.Log(arm);
        if (arm == Arm.Left) {
            playerInput.LeftFire = Fire;
        } else {
            playerInput.RightFire = Fire;
        }
        
        player = GetComponentInParent<FPSPlayerMovementCharacterController>().gameObject;
        
        activeAbility = availableAbilities[0];
        activeAbility.Equip(player, gameObject);
        
        
    }

    private void Update() {
        if (activeAbility) {
            activeAbility.AbilityUpdate(); //Calls the update inside of the abolities [Most dont actually use this]
        }
    }

    private bool lastButtonContext = false;
    public void Fire(bool pressed) {
        if (lastButtonContext != pressed) {
            activeAbility.Fire(pressed);
            Debug.Log(pressed);
            lastButtonContext = pressed;
        }
        
    }

    public void Equip(AbilityClass ability) {
        activeAbility.UnEquip();
        activeAbility = ability;
        ability.Equip(player, gameObject);
    }
}
