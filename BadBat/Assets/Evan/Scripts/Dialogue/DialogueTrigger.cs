using System;
using System.Collections;
using System.Collections.Generic;

using Evan.Scripts.PlayerMovement;

using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DialogueTrigger : MonoBehaviour
{
    public string dialogue;
    public bool selfDestroy;

    private void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent(out FPSPlayerInput player)) {
            DialogueController.instance.NewDialogueInstance(dialogue, "TESTER");

            if (selfDestroy) {
                Destroy(gameObject);
            }
        }
    }
}
