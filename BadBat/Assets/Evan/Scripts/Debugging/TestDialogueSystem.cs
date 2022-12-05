using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDialogueSystem : MonoBehaviour
{
	private void Start() {
		for (int i = 0; i < 10; i++) {
			string dialoguetest = "THIS IS A TEST DIALOGUE USED TO TEST OUR DIALOGUE SYSTEM AND SEE HOW IT LOOKS " + i;
			DialogueController.instance.NewDialogueInstance(dialoguetest, "TEST CHARACTER");
		}
	}
}
