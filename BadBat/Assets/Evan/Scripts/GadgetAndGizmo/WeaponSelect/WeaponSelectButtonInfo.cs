using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectButtonInfo : MonoBehaviour
{
    public AbilityClass ability;
    public GadgetAndGizmo gauntlet;

    public void InitButtonInfo(AbilityClass ability, GadgetAndGizmo gauntlet) {
        this.ability = ability;
        this.gauntlet = gauntlet;

        var textBox = GetComponentInChildren<TMP_Text>();
        textBox.text = ability.abilityName;

        //Find the mid point of the curve so we can place the text in middle
        // var zRotMin = transform.eulerAngles.z;
        // var zRotMax = Map(GetComponent<Image>().fillAmount, 0, 1, 0, 360);
        // var zRotFinal = (zRotMax - zRotMin) / 2;
        
        textBox.transform.localRotation = Quaternion.Euler(0,0, -transform.eulerAngles.z);
    }
    
    public void Equip() {
        gauntlet.Equip(ability);
        SelectButton();
    }

    public bool IsEquipped() {
        return gauntlet.activeAbility == ability;
    }

    public void SelectButton() {
        GetComponent<Image>().color = Color.green;
    }

    public void DeselectButton() {
        GetComponent<Image>().color = Color.white;
    }
}
