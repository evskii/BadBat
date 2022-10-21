using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;


public class Projectile_ElectricalCables : MonoBehaviour
{
    private Transform startPos;
    private Transform endPos;

    public int totalSteps;

    private LineRenderer lineRenderer;
    
    private bool active;

    private void Awake() {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void InitCable(Transform start, Transform end) {
        startPos = start;
        endPos = end;
        
        var itemsInRange = Physics.OverlapSphere(endPos.position, 1);
		
        foreach (var item in itemsInRange) {
            if (item.TryGetComponent(out IElectrical electrical)) {
                electrical.Surge();
            }
        }
        
        active = true;
    }

    private void Update() {
        if (active) {
            lineRenderer.SetVertexCount(totalSteps + 1);

            //Make points in the middle
            for (int i = 0; i < totalSteps; i++) {
                var linePointVector = Vector3.Lerp(startPos.position, endPos.position, (float)i / totalSteps);
                
                Vector3 finalVector = i > 0 ? new Vector3(linePointVector.x, linePointVector.y * Random.Range(0.8f, 0.95f), linePointVector.z) : linePointVector;
                
                lineRenderer.SetPosition(i, finalVector);
            }
        
            lineRenderer.SetPosition(totalSteps, endPos.position);

        }
    }
}
