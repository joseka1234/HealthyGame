using UnityEngine;

namespace AssemblyCSharp
{
	public class EnemigoCaminante : Enemigo
	{
		public float speed = 10f;
		public float Azucar = 30f;

		void Start ()
		{
			SetAzucarProporcionada (Azucar);
		}

		public override void Movimiento ()
		{
			Rigidbody2D body = GetComponent<Rigidbody2D> ();
			body.velocity = new Vector2 (speed, body.velocity.y);
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

		private void CambiarDireccion ()
		{
			speed *= -1;
			if (gameObject.GetComponent<SpriteRenderer> ().flipX) {
				gameObject.GetComponent<SpriteRenderer> ().flipX = false;
			} else {
				gameObject.GetComponent<SpriteRenderer> ().flipX = true;
			}

		}

		private bool MoviendoHaciaDerecha ()
		{
			return speed > 0;
		}
	}
}
