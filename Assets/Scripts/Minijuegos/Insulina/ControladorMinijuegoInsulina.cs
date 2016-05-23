using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControladorMinijuegoInsulina : MonoBehaviour
{
	public float LIMITE_IZQUIERDA = 255f;
	public float LIMITE_DERECHA = 100f;
	private const float HOLGURA = 10f;

	private const string RESPUESTA_LENTA = "LENTA";
	private const string RESPUESTA_RAPIDA = "RÁPIDA";

	private const float VELOCIDAD_INTERFACES = 300f;
	public GameObject opciones;
	public GameObject cuerpoHumano;
	public GameObject tipoInsulina;
	public GameObject error;
	public GameObject minijuego;
	public GameObject jeringa;
	private static Vector3 POSICION_INZQUIERDA = new Vector3 (-700, 0, 0);
	private static Vector3 POSICION_DERECHA = new Vector3 (1000, 0, 0);
	private static Vector3 POSICION_CUERPO_HUMANO = new Vector3 (175, 0, 0);

	void Start ()
	{
		switch (UnityEngine.Random.Range (0, 2)) {
		case 0:
			tipoInsulina.GetComponent<Text> ().text = "RÁPIDA";
			break;
		case 1:
			tipoInsulina.GetComponent<Text> ().text = "LENTA";
			break;
		default:
			throw new Exception ("Esto es imposible.");
			break;
		}
	}

	/// <summary>
	/// Cierra las opciones
	/// </summary>
	private void CerrarOpciones ()
	{
		StartCoroutine (FuncionesComunes.DesplazarInterfaz (opciones, POSICION_INZQUIERDA, VELOCIDAD_INTERFACES));
		StartCoroutine (FuncionesComunes.DesplazarInterfaz (cuerpoHumano, POSICION_CUERPO_HUMANO, VELOCIDAD_INTERFACES));
	}

	/// <summary>
	/// Selecciona una opción de las dos ofrecidas
	/// </summary>
	/// <param name="opcion">Opcion.</param>
	public void SeleccionarOpcion (GameObject opcion)
	{
		if (GetRespuestaInsulina () == RESPUESTA_LENTA) {
			if (opcion.name == "InsulinaLenta") {
				CerrarOpciones ();
			} else {
				StartCoroutine (MostrarError ());
			}
		} else {
			if (opcion.name == "InsulinaLenta") {
				StartCoroutine (MostrarError ());
			} else {
				CerrarOpciones ();
			}
		}
	}

	/// <summary>
	/// Muestra el mensaje de que has fallado
	/// </summary>
	/// <returns>The error.</returns>
	private IEnumerator MostrarError ()
	{
		error.SetActive (true);
		yield return new WaitForSeconds (3f);
		error.SetActive (false);
	}

	/// <summary>
	/// Devuelve la insulina que necesita el personaje en este momento
	/// </summary>
	/// <returns>The respuesta insulina.</returns>
	private string GetRespuestaInsulina ()
	{
		return tipoInsulina.GetComponent<Text> ().text;
	}

	public void SeleccionaZonaCuerpo (GameObject opcion)
	{
		StartCoroutine (SeleccionarZonaCuerpoCoroutine (opcion.name));
	}

	private IEnumerator SeleccionarZonaCuerpoCoroutine (string opcion)
	{
		if (GetRespuestaInsulina () == RESPUESTA_LENTA) {
			switch (opcion) {
			case "MusloDerecho":
			case "MusloIzquierdo":
			case "Gluteos":
				StartCoroutine (SetColor (true));
				while (ejecutando) {
					yield return new WaitForSeconds (0.01f);
				}
				AbrirMinijuego ();
				break;
			case "BrazoIzquierdo":
			case "BrazoDerecho":
			case "Abdomen":
				StartCoroutine (SetColor (false));
				break;
			}
		} else {
			switch (opcion) {
			case "MusloDerecho":
			case "MusloIzquierdo":
			case "Gluteos":
				StartCoroutine (SetColor (false));
				break;
			case "BrazoIzquierdo":
			case "BrazoDerecho":
			case "Abdomen":
				StartCoroutine (SetColor (true));
				while (ejecutando) {
					yield return new WaitForSeconds (0.01f);
				}
				AbrirMinijuego ();
				break;
			}
		}
	}

	bool ejecutando;

	/// <summary>
	/// Establece el color de acierto o no
	/// </summary>
	/// <returns>The color.</returns>
	/// <param name="acierto">If set to <c>true</c> acierto.</param>
	private IEnumerator SetColor (bool acierto)
	{
		ejecutando = true;
		if (acierto) {
			cuerpoHumano.GetComponent<Image> ().color = Color.green;
		} else {
			cuerpoHumano.GetComponent<Image> ().color = Color.red;
		}
		yield return new WaitForSeconds (1f);
		cuerpoHumano.GetComponent<Image> ().color = Color.white;
		ejecutando = false;
	}

	/// <summary>
	/// Abre el minijuego para pinchar la insulina
	/// </summary>
	private void AbrirMinijuego ()
	{
		StartCoroutine (FuncionesComunes.DesplazarInterfaz (cuerpoHumano, POSICION_INZQUIERDA, VELOCIDAD_INTERFACES));
		StartCoroutine (FuncionesComunes.DesplazarInterfaz (minijuego, Vector3.zero, VELOCIDAD_INTERFACES));
		StartCoroutine (BalancearJeringa ());
	}


	/// <summary>
	/// Balancea la jeringa
	/// </summary>
	private IEnumerator BalancearJeringa ()
	{
		float incrementoRotacion = 0.8f;
		bool haciaLaIzquierda = true;
		RectTransform transformJeringa = jeringa.GetComponent<RectTransform> ();
		float posicionZ;
		while (true) {
			posicionZ = transformJeringa.rotation.eulerAngles.z;
			if (posicionZ >= LIMITE_IZQUIERDA && haciaLaIzquierda) {
				incrementoRotacion *= -1f;
				haciaLaIzquierda = false;
			} else if (posicionZ <= LIMITE_DERECHA && !haciaLaIzquierda) {
				incrementoRotacion *= -1f;
				haciaLaIzquierda = true;
			}
			transformJeringa.Rotate (Vector3.forward * incrementoRotacion);
			if (Input.GetMouseButtonDown (0)) {
				if ((posicionZ <= 225 + HOLGURA && posicionZ >= 225 - HOLGURA) || (posicionZ <= 135 + HOLGURA && posicionZ >= 135 - HOLGURA)) {
					// TODO: Aquí va la animación de pinchazo
					break;
				}
			}
			yield return new WaitForSeconds (0.01f);
		}
		SceneManager.LoadScene (FuncionesComunes.GetEscenaPrevia ());
	}
}
