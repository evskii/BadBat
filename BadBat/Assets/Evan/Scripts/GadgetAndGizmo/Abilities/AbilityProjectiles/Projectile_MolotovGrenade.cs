using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_MolotovGrenade : MonoBehaviour
{

    public GameObject projectileMesh;
    public GameObject fireArea;

    private void OnCollisionEnter(Collision other) {
        //Spawn in ice area
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        projectileMesh.SetActive(false);
        
        fireArea.SetActive(true);

    }
}
