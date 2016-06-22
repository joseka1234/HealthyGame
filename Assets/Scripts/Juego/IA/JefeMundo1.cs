using AssemblyCSharp;
using UnityEngine;

public class JefeMundo1 : Enemigo
{
	private float tiempo;
	public GameObject naceBala;
	public GameObject prefabBala;
	public float Azucar = 50f;

	// TODO: Mirar por que el jefazo no salta!

	void Start ()
	{
		SetAzucarProporcionada (Azucar);
		tiempo = Time.time;
	}

	public override void Movimiento ()
	{
		if (EnSuelo ()) {
			GetComponent<Animator> ().SetBool ("Salto", false);
		}
		if (Time.time - tiempo >= 1.5f) {
			Disparar ();
			tiempo = Time.time;
			// FIXME: Arreglar este boss que está rotisimo!!
			GetComponent<Animator> ().SetBool ("Salto", true);
			//SaltoDiagonal (saltoHorizontal, saltoVertical);
		}
	}

	public override void RecibeGolpe ()
	{
		if (!estadoInvencibilidad) {
			StartCoroutine (EstadoInvencible ());
			vidas--;
		}
	}

	private void Disparar ()
	{
		const string BOSS = "Agregados/Enemigos/JefeMundo1";
		// TODO: Hacer que el disparo vaya dirigido siempre hacia el jugador!
		GameObject balaClone = Instantiate (prefabBala, naceBala.transform.position, Quaternion.identity) as GameObject;
		balaClone.tag = "BalaEnemigo";
		InformacionBala datosBala = balaClone.GetComponent<InformacionBala> ();
		balaClone.GetComponent<InformacionBala> ().posicionOrigen = naceBala.transform.position;
		Rigidbody2D cuerpoBalaClone = balaClone.GetComponent<Rigidbody2D> ();
		datosBala.SetAzucar (20);

		Vector3 vectorDireccion = player.transform.position - GameObject.Find (BOSS + "/NaceBala").transform.position;
		vectorDireccion.Normalize ();

		cuerpoBalaClone.velocity = new Vector2 (datosBala.velocidadBala * vectorDireccion.x, datosBala.velocidadBala * vectorDireccion.y);
		PlayerController.RotarObjeto (balaClone);
		balaClone.transform.parent = GameObject.Find (PlayerController.BALAS).transform;
	}

	private bool EnSuelo ()
	{
		return GetComponent<Rigidbody2D> ().velocity.y == 0;
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Bala") {
			RecibeGolpe ();
		}
	}
}
