using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class Projectile_DeployableCover : MonoBehaviour
{

	public GameObject subProjectile;

	private void OnCollisionEnter(Collision other) {
		if (other.contacts[0].normal == new Vector3(0, 1, 0)) {
			Instantiate(subProjectile, other.contacts[0].point, transform.rotation);
			Destroy(gameObject);
		}
	}


}
