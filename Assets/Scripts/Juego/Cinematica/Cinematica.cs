using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using AssemblyCSharp;

public class Cinematica : MonoBehaviour
{
	public ElementoCinematica[] ElementosCinematica;

	private IEnumerator Reproducir ()
	{
		PlayerController.pausa = true;
		foreach (ElementoCinematica elemento in ElementosCinematica) {
			StartCoroutine (elemento.Mostrar ());
			while (ElementoCinematica.Ejecutando) {
				yield return new WaitForSeconds (0.1f);
			}
		}
		PlayerController.pausa = false;
		Destroy (this.gameObject);
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Player") {
			StartCoroutine (Reproducir ());
		}
	}
}
