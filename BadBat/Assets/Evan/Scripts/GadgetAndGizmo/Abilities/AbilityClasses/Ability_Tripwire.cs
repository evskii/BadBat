using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Tripwire : AbilityClass
{
    public float placementRange;
    public float tripwireDistance;

    public Color positiveColor;
    private Material positiveMaterial;
    public Color negativeColor;
    private Material negativeMaterial;

    private GameObject placedTripwire;
    private GameObject visualizationTripwire;
    
    private bool visualizationMode;

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
        if (!placedTripwire) {
            
            if (pressed) {
                myGag.AnimWindUp();
            } else {
                myGag.AnimFire();
            }
            
            if (!pressed) { //Release
                visualizationMode = false;
                Destroy(visualizationTripwire);
                
                RaycastHit hit;
                if (Physics.Raycast(gauntlet.transform.position, gauntlet.transform.forward , out hit, placementRange)) {
                    //Where we point on the wall to fire
                    var placementPoint = hit.point;
                    var placementNormal = hit.normal.normalized;

                    RaycastHit rayToWall;
                    if (Physics.Raycast(placementPoint, placementNormal, out rayToWall, tripwireDistance)) {
                        placedTripwire = Instantiate(abilityProjectile, placementPoint, Quaternion.LookRotation(placementNormal.normalized), null);
                        placedTripwire.GetComponent<Projectile_Tripwire>().InitTripwire(placementPoint, rayToWall.point, tripwireDistance);
                    }
                }
            } else { //Press
                visualizationMode = true;
            }
        }

    }

    public override void AbilityUpdate() {
        if (visualizationMode) {
            if (!visualizationTripwire) {
                visualizationTripwire = GameObject.CreatePrimitive(PrimitiveType.Cube);
                visualizationTripwire.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                visualizationTripwire.GetComponent<Collider>().enabled = false;
            }
            
            
            RaycastHit hit;
            if (Physics.Raycast(gauntlet.transform.position, gauntlet.transform.forward , out hit, placementRange)) {
                //Where we point on the wall to fire
                var placementPoint = hit.point;
                var hitPoint = placementPoint;

                visualizationTripwire.transform.position = hit.point;
                visualizationTripwire.transform.rotation = Quaternion.LookRotation(hit.normal.normalized);
                
                //We run a raycast from the new position back to the wall to find the wall [that is the same as the one it originally hit]
                RaycastHit placementHit;
                if (Physics.Raycast(placementPoint, hit.normal.normalized, out placementHit, tripwireDistance)) {
                    visualizationTripwire.GetComponent<Renderer>().material = positiveMaterial;
                } else {
                    visualizationTripwire.GetComponent<Renderer>().material = negativeMaterial;
                }
            }
        }
    }
	
    public override void UnEquip() {
    }
}
