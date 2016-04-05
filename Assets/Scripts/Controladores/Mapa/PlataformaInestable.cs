using UnityEngine;
using System.Collections;

public class PlataformaInestable : MonoBehaviour
{
	public float tiempoAguante = 2f;

	private float tiempoComienzo { get; set; }

	private bool pisada { get; set; }

	private bool callendo { get; set; }

	void Start ()
	{
		pisada = false;

	}

	void Update ()
	{
		if (pisada) {
			if (Time.time - tiempoComienzo >= tiempoAguante) {
				Caer ();
			}
		} else {
			if (callendo) {
				if (Time.time - tiempoComienzo >= tiempoAguante * 5) {
					Destroy (gameObject);
				}
			}
		}
	}

	private void Caer ()
	{
		Rigidbody2D cuerpo = GetComponent<Rigidbody2D> ();
		cuerpo.gravityScale = 5;
		cuerpo.constraints = RigidbodyConstraints2D.None;

		foreach (BoxCollider2D cajaCollider in GetComponents<BoxCollider2D>()) {
			cajaCollider.isTrigger = true;
		}
		callendo = true;
		pisada = false;
		tiempoComienzo = Time.time;
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Player") {
			tiempoComienzo = Time.time;
			pisada = true;
			Debug.Log ("Chivato");
		}
	}
}
