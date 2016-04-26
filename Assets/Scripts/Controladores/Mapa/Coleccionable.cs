using UnityEngine;
using AssemblyCSharp;

public class Coleccionable : MonoBehaviour
{

	public float VelocidadDeGiro = 2f;

	void Update ()
	{
		transform.Rotate (Vector3.up * VelocidadDeGiro);
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Player") {
			GameObject.Find (PlayerController.PLAYER).GetComponent<PlayerController> ().puntuacion++;
			Destroy (this.gameObject);
		}
	}
}
