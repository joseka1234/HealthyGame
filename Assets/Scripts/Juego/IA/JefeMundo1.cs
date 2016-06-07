using AssemblyCSharp;
using UnityEngine;

public class JefeMundo1 : Enemigo
{
	private float tiempo;
	public float saltoHorizontal = 10;
	public float saltoVertical = 30;
	public GameObject naceBala;
	public GameObject prefabBala;
	public float Azucar = 50f;

	void Start ()
	{
		SetAzucarProporcionada (Azucar);
		tiempo = Time.time;
	}

	public override void Movimiento ()
	{
		if (Time.time - tiempo >= 1.5f) {
			Disparar ();
			tiempo = Time.time;
			SaltoDiagonal (saltoHorizontal, saltoVertical);
			saltoHorizontal *= -1;
		}
	}

	public override void RecibeGolpe ()
	{
		// TODO: Implementar esto
	}

	private void Disparar ()
	{
		GameObject balaClone = Instantiate (prefabBala, naceBala.transform.position, Quaternion.identity) as GameObject;
		balaClone.tag = "BalaEnemigo";
		InformacionBala datosBala = balaClone.GetComponent<InformacionBala> ();
		balaClone.GetComponent<InformacionBala> ().posicionOrigen = naceBala.transform.position;
		Rigidbody2D cuerpoBalaClone = balaClone.GetComponent<Rigidbody2D> ();
		datosBala.SetAzucar (20);
		cuerpoBalaClone.velocity = new Vector2 (-datosBala.velocidadBala, cuerpoBalaClone.velocity.y);
		PlayerController.RotarObjeto (balaClone);
		balaClone.transform.parent = GameObject.Find (PlayerController.BALAS).transform;
	}

	private void SaltoDiagonal (float saltoHorizontal, float saltoVertical)
	{
		Rigidbody2D body = GetComponent<Rigidbody2D> ();
		body.velocity = new Vector2 (-saltoHorizontal, saltoVertical);
	}
}
