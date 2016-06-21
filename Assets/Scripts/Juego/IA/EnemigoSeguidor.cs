using UnityEngine;

namespace AssemblyCSharp
{
	public class EnemigoSeguidor : Enemigo
	{
		public float retrasoAtaque = 100f;

		private bool recibiendoGolpe { get; set; }

		private float tiempoKnockBack { get; set; }

		private float distanciaKnockBack { get; set; }

		public float Azucar = 15f;

		void Start ()
		{
			SetAzucarProporcionada (Azucar);
			recibiendoGolpe = false;
			distanciaDeteccion = 10f;
		}

		bool primeraVez = true;

		public override void Movimiento ()
		{
			if (primeraVez) {
				primeraVez = false;
				GetComponent<Animator> ().SetBool ("Visto", true);
			}
			if (!recibiendoGolpe) {
				Vector3 posicionJugador = player.transform.position;
				posicionJugador.y = transform.position.y;
				transform.position = Vector3.Lerp (transform.position, posicionJugador, 1 / retrasoAtaque);
			}

			if (player.transform.position.x > transform.position.x) {
				GetComponent<SpriteRenderer> ().flipX = true;
			} else {
				GetComponent<SpriteRenderer> ().flipX = false;
			}
		}

		public override void RecibeGolpe ()
		{
			if (!estadoInvencibilidad) {
				StartCoroutine (EstadoInvencible ());
				vidas--;
			}
			while (recibiendoGolpe) {
				tiempoKnockBack = Time.time;
				if (Time.time - tiempoKnockBack < tiempoInvencibilidad / 10) {
					if (golpePorLaDerecha) {
						if (GetComponent<SpriteRenderer> ().flipX) {
							transform.Translate (Vector3.left * distanciaKnockBack);
						} else {
							transform.Translate (Vector3.left * -distanciaKnockBack);
						}
					} else {
						if (GetComponent<SpriteRenderer> ().flipX) {
							transform.Translate (Vector3.right * distanciaKnockBack);
						} else {
							transform.Translate (Vector3.right * -distanciaKnockBack);
						}

					}
				} else {
					recibiendoGolpe = false;
				}
			}
		}

		void OnTriggerEnter2D (Collider2D other)
		{
			if (other.tag == "Bala") {
				golpePorLaDerecha = other.transform.position.x > transform.position.x;
				RecibeGolpe ();
			}
		}
	}
}