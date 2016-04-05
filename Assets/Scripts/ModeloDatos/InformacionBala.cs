using UnityEngine;
using UnityEngine.Networking.Match;

namespace AssemblyCSharp
{
	public class InformacionBala : MonoBehaviour
	{
		public Vector3 posicionOrigen;

		public float distanciaDesaparicion = 5f;

		public float velocidadBala = 10f;

		void OnTriggerEnter2D (Collider2D other)
		{
			if (SePuedeChocar (other)) {
				Destroy (gameObject);
			}
		}

		private bool SePuedeChocar (Collider2D other)
		{
			string etiqueta = other.gameObject.tag;
			return etiqueta != "Player" && etiqueta != "Escalera";
		}
	}
}