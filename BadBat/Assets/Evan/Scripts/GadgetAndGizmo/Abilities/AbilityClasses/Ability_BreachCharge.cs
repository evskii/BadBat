using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

using UnityEngine;
using UnityEngine.UIElements;

using Color = UnityEngine.Color;

public class Ability_BreachCharge : AbilityClass
{
    public float placementRange;
    public float deployableThickness;

    public Color positiveColor;
    private Material positiveMaterial;
    public Color negativeColor;
    private Material negativeMaterial;

    private GameObject placedBreach;
    private GameObject visualizationBreach;
    
    private bool visualizationMode;

    public LayerMask layersToIgnore;

    public override void Equip(GameObject player, GameObject gauntlet, GadgetAndGizmo myGag) {
        this.player = player;
        this.gauntlet = gauntlet;
        this.myGag = myGag;

        positiveMaterial = new Material(Shader.Find("Specular"));
        positiveMaterial.color = positiveColor;

        negativeMaterial = new Material(Shader.Find("Specular"));
        negativeMaterial.color = negativeColor;
    }
    public override void Fire(bool pressed) {
        if (!placedBreach) {
            
            if (pressed) {
                myGag.AnimWindUp();
            } else {
                myGag.AnimFire();
            }
            
            if (!pressed) { //Release
                visualizationMode = false;
                Destroy(visualizationBreach);
                
                RaycastHit hit;
                if (Physics.Raycast(gauntlet.transform.position, gauntlet.transform.forward , out hit, placementRange, ~layersToIgnore)) {
                    //Where we point on the wall to fire
                    var placementPoint = hit.point;
                    var hitPoint = placementPoint;
                
                    //We invert the normal of this point to make it face backwards
                    var placementNormal = hit.normal.normalized;
                    placementNormal = Vector3.Scale(placementNormal, new Vector3(-1, -1, -1));
                    var placementVector = Vector3.Scale(placementNormal, new Vector3(deployableThickness, deployableThickness, deployableThickness));
                
                    //We get a point passed the wall that allows us to look backwards at the wall
                    var lookbackPos = placementPoint + placementVector;
                    var lookbackPoint = lookbackPos;

                    //We run a raycast from the new position back to the wall to find the wall [that is the same as the one it originally hit]
                    RaycastHit placementHit;
                    if (Physics.Raycast(lookbackPos, hit.normal.normalized, out placementHit, deployableThickness, ~layersToIgnore)) {
                        if (placementHit.collider == hit.collider) {
                            placedBreach = Instantiate(abilityProjectile, placementHit.point, Quaternion.LookRotation(placementHit.normal.normalized), null);
                        }
                    }
                }
            } else { //Press
                visualizationMode = true;
            }
        }

    }

    public override void AbilityUpdate() {
        if (visualizationMode) {
            if (!visualizationBreach) {
                visualizationBreach = GameObject.CreatePrimitive(PrimitiveType.Cube);
                visualizationBreach.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                visualizationBreach.GetComponent<Collider>().enabled = false;
            }
            
            
            RaycastHit hit;
            if (Physics.Raycast(gauntlet.transform.position, gauntlet.transform.forward , out hit, placementRange, ~layersToIgnore)) {
                //Where we point on the wall to fire
                var placementPoint = hit.point;
                var hitPoint = placementPoint;
                
                //We invert the normal of this point to make it face backwards
                var placementNormal = hit.normal.normalized;
                placementNormal = Vector3.Scale(placementNormal, new Vector3(-1, -1, -1));
                var placementVector = Vector3.Scale(placementNormal, new Vector3(deployableThickness, deployableThickness, deployableThickness));
                
                //We get a point passed the wall that allows us to look backwards at the wall
                var lookbackPos = placementPoint + placementVector;
                var lookbackPoint = lookbackPos;

                visualizationBreach.transform.position = hit.point;
                visualizationBreach.transform.rotation = Quaternion.LookRotation(hit.normal.normalized);

                //We run a raycast from the new position back to the wall to find the wall [that is the same as the one it originally hit]
                RaycastHit placementHit;
                if (Physics.Raycast(lookbackPos, hit.normal.normalized, out placementHit, deployableThickness, ~layersToIgnore)) {
                    visualizationBreach.GetComponent<Renderer>().material = placementHit.collider == hit.collider ? positiveMaterial : negativeMaterial;
                } else {
                    visualizationBreach.GetComponent<Renderer>().material = negativeMaterial;
                }
            }
        }
    }
	
    public override void UnEquip() {
    }

}
