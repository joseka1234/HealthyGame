using System.Collections;
using UnityEngine;

public class Animacion : ElementoCinematica
{

	public Animator Animaciones;

	// public Movimiento[] Movimientos;

	/*
	public override IEnumerator Mostrar ()
	{
		Ejecutando = true;
		foreach (Movimiento movimiento in Movimientos) {
			StartCoroutine (movimiento.Mostrar ());
			while (movimiento.Ejecutando) {
				yield return new WaitForSeconds (0.01f);
			}
		}
		Ejecutando = false;
	}
	*/

	public override IEnumerator Mostrar ()
	{
		Ejecutando = true;
		Animaciones.SetBool ("Inicio", true);
		while (!Animaciones.GetBool ("Fin")) {
			yield return new WaitForSeconds (0.01f);
		}
		Ejecutando = false;
	}
}
