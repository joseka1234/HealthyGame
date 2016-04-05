using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Mensaje : MonoBehaviour
{

	public string texto;

	// Use this for initialization
	void Start ()
	{
		texto = texto.Replace ("\\n", "\n");
		GetComponentInChildren<Text> ().text = texto;
	}
}
