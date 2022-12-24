using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class ChristmasTreeLights : MonoBehaviour
{
    public GameObject[] lightObjects;

    private void Start() {
        InvokeRepeating("RandomizeLights", 0, 1f);
    }

    public void RandomizeLights() {
        foreach (var light in lightObjects) {
            bool turnedOn = Random.Range(0, 100) < 50;
            light.SetActive(turnedOn);
        }
    }
}
