using UnityEngine;
using AssemblyCSharp;
using System;

public class DetectorSuelo : MonoBehaviour
{

	void OnCollisionEnter2D (Collision2D other)
	{
		PlayerController.enSuelo |= other.collider.tag == "Suelo";
	}

	void OnCollisionExit2D (Collision2D other)
	{
		PlayerController.enSuelo &= other.collider.tag != "Suelo";
	}
}
