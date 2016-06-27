using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarFase : MonoBehaviour
{
	public string NombreFase;

	/// <summary>
	/// Cargamos la escena especificada cuando el jugador choca con el trigger.
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Player") {
			SceneManager.LoadScene (NombreFase);
		}
	}
}
