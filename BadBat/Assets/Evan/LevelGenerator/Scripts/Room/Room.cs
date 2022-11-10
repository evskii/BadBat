using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject modelReference;
    public int doorCount;
    
    public enum  ConnectorType {Wall, Door}

    public bool shopRoom;
    public bool bossRoom;
    public bool hiddenRoom;
}
