using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level Settings", menuName = "Level Settings")]
public class LevelSettings : ScriptableObject
{
    public int mainPathLength;

    public bool hasShop;
    public bool hasBoss;
    public bool hasHiddenRoom;
}
