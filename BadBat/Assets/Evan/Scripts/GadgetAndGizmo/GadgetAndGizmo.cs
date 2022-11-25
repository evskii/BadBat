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

    public Animator animController;

    private void Start() {
        animController = GetComponentInChildren<Animator>();
        if (!animController) {
            Debug.LogError("Cannot get Animoator for: " + gameObject.name);
        }
        
        playerInput = GetComponentInParent<FPSPlayerInput>();
        // Debug.Log(arm);
        if (arm == Arm.Left) {
            playerInput.LeftFire = Fire;
            playerInput.ClearAbilityLeft = Clear;
        } else {
            playerInput.RightFire = Fire;
            playerInput.ClearAbilityRight = Clear;
        }
        
        
        player = GetComponentInParent<FPSPlayerMovementCharacterController>().gameObject;
        
        activeAbility = availableAbilities[0];
        activeAbility.Equip(player, gameObject, this);
        
        
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
            lastButtonContext = pressed;
        }
    }

    public void Clear(bool pressed) {
        if (pressed) {
            var lastEquipped = FindObjectOfType<RadialWeaponWheel>().lastEquippedAbility;
            if (lastEquipped == activeAbility) {
                lastEquipped.Clear();
            }
        }
    }

    public void AnimWindUp() {
        animController.SetTrigger("Windup");
    }

    public void AnimFire() {
        animController.SetTrigger("Fire");
    }

    public void AnimImmediateFire() {
        animController.SetTrigger("ImmediateFire");
    }

    public void AnimGunEquipped(bool isOn) {
        animController.SetBool("FingerGunEquip", isOn);
    }

    public void AnimFingerGunFire() {
        animController.SetTrigger("FingerGunFire");
    }

    public void AnimSnapFinger() {
        animController.SetTrigger("SnapFinger");
    }

    public void Equip(AbilityClass ability) {
        activeAbility.UnEquip();
        activeAbility = ability;
        ability.Equip(player, gameObject, this);
    }
}
