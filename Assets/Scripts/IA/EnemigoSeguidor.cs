using UnityEngine;
using System.Collections;

namespace AssemblyCSharp
{
	public class EnemigoSeguidor : Enemigo
	{
		public float retrasoAtaque = 100f;

		public override void Movimiento ()
		{
			Vector3 posicionJugador = player.transform.position;
			posicionJugador.y = transform.position.y;
			transform.position = Vector3.Lerp (transform.position, posicionJugador, 1 / retrasoAtaque);
		}

		public override void RecibeGolpe ()
		{
			// TODO: Implementar el knockback y tal
		}
	}
}