using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	//Use getter and setter method for this so we dont need extra methods for this process
	//A private setter is used and methods are made so we can handle any incoming setter
	//issues in the method themselves. Doesnt make sense to read now but makes sense in my
	
	public int partsTotal { get; private set; }
	public int cashTotal { get; private set; }

	public void UpdatePartsTotal(int amt) {
		partsTotal -= amt;
	}

	public void UpdateCashTotal(int amt) {
		cashTotal -= amt;
	}
}
