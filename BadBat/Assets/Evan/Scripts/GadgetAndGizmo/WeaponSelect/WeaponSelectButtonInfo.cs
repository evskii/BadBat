using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class WeaponSelectButtonInfo : MonoBehaviour
{
    public AbilityClass ability;
    public GadgetAndGizmo gauntlet;

    public void InitButtonInfo(AbilityClass ability, GadgetAndGizmo gauntlet) {
        this.ability = ability;
        this.gauntlet = gauntlet;

        GetComponentInChildren<TMP_Text>().text = ability.abilityName;
    }
    
    public void Equip() {
        gauntlet.Equip(ability);
    }
}
