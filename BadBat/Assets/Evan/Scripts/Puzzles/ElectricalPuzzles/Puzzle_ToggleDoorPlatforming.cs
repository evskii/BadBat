using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle_ToggleDoorPlatforming : MonoBehaviour
{
	private bool toggledOn = true;
	public LerpingObjects[] platforms;
	public Puzzle_OpenableDoor door;

	private void Start() {
		PuzzleToggle(true);
	}

	private delegate void DoorAction();
	private DoorAction doorAction;

	public void PuzzleToggle() {
		toggledOn = !toggledOn;

		PuzzleToggle(toggledOn);
	}

	public void PuzzleToggle(bool toggledOn) {
		this.toggledOn = toggledOn;
		
		doorAction = toggledOn ? door.OpenDoor : door.CloseDoor;
		doorAction();
		
		foreach (LerpingObjects platform in platforms) {
			if (toggledOn) {
				platform.LerpToStartPos();
			} else {
				platform.LerpToEndPos();
			}
		}
	}
}
