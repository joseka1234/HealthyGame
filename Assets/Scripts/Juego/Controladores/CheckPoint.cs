using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using AssemblyCSharp;

public class CheckPoint : MonoBehaviour
{
	public GameObject ZonaRespawn;

	void Start ()
	{
		GameObject player = GameObject.Find ("GameScene/Player");
		if (player.transform.position.x > gameObject.transform.position.x
		    || player.transform.position.x == ZonaRespawn.transform.position.x) {
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		Debug.Log ("Chivato");
		if (other.name == "Player") {
			StartCoroutine (Guardar ());
		}
	}

	private IEnumerator Guardar ()
	{
		PlayerController.pausa = true;
		GameObject.Find ("GameScene/UI/Guardar").SetActive (true);
		GameObject.FindGameObjectWithTag ("Respawn").transform.position = ZonaRespawn.transform.position;
		FuncionesComunes.SetEscenaPrevia ();
		yield return new WaitForSeconds (2.5f);
		SceneManager.LoadScene ("MinijuegoMedirAzucar");
	}
}
