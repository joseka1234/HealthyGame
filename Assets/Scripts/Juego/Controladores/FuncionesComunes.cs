using UnityEngine;
using System.Collections;

public class FuncionesComunes : MonoBehaviour
{

	private const float ESPERA_DESPLAZAMIENTO = 0.001f;
	public static bool EJECUTANDO;

	public static IEnumerator DesplazarObjeto (GameObject objeto, Vector3 posicionDestino, float velocidad)
	{
		EJECUTANDO = true;
		Vector3 posicionInicial = objeto.transform.localPosition;
		float tiempoInicial = Time.time;
		while (Vector3.Distance (objeto.transform.localPosition, posicionDestino) > 0) {
			float distanciaCubierta = (Time.time - tiempoInicial) * velocidad;
			float distanciaAObjetivo = Vector3.Distance (posicionInicial, posicionDestino);
			float fraccionViaje = distanciaCubierta / distanciaAObjetivo;
			objeto.transform.localPosition = Vector3.Lerp (posicionInicial, posicionDestino, fraccionViaje);
			yield return new WaitForSeconds (ESPERA_DESPLAZAMIENTO);
		}
		EJECUTANDO = false;
	}

	public static IEnumerator DesplazarInterfaz (GameObject objeto, Vector3 posicionDestino, float velocidad)
	{
		EJECUTANDO = true;
		Vector3 posicionInicial = objeto.GetComponent<RectTransform> ().localPosition;
		float tiempoInicial = Time.time;
		while (Vector3.Distance (objeto.GetComponent<RectTransform> ().localPosition, posicionDestino) > 0) {
			float distanciaCubierta = (Time.time - tiempoInicial) * velocidad;
			float distanciaAObjetivo = Vector3.Distance (posicionInicial, posicionDestino);
			float fraccionViaje = distanciaCubierta / distanciaAObjetivo;
			objeto.GetComponent<RectTransform> ().localPosition = Vector3.Lerp (posicionInicial, posicionDestino, fraccionViaje);
			yield return new WaitForSeconds (ESPERA_DESPLAZAMIENTO);
		}
		EJECUTANDO = false;
	}
}
