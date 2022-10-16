using System.Collections;
using System.Collections.Generic;

using Evan.Scripts.PlayerMovement;

using UnityEngine;

public class TestPlayerDisable : MonoBehaviour
{

    [ContextMenu("Test Disable")]
    public void TestDisable() {
        FindObjectOfType<FPSPlayerInput>().enabled = false;
    }

    [ContextMenu("Test Enable")] 
    public void TestEnable() {
        FindObjectOfType<FPSPlayerInput>().enabled = true;
    }
}
