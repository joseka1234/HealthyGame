using UnityEngine;
using System.Collections;

namespace AssemblyCSharp
{
	public abstract class Enemigo : MonoBehaviour
	{
		// TODO: Mirar como hacer para que pare el enemigo y tah
		// TODO: Implementar el comportamiento general de cualquier enemigo

		public float distanciaDeteccion = 1f;

		protected GameObject player { get; set; }

		public int vidas = 3;

		protected float tiempoInvencibilidad = 1f;
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
			// Debug.Log (vidas);
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

		void OnTriggerEnter2D (Collider2D other)
		{
			Debug.Log (other.tag);
			if (other.tag == "Bala") {
				Debug.Log ("Chivato");
				golpePorLaDerecha |= other.transform.position.x > transform.position.x;
				RecibeGolpe ();
				Destroy (other.gameObject);
			}
		}
	}
}
