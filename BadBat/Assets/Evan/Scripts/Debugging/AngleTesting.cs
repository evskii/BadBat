using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleTesting : MonoBehaviour
{
    public GameObject otherPlayer;

    public float viewingAngle;
    public bool canSee;
    public float angle;
    public Vector3 direction;

    private void Update() {
        GetAngle();
    }

    [ContextMenu("Get Angle")]
    private void GetAngle() {
        direction = otherPlayer.transform.position - transform.position;
        angle = Vector3.Angle(transform.forward, direction);

        canSee = angle < viewingAngle;

        // Debug.Log(angle);
    }

    private void OnDrawGizmos() {
        Gizmos.color = angle <= viewingAngle ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, otherPlayer.transform.position);

        Gizmos.color = Color.cyan;
        Quaternion rot = Quaternion.AngleAxis(viewingAngle, Vector3.up);
        Vector3 dirVec = rot * transform.forward * 10;
        Gizmos.DrawRay(transform.position, dirVec);

        Quaternion leftRot = Quaternion.AngleAxis(-viewingAngle, Vector3.up);
        dirVec = leftRot * transform.forward * 10;
        Gizmos.DrawRay(transform.position, dirVec);
    }
}
