using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.Events;

public class Puzzle_ElectricalBox : MonoBehaviour,IElectrical
{
	public UnityEvent surgeEvent;

	public bool timedEvent = false;
	public float timedEventDelay;
	public UnityEvent timedDelayEvent;


    public void Surge() {
       surgeEvent.Invoke();

       if (timedEvent) {
	       StartCoroutine(CallTimedEvent());
       }
    }


    private IEnumerator CallTimedEvent() {
	    yield return new WaitForSeconds(timedEventDelay);
	    timedDelayEvent.Invoke();
    }
}
