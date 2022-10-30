using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

using UnityEngine;

using Color = UnityEngine.Color;

public class Ability_BreachCharge : AbilityClass
{
    public float placementRange;
    public float deployableThickness;

    private Vector3 hitPoint;
    private Vector3 lookbackPoint;

    public override void Equip(GameObject player, GameObject gauntlet) {
        this.player = player;
        this.gauntlet = gauntlet;
    }
    public override void Fire(bool pressed) {
        if (pressed) {
            RaycastHit hit;
            //Debug.DrawRay(gauntlet.transform.position, gauntlet.transform.forward, Color.red, gunRange);
		
            if (Physics.Raycast(gauntlet.transform.position, gauntlet.transform.forward , out hit, placementRange)) {
                //Where we point on the wall to fire
                var placementPoint = hit.point;
                hitPoint = placementPoint;
                
                //We invert the normal of this point to make it face backwards
                var placementNormal = hit.normal.normalized;
                placementNormal = Vector3.Scale(placementNormal, new Vector3(-1, -1, -1));
                var placementVector = Vector3.Scale(placementNormal, new Vector3(deployableThickness, deployableThickness, deployableThickness));
                
                //We get a point passed the wall that allows us to look backwards
                var lookbackPos = placementPoint + placementVector;
                lookbackPoint = lookbackPos;

                // var hitTrans = new GameObject();
                // hitTrans.transform.position = placementPoint;
                // hitTrans.name = "Hit Trans";
                //
                // var newTrans = new GameObject();
                // newTrans.transform.position = lookbackPos;
                // newTrans.name = "Lookback Trans";

                RaycastHit placementHit;
                if (Physics.Raycast(lookbackPos, hit.normal.normalized, out placementHit, deployableThickness)) {
                    if (placementHit.collider == hit.collider) {
                        Instantiate(abilityProjectile, placementHit.point, Quaternion.LookRotation(placementHit.normal.normalized), null);
                    }
                }
            }
        }
		
    }
	
    public override void AbilityUpdate() {
        //Not Used
    }
	
    public override void UnEquip() {
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(hitPoint, 0.25f);
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(lookbackPoint, 0.25f);
    }
}
