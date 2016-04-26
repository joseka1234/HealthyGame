using UnityEngine;

namespace AssemblyCSharp
{
	public class EnemigoCaminante : Enemigo
	{
		public float speed = 10f;

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
				speed = -speed;
			} else if (!golpePorLaDerecha && !MoviendoHaciaDerecha ()) {
				speed = -speed;
			}
		}

		void OnTriggerEnter2D (Collider2D other)
		{
			if (other.tag == "Suelo") {
				speed = -speed;
			}
			if (other.tag == "Bala") {
				golpePorLaDerecha |= other.transform.position.x > transform.position.x;
				RecibeGolpe ();
				Destroy (other.gameObject);
			}
		}

		private bool MoviendoHaciaDerecha ()
		{
			return speed > 0;
		}
	}
}
