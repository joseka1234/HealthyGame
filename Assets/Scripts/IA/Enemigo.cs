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

		private bool atacando { get; set; }

		public int vidas = 3;

		void Awake ()
		{
			player = GameObject.Find ("GameScene/Player");
			atacando = false;
		}

		void Update ()
		{
			// Comprobamos la cercanía para comenzar el ataque
			if (PuedeAtacar ()) {
				Movimiento ();
				atacando = true;
			}
			// Después de esto atacamos infinitamente
			if (atacando) {
				Movimiento ();
			}
		}

		public abstract void Movimiento ();

		public abstract void RecibeGolpe ();

		private bool PuedeAtacar ()
		{
			return Vector3.Distance (transform.position, player.transform.position) < distanciaDeteccion;
		}

		void OnTriggerEnter2D (Collider2D other)
		{
			if (other.tag == "Bala") {
				vidas--;
				RecibeGolpe ();
			}
		}
	}
}
