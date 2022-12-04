using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant_Offensive : MonoBehaviour, IPurchase
{
	//This script lets you code exactly what happens when purchasing for a shop, instead of having
	//different merchant scripts for each shop, you just add a new IPurchase script that it calls
	//Purchase from
	
	public void Purchase(SO_ShopItems item) {
		//Code here for what do do when purchasing an item
		Debug.Log("Merchant: Offensive. Purchasing: " + item.itemName);
	}
}
