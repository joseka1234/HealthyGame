using UnityEngine;
using UnityEngine.SceneManagement;

public class Botones : MonoBehaviour
{

	public void ComenzarJuego ()
	{
		SceneManager.LoadScene ("Juego");
	}

	public void Creditos ()
	{
		SceneManager.LoadScene ("Creditos");
	}

	public void Salir ()
	{
		Application.Quit ();
	}
}
