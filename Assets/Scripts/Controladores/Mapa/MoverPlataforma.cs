using UnityEngine;
using System.Collections;

public class MoverPlataforma : MonoBehaviour
{

	public GameObject[] posicionesDestino;
	public float tiempoEspera;
	public float velocidadPlataforma;
	public bool retorno;

	private int posicionActual { get; set; }
	private float tiempoComienzo { get; set; }
	private Vector3 posicionComienzo { get; set; }
	private bool SeProduceCambio { get; set; }
	private Vector3 posicionDestinoActual { get; set; }


	void Start ()
	{
		posicionActual = 0;
		tiempoComienzo = Time.time;
		posicionComienzo = this.transform.position;
		SeProduceCambio = true;
		posicionDestinoActual = posicionesDestino [posicionActual].transform.position;
	}

	// Update is called once per frame
	void Update ()
	{
		if (posicionDestinoActual != transform.position) {
			SeProduceCambio = true;
			Desplazar ();
		} else {
			if (SeProduceCambio) {
				SeProduceCambio = false;
				StartCoroutine (EjecutarEspera ());
			}
		}
	}

	private IEnumerator EjecutarEspera ()
	{
		yield return new WaitForSeconds (tiempoEspera);
		CambiarPosicionActual ();
	}

	private void Desplazar ()
	{
		float distanciaCubierta = (Time.time - tiempoComienzo) * velocidadPlataforma;
		float distanciaAObjetivo = Vector3.Distance (posicionComienzo, posicionDestinoActual);
		float fraccionViaje = distanciaCubierta / distanciaAObjetivo;
		transform.position = Vector3.Lerp (posicionComienzo, posicionDestinoActual, fraccionViaje);
	}

	private void CambiarPosicionActual ()
	{
		if (!retorno) {
			if (posicionActual + 1 < posicionesDestino.Length) {
				posicionActual++;
			}
		} else {
			if (posicionActual + 1 < posicionesDestino.Length) {
				posicionActual++;
			} else {
				posicionActual = -1;
			}
		}
		CambiarPosicionDestinoActual ();
	}

	private void CambiarPosicionDestinoActual ()
	{
		if (posicionActual >= 0) {
			posicionDestinoActual = posicionesDestino [posicionActual].transform.position;
		} else {
			posicionDestinoActual = posicionComienzo;
		}
		tiempoComienzo = Time.time;
		posicionComienzo = transform.position;
	}
		
	void OnTriggerStay2D (Collider2D other)
	{
		// Incluímos como hijo de la plataforma a cualquier objeto que se pose sobre ella
		other.transform.parent = transform;
	}
	
	void OnTriggerExit2D (Collider2D other)
	{
		// Excluímos como hijo de la plataforma a cualquier objeto que se separe de ella
		other.transform.parent = GameObject.Find ("GameScene").transform;
	}

}
