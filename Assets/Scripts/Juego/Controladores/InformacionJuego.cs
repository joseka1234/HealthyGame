using UnityEngine;
using System.Collections.Generic;

public class InformacionJuego : MonoBehaviour
{

	private static InformacionJuego instancia = null;

	protected InformacionJuego ()
	{
	}

	public static InformacionJuego Instancia {
		get { 
			if (instancia == null) {
				instancia = new InformacionJuego ();
			}
			instancia.Inicializar ();
			return instancia;
		}
	}

	public Dictionary<string, float> HidratosDeCarbono;

	public void Inicializar ()
	{
		string line;
		string[] aux;

		HidratosDeCarbono = new Dictionary<string, float> ();
		System.IO.StreamReader file = new System.IO.StreamReader ("Assets/FicheroHidratos.info");
		while ((line = file.ReadLine ()) != null) {
			aux = line.Split (',');
			HidratosDeCarbono [aux [0].ToLower ()] = float.Parse (aux [1]);
		}

		file.Close ();
	}
}
