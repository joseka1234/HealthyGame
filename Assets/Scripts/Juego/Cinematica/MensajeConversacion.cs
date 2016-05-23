using UnityEngine;
using UnityEngine.UI;

public class MensajeConversacion : MonoBehaviour
{

	public string Texto;

	public string PathImagen;

	private Transform CuadroTexto { get; set; }

	public void Mostrar ()
	{
		CuadroTexto = GameObject.Find ("GameScene/UI/TextBox").transform;
		var Imagen = Resources.Load<Sprite> (PathImagen);
		Transform hijo;
		for (int i = 0; i < CuadroTexto.childCount; i++) {
			hijo = CuadroTexto.GetChild (i);
			switch (hijo.name) {
			case "TextoCaja":
				hijo.GetComponent<Text> ().text = Texto;
				break;
			case "Imagen":
				hijo.GetComponent<Image> ().sprite = Imagen;
				break;
			}
		}
	}
}
