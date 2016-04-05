using UnityEngine;
using System.Collections;
using System.Threading;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class EnemigoCaminante : Enemigo
	{
		public float speed = 10f;

		public override void Movimiento ()
		{
			Rigidbody2D body = GetComponent<Rigidbody2D> ();
			body.velocity = new Vector2 (-speed, body.velocity.y);
		}

		public override void RecibeGolpe ()
		{
			// TODO: Implementar el knockback y tal
		}

		void OnTriggerEnter2D (Collider2D other)
		{
			if (other.tag == "Suelo") {
				speed = -speed;
			}
		}
	}
}
