using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Tripwire : MonoBehaviour
{
    private Vector3 startPoint;
    private Vector3 endPoint;
    private float maxLength;

    public GameObject explosionParticle;
    
    private GameObject endObject;

    private LineRenderer lineRenderer;

    public void InitTripwire(Vector3 startPos, Vector3 endPos, float maxLength) {
        lineRenderer = GetComponent<LineRenderer>();
        
        endPoint = endPos;
        startPoint = startPos;
        this.maxLength = maxLength;
        
        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);
    }

    private void Update() {
        var rayDirection = (endPoint - startPoint).normalized;
        RaycastHit hit;
        if (Physics.Raycast(startPoint, rayDirection, out hit, maxLength)) {
            if (!endObject) {
                endObject = hit.transform.gameObject;
            }
            lineRenderer.SetPosition(1, hit.point);
            if (hit.transform.gameObject != endObject) {
                Instantiate(explosionParticle, transform.position, Quaternion.identity);
                Destroy(gameObject);
                //NEED TO ADD CODE HERE TO MAKE SHIT TAKE DAMAGE
            }
        }
    }
}
