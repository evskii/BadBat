using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_IceGrenade : MonoBehaviour
{

    public GameObject projectileMesh;
    public GameObject iceArea;

    private void OnCollisionEnter(Collision other) {
        //Spawn in ice area
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        projectileMesh.SetActive(false);
        // GameObject iceArea = new GameObject();
        // iceArea.name = "Ice Area";
        // var collider = iceArea.AddComponent<BoxCollider>();
        // collider.isTrigger = true;
        // iceArea.tag = "Ice";
        // collider.size = new Vector3(10, 10, 10);
        // iceArea.transform.position = transform.position;
        
        iceArea.SetActive(true);

        // Destroy(gameObject);
    }
}
