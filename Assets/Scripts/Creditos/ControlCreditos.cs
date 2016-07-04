using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ControlCreditos : MonoBehaviour
{
	void Start ()
	{
		StartCoroutine (CambiarAMenu ());
	}

	private IEnumerator CambiarAMenu ()
	{
		yield return new WaitForSeconds (5f);
		SceneManager.LoadScene ("MenuPrincipal");
	}
}
