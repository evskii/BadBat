using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GivePlayerAbility : MonoBehaviour
{
    public AbilityClass toGive;
    public GadgetAndGizmo.Arm armToGive;
    private GadgetAndGizmo toGiveTo;

    private void Awake() {
        var arms = FindObjectsOfType<GadgetAndGizmo>();
        foreach (var arm in arms) {
            if (arm.arm == armToGive) {
                toGiveTo = arm;
            }
        }
    }

    public void GiveAbility() {
        toGiveTo.availableAbilities.Add(toGive);
    }
}
