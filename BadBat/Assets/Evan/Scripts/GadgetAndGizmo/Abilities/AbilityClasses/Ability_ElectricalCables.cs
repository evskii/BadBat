using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ability_ElectricalCables : AbilityClass
{

    

    public float maxLength;

    private GameObject cable;
    private GameObject endPointTransform;
    
    public override void Equip(GameObject player, GameObject gauntlet) {
        this.player = player;
        this.gauntlet = gauntlet;
    }
    
    public override void Fire(bool pressed) {
        if (pressed) {
            RaycastHit hit;
		
            if (Physics.Raycast(gauntlet.transform.position, gauntlet.transform.forward , out hit, maxLength)) {
                cable = Instantiate(abilityProjectile, gauntlet.transform.position, Quaternion.identity);
                endPointTransform = new GameObject();
                endPointTransform.transform.position = hit.point;
                cable.GetComponent<Projectile_ElectricalCables>().InitCable(gauntlet.transform, endPointTransform.transform);
            }
        } else {
            Destroy(endPointTransform);
            Destroy(cable);
        }
        
    }
    
    public override void UnEquip() {
        if (endPointTransform != null) {
            Destroy(endPointTransform);
        }

        if (cable != null) {
            Destroy(cable);
        }
        
    }
}
