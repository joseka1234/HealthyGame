using UnityEngine;
using System;

namespace AssemblyCSharp
{
	public class InformacionBala : MonoBehaviour
	{
		public Vector3 posicionOrigen;

		public float distanciaDesaparicion = 5f;

		public float velocidadBala = 10f;

		public float azucar = 0;

		void OnTriggerEnter2D (Collider2D other)
		{
			if (SePuedeChocar (other.tag)) {
				Destroy (gameObject);
			}
		}

		/// <summary>
		/// Indica si la bala se destruirá al chocar contra un objeto con esta etiqueta
		/// </summary>
		/// <returns><c>true</c>, if puede chocar was sed, <c>false</c> otherwise.</returns>
		/// <param name="etiqueta">Etiqueta.</param>
		private bool SePuedeChocar (String etiqueta)
		{
			if (tag == "BalaEnemigo") {
				return etiqueta != "Enemigo" && etiqueta != "Escalera";
			} else {
				return etiqueta != "Player" && etiqueta != "Escalera";
			}
		}

		public float GetAzucar ()
		{
			return azucar;
		}

		public void SetAzucar (float _azucar)
		{
			azucar = _azucar;
		}
	}
}