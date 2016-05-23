using UnityEngine;
using System.Collections;

public class Conversacion : ElementoCinematica
{
	public MensajeConversacion[] Mensajes;

	public override IEnumerator Mostrar ()
	{
		Ejecutando = true;
		GameObject.Find ("GameScene/UI/TextBox").SetActive (true);
		foreach (MensajeConversacion mensaje in Mensajes) {
			mensaje.Mostrar ();
			yield return new WaitForSeconds (TiempoEspera (mensaje));
		}
		GameObject.Find ("GameScene/UI/TextBox").SetActive (false);
		Ejecutando = false;
	}

	private float TiempoEspera (MensajeConversacion mensaje)
	{
		
		return ((float)mensaje.Texto.Split (new char[]{ ' ' }).Length / 200f) * 60f * 1.5f;
	}
}
