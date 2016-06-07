using UnityEngine;
using System.Collections;

// TODO: Existe un bug por el que si tocamos a un enemigo estacionario su rutina de movimiento se bugea.
// PosibleSolución: Hacer que el enemigo estacionario muera al tocar al personaje.
// FIXME: Las rutinas de movimiento no funcionan NI DE COÑA.
namespace AssemblyCSharp
{
	public class EnemigoEstacionario : Enemigo
	{
		private const float CONSTANTE_CERCANIA = 0.0001f;

		// Factor de frenado para el Lerp
		public float velocidad = 1;
		public GameObject posicionDestino;
		public float tiempoEspera = 2;

		private Vector3 posicionComienzo { get; set; }

		private Vector3 posicionDestinoActual { get; set; }

		private bool SeProduceCambio { get; set; }

		private float tiempoComienzo { get; set; }

		void Start ()
		{
			posicionComienzo = transform.position;
			SeProduceCambio = true;
			posicionDestinoActual = posicionDestino.transform.position;
			tiempoComienzo = Time.time;
		}


		public override void Movimiento ()
		{
			if (DistanciaAObjetivo () > CONSTANTE_CERCANIA) {
				SeProduceCambio = true;
				Desplazar ();
			} else {
				if (SeProduceCambio) {
					SeProduceCambio = false;
					StartCoroutine (EjecutarEspera ());
				}
			}
		}

		private float DistanciaAObjetivo ()
		{
			return Vector3.Distance (posicionDestinoActual, transform.position);
		}

		private IEnumerator EjecutarEspera ()
		{
			yield return new WaitForSeconds (tiempoEspera);
			CambiarPosicionDestinoActual ();
		}

		public void CambiarPosicionDestinoActual ()
		{
			if (posicionDestinoActual == posicionComienzo) {
				Debug.Log ("Chivato");
				posicionDestinoActual = posicionDestino.transform.position;
			} else {
				Debug.Log ("Chivato2");
				posicionDestinoActual = posicionComienzo;
			}

			posicionComienzo = transform.position;
			tiempoComienzo = Time.time;
		}

		public void Desplazar ()
		{
			float distanciaCubierta = (Time.time - tiempoComienzo) * velocidad;
			float distanciaAObjetivo = Vector3.Distance (posicionComienzo, posicionDestinoActual);
			float fraccionViaje = distanciaCubierta / distanciaAObjetivo;
			transform.position = Vector3.Lerp (posicionComienzo, posicionDestinoActual, fraccionViaje);
		}

		public override void RecibeGolpe ()
		{
			// TODO: Implementar el knockback y tal
		}
	}
}