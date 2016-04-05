using UnityEngine;
using System.Collections;

namespace AssemblyCSharp
{
	public class EnemigoSaltador : Enemigo
	{
		public float saltoHorizontal = 10f;
		public float saltoVertical = 10f;
		public float esperaEntreSaltos = 10f;

		public override void Movimiento ()
		{
			if (EstaEnSuelo ()) {
				SaltoDiagonal ();
			}
		}

		private void SaltoDiagonal ()
		{
			Rigidbody2D body = GetComponent<Rigidbody2D> ();
			body.velocity = new Vector2 (-saltoHorizontal, saltoVertical);
		}

		private bool EstaEnSuelo ()
		{
			return GetComponent<Rigidbody2D> ().velocity.y == 0f;
		}

		public override void RecibeGolpe ()
		{
			// TODO: Implementar el knockback y tal
		}
	}
}