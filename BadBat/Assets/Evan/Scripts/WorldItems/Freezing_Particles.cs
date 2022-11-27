using System;
using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;

public class Freezing_Particles : MonoBehaviour
{
    private void OnParticleCollision(GameObject other) {
        
        if (other.TryGetComponent(out IFreezeable freezeable)) {
            // Debug.Log("FREEZE CALL");
            freezeable.Freeze();
        }
    }
    
}
