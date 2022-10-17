using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class WeaponSelectButtonInfo : MonoBehaviour
{
    public string abilityName;
    public GadgetAndGizmo gauntlet;

    public void InitButtonInfo(string name, GadgetAndGizmo gauntlet) {
        abilityName = name;
        this.gauntlet = gauntlet;

        GetComponentInChildren<TMP_Text>().text = abilityName;
    }
    
    public void Equip() {
        gauntlet.Equip(abilityName);
    }
}
