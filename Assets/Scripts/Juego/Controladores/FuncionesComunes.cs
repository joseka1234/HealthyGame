using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class FuncionesComunes : MonoBehaviour
{

	private const float ESPERA_DESPLAZAMIENTO = 0.001f;
	public static bool EJECUTANDO;

	/// <summary>
	/// Desplaza un objeto (gameobject)
	/// </summary>
	/// <returns>The objeto.</returns>
	/// <param name="objeto">Objeto.</param>
	/// <param name="posicionDestino">Posicion destino.</param>
	/// <param name="velocidad">Velocidad.</param>
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

	/// <summary>
	/// Desplaza una interfaz (objeto de la UI)
	/// </summary>
	/// <returns>The interfaz.</returns>
	/// <param name="objeto">Objeto.</param>
	/// <param name="posicionDestino">Posicion destino.</param>
	/// <param name="velocidad">Velocidad.</param>
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

	/// <summary>
	/// Establece cual fue la escena previa.
	/// </summary>
	public static void SetEscenaPrevia ()
	{
		GameObject indicador = GameObject.FindGameObjectWithTag ("IndicadorEscena");
		indicador.GetComponent<EscenaActual> ().SetEscena (SceneManager.GetActiveScene ());
	}

	/// <summary>
	/// Recupera la escena anterior.
	/// </summary>
	/// <returns>The previa.</returns>d
	public static string GetEscenaPrevia ()
	{
		return GameObject.FindGameObjectWithTag ("IndicadorEscena").GetComponent<EscenaActual> ().GetEscena ();
	}

	/// <summary>
	/// Ejecuta un método al cuando termine de ejecutarse aquello que incluya el booleano EJECUTANDO
	/// </summary>
	/// <returns>The al final.</returns>
	/// <param name="metodo">Metodo.</param>
	public static IEnumerator EjecutarAlFinal (Action metodo)
	{
		while (EJECUTANDO) {
			yield return new WaitForSeconds (0.01f);
		}
		metodo ();
	}

	public static IEnumerator EjecutarAlFinal (float segundosExtra, Action metodo)
	{
		while (EJECUTANDO) {
			yield return new WaitForSeconds (0.01f);
		}
		yield return new WaitForSeconds (segundosExtra);
		metodo ();
	}
}