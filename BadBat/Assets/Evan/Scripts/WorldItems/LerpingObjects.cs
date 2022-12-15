using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpingObjects : MonoBehaviour
{
	public bool localSpace;
	public Vector3 startPos;
	public Vector3 endPos;
	private Vector3 destinationPosition;

	public float moveSpeed;
	public float stoppingDistance;

	public bool lerpOnStart = false;
	
	private bool active = false;

	private void Start() {
		if (lerpOnStart) {
			LerpToStartPos();
		}
	}

	public void LerpToEndPos() {
		destinationPosition = endPos;
		active = true;
	}

	public void LerpToStartPos() {
		destinationPosition = startPos;
		active = true;
	}

	private void Update() {
		if (active) {
			if (localSpace) {
				transform.localPosition = Vector3.Lerp(transform.localPosition, destinationPosition, moveSpeed * Time.deltaTime);
				active = Vector3.Distance(transform.localPosition, destinationPosition) > stoppingDistance;
			} else {
				transform.position = Vector3.Lerp(transform.position, destinationPosition, moveSpeed * Time.deltaTime);
				active = Vector3.Distance(transform.position, destinationPosition) > stoppingDistance;
			}
			
		}
	}
}
