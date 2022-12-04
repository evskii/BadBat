using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shop Item", menuName = "Shop Items")]
public class SO_ShopItems : ScriptableObject
{
    public string itemName;
    [TextArea] public string itemDescription;
    public Sprite itemSprite;
    public enum ItemType
    {
        Offensive,
        Tactical,
        Cosmetic
    }
    public ItemType itemType;
    public int itemCostCash;
    public int itemCostParts;
    
}
