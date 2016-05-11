using UnityEngine;
using System.Collections;

public class Movimiento : MonoBehaviour
{
	public float Velocidad;
	public GameObject Destino;
	public GameObject Personaje;
	public bool Ejecutando;

	void Start ()
	{
		Ejecutando = false;
	}

	public IEnumerator Mostrar ()
	{
		Ejecutando = true;
		float tiempoComienzo = Time.time;
		Vector3 posicionComienzo = Personaje.transform.position;

		while (Personaje.transform.position != Destino.transform.position) {
			float distanciaCubierta = (Time.time - tiempoComienzo) * Velocidad;
			float distanciaAObjetivo = Vector3.Distance (posicionComienzo, Destino.transform.position);
			float fraccionViaje = distanciaCubierta / distanciaAObjetivo;
			Personaje.transform.position = Vector3.Lerp (posicionComienzo, Destino.transform.position, fraccionViaje);
			yield return new WaitForSeconds (0.01f);
		}
		Ejecutando = false;
	}
}
