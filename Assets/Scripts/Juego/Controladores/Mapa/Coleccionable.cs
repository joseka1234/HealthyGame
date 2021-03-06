﻿using UnityEngine;
using AssemblyCSharp;

public class Coleccionable : MonoBehaviour
{

	public float VelocidadDeGiro = 2f;

	public string NombreComida = "Naranja";

	private float AzucarProporcionado { get; set; }

	void Start ()
	{
		AzucarProporcionado = InformacionJuego.Instancia.HidratosDeCarbono [NombreComida.ToLower ()];
	}

	void Update ()
	{
		transform.Rotate (Vector3.up * VelocidadDeGiro);
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Player") {
			PlayerController controlador = GameObject.Find (PlayerController.PLAYER).GetComponent<PlayerController> ();
			controlador.azucar += AzucarProporcionado;
			Destroy (this.gameObject);
		}
	}
}
