using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_MolotovGrenade : MonoBehaviour
{

    public GameObject projectileMesh;
    public GameObject fireArea;
    public float burnTime;
    public float damageRate;
    public int damage;

    private void OnCollisionEnter(Collision other) {
        if (other.contacts[0].normal == new Vector3(0, 1, 0)) { //Allow the collision to register if its on the ground
            //Spawn in ice area
            GetComponent<Collider>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            projectileMesh.SetActive(false);
        
            fireArea.SetActive(true);
            transform.rotation = Quaternion.Euler(new Vector3(0,1,0));
            fireArea.GetComponent<ParticleSystem>().startLifetime = burnTime;

            fireArea.GetComponent<Damaging_Particles>().damage = damage;
            fireArea.GetComponent<Damaging_Particles>().damageRate = damageRate;
        }

    }

    private IEnumerator BurnTimer() {
        yield return new WaitForSeconds(burnTime);
        Destroy(gameObject);
    }
}
