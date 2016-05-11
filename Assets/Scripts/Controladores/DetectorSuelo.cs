using UnityEngine;
using AssemblyCSharp;

public class DetectorSuelo : MonoBehaviour
{
	void OnTriggerStay2D (Collider2D other)
	{
		PlayerController.enSuelo |= other.tag == "Suelo";
	}

	void OnTriggerExit2D (Collider2D other)
	{
		PlayerController.enSuelo &= other.tag != "Suelo";
	}
}
