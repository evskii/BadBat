using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using UnityEngine;

public class DialogueController : MonoBehaviour
{
    //Singleton creation
    public static DialogueController instance;
    private void Awake() {
        instance = this;
    }

    public GameObject dialogueInstancePrefab;
    public Transform dialogueInstanceParent;
    public float defaultQueIterateTime = 5f;

    private Coroutine queIterationCoroutine;
    
    private List<GameObject> dialogueInstanceQue = new List<GameObject>();
    
    //Create a new dialogue instance
    public void NewDialogueInstance(string dialogue, string characterName) {
        var newInstance = Instantiate(dialogueInstancePrefab, dialogueInstanceParent);
        newInstance.GetComponent<DialogueBoxInstance>().InitDialogueBox(dialogue, characterName);
        newInstance.SetActive(false);
        dialogueInstanceQue.Add(newInstance);
        if (queIterationCoroutine == null) {
            queIterationCoroutine = StartCoroutine(IterateQue());
        }
    }

    private IEnumerator IterateQue() {
        Debug.Log("ITERATE QUE START");
        dialogueInstanceQue[0].SetActive(true);
        yield return new WaitForSeconds(defaultQueIterateTime);

        var toDestroy = dialogueInstanceQue[0];
        dialogueInstanceQue.Remove(toDestroy);
        Destroy(toDestroy);
        
        if (dialogueInstanceQue.Count > 0) {
            queIterationCoroutine = StartCoroutine(IterateQue());
        } else {
            queIterationCoroutine = null;
        }
        Debug.Log("ITERATE QUE STOP");
    }
    
    
}
