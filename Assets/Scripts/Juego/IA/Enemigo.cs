using UnityEngine;
using System.Collections;

namespace AssemblyCSharp
{
	public abstract class Enemigo : MonoBehaviour
	{
		protected GameObject player { get; set; }

		public float distanciaDeteccion = 1f;
		protected float tiempoInvencibilidad = 1f;

		public int vidas = 3;

		protected bool estadoInvencibilidad;

		protected bool golpePorLaDerecha { get; set; }

		private bool atacando { get; set; }

		void Awake ()
		{
			player = GameObject.Find ("GameScene/Player");
			atacando = false;
			estadoInvencibilidad = false;
		}

		void Update ()
		{
			if (vidas <= 0) {
				Destroy (this.gameObject);
				return;
			}

			// Comprobamos la cercanía para comenzar el ataque
			if (PuedeAtacar () && !atacando) {
				atacando = true;
			}

			// Después de esto atacamos infinitamente
			if (atacando) {
				Movimiento ();
			}
		}

		public abstract void Movimiento ();

		public abstract void RecibeGolpe ();


		protected IEnumerator EstadoInvencible ()
		{
			estadoInvencibilidad = true;
			Color auxColor = GetComponent<SpriteRenderer> ().color;
			auxColor.a = 0.6f;
			GetComponent<SpriteRenderer> ().color = auxColor;
			yield return new WaitForSeconds (tiempoInvencibilidad);
			estadoInvencibilidad = false;
			auxColor.a = 1;
			GetComponent<SpriteRenderer> ().color = auxColor;
		}

		private bool PuedeAtacar ()
		{

			return Vector3.Distance (transform.position, player.transform.position) < distanciaDeteccion;
		}
	}
}