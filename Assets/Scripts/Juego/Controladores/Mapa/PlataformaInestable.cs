using UnityEngine;
using System.Collections;

public class PlataformaInestable : MonoBehaviour
{
	public float tiempoAguante = 2f;

	private float tiempoComienzo { get; set; }

	private bool pisada { get; set; }

	private bool callendo { get; set; }

	private Vector3 posicionInicio { get; set; }

	void Start ()
	{
		pisada = false;
		callendo = false;
		posicionInicio = transform.position;
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
					Resetear ();
				}
			}
		}
	}

	public void Resetear ()
	{
		callendo = false;
		gameObject.SetActive (false);
		transform.position = posicionInicio;

		Rigidbody2D cuerpo = GetComponent<Rigidbody2D> ();
		cuerpo.gravityScale = 0;
		cuerpo.constraints = RigidbodyConstraints2D.FreezeAll;
		foreach (BoxCollider2D cajaCollider in GetComponents<BoxCollider2D>()) {
			cajaCollider.isTrigger = false;
			break;
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
		}
	}
}
