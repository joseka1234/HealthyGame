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
			SaltoDiagonal (saltoHorizontal, saltoVertical);
			saltoHorizontal *= -1;
		}
	}

	public override void RecibeGolpe ()
	{
		if (!estadoInvencibilidad) {
			StartCoroutine (EstadoInvencible ());
			vidas--;
		}
	}

	/// <summary>
	/// Disparar
	/// </summary>
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

	private bool EnSuelo ()
	{
		return GetComponent<Rigidbody2D> ().velocity.y == 0;
	}

	/// <summary>
	/// Saltos the diagonal.
	/// </summary>
	/// <param name="saltoHorizontal">Salto horizontal.</param>
	/// <param name="saltoVertical">Salto vertical.</param>
	private void SaltoDiagonal (float saltoHorizontal, float saltoVertical)
	{
		GetComponent<Animator> ().SetBool ("Salto", true);
		Rigidbody2D body = GetComponent<Rigidbody2D> ();
		body.velocity = new Vector2 (-saltoHorizontal, saltoVertical);
	}
}
