using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UIElements;

public class DialogueBoxInstance : MonoBehaviour
{
    public TMP_Text dialogueText;
    public Image dialogueCharacterBox;

    public void InitDialogueBox(string dialogue, string characterName) {
        dialogueText.text = dialogue;
        
        //Need to add code for character images
    }
}
