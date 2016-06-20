using UnityEngine;

namespace AssemblyCSharp
{
	public class EnemigoSeguidorAereo : Enemigo
	{
		public float retrasoAtaque = 100f;

		public override void Movimiento ()
		{
			Vector3 posicionJugador = player.transform.position;
			//posicionJugador.y = transform.position.y;
			transform.position = Vector3.Lerp (transform.position, posicionJugador, 1 / retrasoAtaque);
		}

		public override void RecibeGolpe ()
		{
			if (!estadoInvencibilidad) {
				StartCoroutine (EstadoInvencible ());
				vidas--;
			}
		}
	}
}
