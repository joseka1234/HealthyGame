﻿using UnityEngine;
using System.Collections;

// TODO: Existe un bug por el que si tocamos a un enemigo estacionario su rutina de movimiento se bugea.
// PosibleSolución: Hacer que el enemigo estacionario muera al tocar al personaje.

namespace AssemblyCSharp
{
	public class EnemigoEstacionario : Enemigo
	{

		// Factor de frenado para el Lerp
		public float velocidad = 1;
		public GameObject posicionDestino;
		public float tiempoEspera = 2;

		private Vector3 posicionComienzo { get; set; }

		private Vector3 posicionDestinoActual { get; set; }

		private Vector3 posicionInterno { get; set; }

		private bool SeProduceCambio { get; set; }

		private float tiempoComienzo { get; set; }

		public float Azucar = 20f;

		void Start ()
		{
			SetAzucarProporcionada (Azucar);
			posicionComienzo = transform.position;
			SeProduceCambio = true;
			posicionInterno = posicionComienzo;
			posicionDestinoActual = posicionDestino.transform.position;
			tiempoComienzo = Time.time;
		}


		public override void Movimiento ()
		{
			if (transform.position != posicionDestinoActual) {
				SeProduceCambio = true;
				Desplazar ();
			} else {
				if (SeProduceCambio) {
					SeProduceCambio = false;
					StartCoroutine (EjecutarEspera ());
				}
			}

			if (player.transform.position.x > transform.position.x) {
				GetComponent<SpriteRenderer> ().flipX = true;
			} else {
				GetComponent<SpriteRenderer> ().flipX = false;
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
			if (transform.position == posicionComienzo) {
				posicionDestinoActual = posicionDestino.transform.position;
			} else if (transform.position == posicionDestino.transform.position) {
				posicionDestinoActual = posicionComienzo;
			}
			posicionInterno = transform.position;
			tiempoComienzo = Time.time;
		}

		public void Desplazar ()
		{
			float distanciaCubierta = (Time.time - tiempoComienzo) * velocidad;
			float distanciaAObjetivo = Vector3.Distance (posicionInterno, posicionDestinoActual);
			float fraccionViaje = distanciaCubierta / distanciaAObjetivo;
			transform.position = Vector3.Lerp (posicionInterno, posicionDestinoActual, fraccionViaje);
		}

		public override void RecibeGolpe ()
		{
			if (!estadoInvencibilidad) {
				StartCoroutine (EstadoInvencible ());
				vidas--;
			}
		}

		void OnTriggerEnter2D (Collider2D other)
		{
			if (other.tag == "Bala") {
				RecibeGolpe ();
			}
		}
	}
}