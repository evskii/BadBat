using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCall : MonoBehaviour
{
    [TextArea]
    public string dialogue;

    public void SendNewDialogue() {
        DialogueController.instance.NewDialogueInstance(dialogue, "TTEST");
    }
}
