using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_IceGrenade : MonoBehaviour
{

    public GameObject projectileMesh;
    public GameObject iceArea;
    public float particleLifetime;

    private void OnCollisionEnter(Collision other) {
        if (other.contacts[0].normal == new Vector3(0, 1, 0)) { //Allow the collision to register if its on the ground
            //Spawn in ice area
            GetComponent<Collider>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            projectileMesh.SetActive(false);
        
            iceArea.SetActive(true);
            transform.rotation = Quaternion.Euler(new Vector3(0,1,0));
            iceArea.GetComponent<ParticleSystem>().startLifetime = particleLifetime;

            
            Destroy(gameObject, particleLifetime);
        }
    }
}
