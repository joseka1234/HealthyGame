using UnityEngine;
using AssemblyCSharp;

public class DetectorSuelo : MonoBehaviour
{
	void OnCollisionStay2D (Collision2D coll)
	{
		if (coll.collider.tag == "Suelo") {
			PlayerController.enSuelo = true;
		} else {
			PlayerController.enSuelo = false;
		}
	}

	void OnCollisionExit2D (Collision2D coll)
	{
		if (coll.collider.tag == "Suelo") {
			PlayerController.enSuelo = false;
		}
	}

}
