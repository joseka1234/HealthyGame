using UnityEngine;
using System.Collections;

public class Escalera : MonoBehaviour
{

	public float escalones = 2f;

	// Use this for initialization
	void Start ()
	{
		GetComponent<Renderer> ().material.mainTextureScale = new Vector2 (1f, escalones / 2f);
	}
	
	// Update is called once per frame
	void Update ()
	{
		// ONLY FOR TESTING
		// TODO: Borrar
		// GetComponent<Renderer> ().material.mainTextureScale = new Vector2 (1f, escalones / 2f);
	}
}
