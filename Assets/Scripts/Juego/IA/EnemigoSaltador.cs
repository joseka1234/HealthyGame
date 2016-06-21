using UnityEngine;

namespace AssemblyCSharp
{
	public class EnemigoSaltador : Enemigo
	{
		public float saltoHorizontal = 10f;
		public float saltoVertical = 10f;
		public float esperaEntreSaltos = 10f;

		private float tiempo { get; set; }

		private Animator animaciones;

		public float Azucar = 20f;

		void Start ()
		{
			SetAzucarProporcionada (Azucar);
			tiempo = Time.time;
			animaciones = GetComponent<Animator> ();
		}

		public override void Movimiento ()
		{
			if (EstaEnSuelo ()) {
				animaciones.SetBool ("Salto", false);
				if (Time.time - tiempo >= esperaEntreSaltos) {
					tiempo = Time.time;
					SaltoDiagonal ();
				}
			}
		}

		private void SaltoDiagonal ()
		{
			animaciones.SetBool ("Salto", true);
			Rigidbody2D body = GetComponent<Rigidbody2D> ();
			body.velocity = new Vector2 (-saltoHorizontal, saltoVertical);
		}

		private bool EstaEnSuelo ()
		{
			return GetComponent<Rigidbody2D> ().velocity.y == 0;
		}

		public override void RecibeGolpe ()
		{
			if (!estadoInvencibilidad) {
				StartCoroutine (EstadoInvencible ());
				vidas--;
			}
			if (golpePorLaDerecha && MoviendoHaciaDerecha ()) {
				CambiarDireccion ();
			} else if (!golpePorLaDerecha && !MoviendoHaciaDerecha ()) {
				CambiarDireccion ();
			}
		}

		void OnTriggerEnter2D (Collider2D other)
		{
			if (other.tag == "Suelo") {
				CambiarDireccion ();
			}
			if (other.tag == "Bala") {
				golpePorLaDerecha = other.transform.position.x > transform.position.x;
				RecibeGolpe ();
				Destroy (other.gameObject);
			}
		}

		/// <summary>
		/// Cambia la dirección del personaje
		/// </summary>
		private void CambiarDireccion ()
		{
			saltoHorizontal *= -1;
		}

		private bool MoviendoHaciaDerecha ()
		{
			return saltoHorizontal > 0;
		}
	}
}