using System;
using System.Collections;
using System.Collections.Generic;

using Evan.Scripts.PlayerMovement;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsDebugMenu : MonoBehaviour
{
    
    public TMP_InputField BaseMoveSpeedInputField;
    public TMP_InputField SprintMultiplierInputField;
    public TMP_InputField CrouchMultiplierInputField;
    public TMP_InputField gravityInputField;
    public TMP_InputField jumpForceInputField;
    public TMP_InputField slideSpeedMultiplierInputField;
    public TMP_InputField SlideTimeInputField;
    public TMP_InputField slideFalloffInputField;

    private FPSPlayerMovementCharacterController characterController;

    private void Awake() {
        characterController = FindObjectOfType<FPSPlayerMovementCharacterController>();
    }

    private void Start() {
        BaseMoveSpeedInputField.text = characterController.baseMoveSpeed.ToString();
        SprintMultiplierInputField.text = characterController.sprintMultiplier.ToString();
        CrouchMultiplierInputField.text = characterController.crouchMultiplier.ToString();
        gravityInputField.text = characterController.gravity.ToString();
        jumpForceInputField.text = characterController.jumpForce.ToString();
        slideSpeedMultiplierInputField.text = characterController.slideSpeedMulti.ToString();
        slideFalloffInputField.text = characterController.slideFalloff.ToString();
    }

    private void Update() {
        characterController.baseMoveSpeed = float.Parse(BaseMoveSpeedInputField.text);
        characterController.sprintMultiplier = float.Parse(SprintMultiplierInputField.text);
        characterController.crouchMultiplier = float.Parse(CrouchMultiplierInputField.text);
        characterController.gravity = float.Parse(gravityInputField.text);
        characterController.jumpForce = float.Parse(jumpForceInputField.text);
        characterController.slideSpeedMulti = float.Parse(slideSpeedMultiplierInputField.text);
        characterController.slideFalloff = float.Parse(slideFalloffInputField.text);
    }
}
