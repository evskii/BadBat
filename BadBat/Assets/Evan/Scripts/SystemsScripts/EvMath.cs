using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EvMath
{
	public static float Map(float x, float a, float b, float c, float d) {
		return (x - a) / (b - a) * (d - c) + c;
	}
}
