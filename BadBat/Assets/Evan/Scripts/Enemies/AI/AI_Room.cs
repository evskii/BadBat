using System;
using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting.FullSerializer;

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

using Random = UnityEngine.Random;

public class AI_Room : MonoBehaviour
{   
    //  This script is used to let AI know which room they are bound to
    //  will be called upon when they need to know things about their room

    [Header("Room Settings")]
    [SerializeField] private Vector3 roomScale;
    [SerializeField] private float boundsPadding; //How much padding between wall and queries
    
    [Header("Debug Settings")]
    [SerializeField] private bool debugDisplay;
    [SerializeField] private Color debugColor;
    private Vector3 castFrom;
    private Vector3 randomPos;

    public Vector3 GetRandomPositionInRoom(bool requireNavmesh) {
        Vector3 finalPosition = Vector3.zero; //Setup return vector

        //Get random position on roof of room to cast from
        var randomXPos = Random.Range(transform.position.x - ((roomScale.x - boundsPadding) / 2), transform.position.x + ((roomScale.x - boundsPadding) / 2));
        var randomZPos = Random.Range(transform.position.z - ((roomScale.z - boundsPadding) / 2), transform.position.z + ((roomScale.z - boundsPadding) / 2));
        Vector3 castFromPoint = new Vector3(randomXPos, transform.position.y + (roomScale.y / 2), randomZPos);
        
        //Raycast down from the roof to get our random position
        RaycastHit hit;
        if (Physics.Raycast(castFromPoint, Vector3.down, out hit, Mathf.Infinity)) {
            finalPosition = hit.point;
        }

        //If this call needs to be on a navmesh we get the nearest navmesh position
        if (requireNavmesh) {
            NavMeshHit navHit;
            if (NavMesh.SamplePosition(hit.point, out navHit, 10f, NavMesh.AllAreas)) {
                finalPosition = navHit.position;
            }
        }

        //For debugging stuff
        castFrom = castFromPoint; 
        randomPos = finalPosition; 
        
        return finalPosition;
    }

    //-------------------------------------------- Debug Shite -------------------------------------------------------------------
    
    [ContextMenu("Test GetPos")]
    private void TestGetPos() {
        GetRandomPositionInRoom(true);
    }

    private void OnDrawGizmos() {
        if (debugDisplay) {
            Gizmos.color = debugColor;
            Gizmos.DrawWireCube(transform.position, roomScale);

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, new Vector3(roomScale.x - boundsPadding, roomScale.y, roomScale.z  - boundsPadding));

            Gizmos.color = debugColor;
            Gizmos.DrawSphere(castFrom, .25f);
            
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(randomPos, .25f);

        }
    }
}
