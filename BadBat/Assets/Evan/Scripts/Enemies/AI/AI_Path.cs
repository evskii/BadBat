using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Path : MonoBehaviour
{
    public List<Transform> pathPoints = new List<Transform>();
    public bool isLoop;
    
    //Explanation needed for future self as to why I did script like this. If the enemy leaves their path, this script would keep their progress
    //through the path with the beneath variable. Meaning they can just come back to the path if needed. No need to save index in the state script
    //then :)
    public int startPointIndex;
    [HideInInspector] public int currentPathPointIndex;

    private void Start() {
	    if (pathPoints.Count < 1) {
		    Debug.LogError("The following path has 0 points: " + gameObject.name);
	    }
	    currentPathPointIndex = startPointIndex;
    }

    public Vector3 GetNextPathPoint() {
	    if (isLoop) { //If we are a looped path increment or go back to the start
		    currentPathPointIndex = currentPathPointIndex == pathPoints.Count - 1 ? currentPathPointIndex = 0 : currentPathPointIndex + 1;
	    } else {
		    //If we are not looped, and we reach the end, return the same point or increment
		    // [There is code in the patrol scripts to handle changing back to idle if we look for a new point and it returns the same one]
		    currentPathPointIndex = currentPathPointIndex == pathPoints.Count - 1 ? currentPathPointIndex : currentPathPointIndex + 1;
	    }
	    return pathPoints[currentPathPointIndex].position;
    }

    //Debuggy
    private void OnDrawGizmos() {
	    for (int i = 0; i < pathPoints.Count; i++) {
		    Gizmos.color = Color.cyan;
		    Gizmos.DrawWireSphere(pathPoints[i].position, 0.2f);
		    
		    if (i == pathPoints.Count - 1) {
			    if (isLoop) {
				    Gizmos.color = Color.magenta;
				    Gizmos.DrawLine(pathPoints[i].position, pathPoints[0].position);
			    }
		    } else {
			    Gizmos.color = Color.magenta;
			    Gizmos.DrawLine(pathPoints[i].position, pathPoints[i + 1].position);
		    }
	    }
    }
}
