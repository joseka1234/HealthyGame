using UnityEngine;
using UnityEngine.SceneManagement;

public class EscenaActual : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{	
		if (GameObject.FindGameObjectsWithTag ("IndicadorEscena").Length >= 2) {
			Destroy (GameObject.FindGameObjectsWithTag ("IndicadorEscena") [0]);
		} else {
			DontDestroyOnLoad (gameObject);
		}
	}

	/// <summary>
	/// Getter de la escena
	/// </summary>
	/// <returns>The scene.</returns>
	public string GetEscena ()
	{
		return gameObject.name;
	}

	/// <summary>
	/// Setters de la escena
	/// </summary>
	/// <param name="NombreEscena">Nombre escena.</param>
	public void SetEscena (string NombreEscena)
	{
		gameObject.name = NombreEscena;
	}

	public void SetEscena (Scene _Escena)
	{
		gameObject.name = _Escena.name;
	}
}
