using UnityEngine;
using AssemblyCSharp;

public class DetectorSuelo : MonoBehaviour
{
	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Suelo" || other.tag == "Enemigo") {
			PlayerController.enSuelo = true;
		}
	}

	void OnTriggerExit2D (Collider2D other)
	{
		if (other.tag == "Suelo" || other.tag == "Enemigo") {
			PlayerController.enSuelo = false;
		}
	}
}