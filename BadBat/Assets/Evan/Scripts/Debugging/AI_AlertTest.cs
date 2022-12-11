using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class AI_AlertTest : MonoBehaviour
{
    [ContextMenu("TEST ALERT")]
    public void TestAlert() {
        var alertObjects = FindObjectsOfType<MonoBehaviour>().OfType<IAlert>().ToList();
        foreach (var obj in alertObjects) {
            obj.Alert(transform.position);
        }
    }
}
