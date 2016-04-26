using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


// TODO: Problema de las paredes (Parcialmente solucionado, ahora resbala poco a poco, se podría poner una animación y lehto!)
namespace AssemblyCSharp
{
	public class PlayerController : MonoBehaviour
	{
		private const float EPSILON = 0.01f;

		public const string WALKING = "Walking";
		public const string GROUNDED = "Grounded";
		public const string DISPARAR = "Disparar";
		public const string BALAS = "GameScene/Balas";
		public const string TEXTO_GUI = "GameScene/UI/TextBox";
		public const string HUD = "GameScene/UI/HUD";
		public const string PLAYER = "GameScene/Player";

		public float velocidad = 10f;
		public float fuerzaSalto = 10f;
		public float gravedad = 20f;
		public float agarre = 5f;
		public float tiempoInvencibilidad = 1f;
		public float distanciaKnockBack = 0.2f;

		public int vidas = 3;
		public int puntuacion = 0;

		public static bool face;
		public static bool enSuelo;


		private Animator animaciones { get; set; }

		private Rigidbody2D body { get; set; }

		private ControladorTexto controladorDeTexto { get; set; }

		private List<string> textosAMostrar;

		private static bool invencible { get; set; }

		private static bool recibiendoGolpe { get; set; }

		private bool frenteAEscalera { get; set; }

		private bool cajaTextoAbierta { get; set; }

		private bool muerto { get; set; }

		private bool controlesActivados { get; set; }

		private float tiempoKnockBack { get; set; }

		private bool golpePorLaDerecha { get; set; }

		// Use this for initialization
		void Start ()
		{
			enSuelo = true;
			invencible = false;
			muerto = false;
			animaciones = GetComponent<Animator> ();
			body = GetComponent<Rigidbody2D> ();
			textosAMostrar = new List<string> ();
			body.gravityScale = gravedad;
			body.drag = agarre;
			face = true;
			controlesActivados = true;
			controladorDeTexto = new ControladorTexto ();
			frenteAEscalera = false;
		}

		// Update is called once per frame
		void FixedUpdate ()
		{

			if (enSuelo) {
				animaciones.SetBool (GROUNDED, true);
			} else {
				animaciones.SetBool (GROUNDED, false);
			}

			if (vidas <= 0) {
				Morir ();
			}

			GameObject.Find (HUD + "/Puntuacion").GetComponentInChildren<Text> ().text = puntuacion.ToString ();

			ControlPersonaje ();
			CompruebaBalas ();
		}

		/// <summary>
		/// Comprueba la distancia que han recorrido las balas para destruirlas
		/// </summary>
		private static void CompruebaBalas ()
		{
			Transform[] balas = GameObject.Find (BALAS).GetComponentsInChildren<Transform> ();
			InformacionBala componenteBala;
			float distanciaBala;
			if (balas.Length > 1) {
				foreach (Transform bala in balas) {
					if (bala.name != "Balas") {
						componenteBala = bala.gameObject.GetComponent<InformacionBala> ();
						distanciaBala = Vector3.Distance (bala.transform.position, componenteBala.posicionOrigen);
						if (distanciaBala >= componenteBala.distanciaDesaparicion) {
							Destroy (bala.gameObject);
						}
					}
				}
			}
		}

		/// <summary>
		/// Índica la distancia de un objeto al jugador
		/// </summary>
		/// <returns>The A jugador.</returns>
		/// <param name="objeto">Objeto.</param>
		private float DistanciaAJugador (Transform objeto)
		{
			return DistanciaAOrigen (transform, objeto.transform);
		}

		/// <summary>
		/// Devuelve la distancia de un punto a otro
		/// </summary>
		/// <returns>The A origen.</returns>
		/// <param name="origen">Origen.</param>
		/// <param name="destino">Destino.</param>
		private float DistanciaAOrigen (Transform origen, Transform destino)
		{
			return Vector2.Distance (origen.position, destino.position);
		}

		/// <summary>
		/// Mecánicas de control del personaje
		/// </summary>
		private void ControlPersonaje ()
		{

			// TODO: Mirar si se ha arreglado el recibir el golpe
			if (recibiendoGolpe) {
				if (controlesActivados) {
					tiempoKnockBack = Time.time;
					controlesActivados = false;
					SetIDLE ();
				}
				if (Time.time - tiempoKnockBack < tiempoInvencibilidad / 10) {
					if (golpePorLaDerecha) {
						if (face) {
							transform.Translate (Vector3.left * distanciaKnockBack);
						} else {
							transform.Translate (Vector3.left * -distanciaKnockBack);
						}
					} else {
						if (face) {
							transform.Translate (Vector3.right * distanciaKnockBack);
						} else {
							transform.Translate (Vector3.right * -distanciaKnockBack);
						}

					}
				} else {
					recibiendoGolpe = false;
					controlesActivados = true;
				}
			}
			// Controles de gameplay
			// TODO: Arreglar el problema con quedarse pegado a las paredes
			if (controlesActivados && !muerto) {
				if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)) {
					if (Atascado () && !face) {
						return;
					}
					CaminarIzquierda ();
				} else if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)) {
					if (Atascado () && face) {
						return;
					}
					CaminarDerecha ();
				} else {
					animaciones.SetBool (WALKING, false);
				}
				if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.UpArrow)) {
					if (frenteAEscalera) {
						SubirEscalera ();
					} else {
						if (enSuelo) {
							Saltar ();
						}
					}
				}
				if (Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.DownArrow)) {
					if (frenteAEscalera) {
						BajarEscalera ();
					}
				}
				if (Input.GetKeyDown (KeyCode.J) || Input.GetKey (KeyCode.Space)) {
					animaciones.SetBool (DISPARAR, true);
				}
			}
			// Controles externos a gameplay
			if (Input.GetKeyDown (KeyCode.Space) && cajaTextoAbierta) {
				if (textosAMostrar.Count <= 0) {
					controladorDeTexto.CerrarCajaTexto ();
					cajaTextoAbierta = false;
					controlesActivados = true;
				} else {
					MostrarTexto ();
				}
			}
			if (Input.GetKeyDown (KeyCode.R) && muerto) {
				// TODO: Arreglar problema con el reseteo de las plataformas que se caen.
				SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
			}
		}

		private bool Atascado ()
		{
			return !enSuelo && (System.Math.Abs (body.velocity.y) < EPSILON);
		}

		private void SubirEscalera ()
		{
			body.velocity = new Vector2 (body.velocity.x, velocidad);
		}

		private void BajarEscalera ()
		{
			body.velocity = new Vector2 (body.velocity.x, -velocidad);
		}

		private void Respawn ()
		{
			ActivarPlataformasInestables ();
			transform.position = GameObject.Find ("GameScene/Mapa/Respawn").transform.position;
		}

		private void ActivarPlataformasInestables ()
		{
			GameObject suelo = GameObject.Find ("GameScene/Mapa/Suelo");
			int hijos = suelo.transform.childCount;
			Transform hijo;
			for (int i = 0; i < hijos; i++) {
				hijo = suelo.transform.GetChild (i);
				if (hijo.GetComponent<PlataformaInestable> () != null && !hijo.gameObject.activeSelf) {
					hijo.GetComponent<PlataformaInestable> ().Resetear ();
					hijo.gameObject.SetActive (true);
				}
			}
		}

		/// <summary>
		/// Mostrar texto en el GUI
		/// </summary>
		private void MostrarTexto ()
		{
			Text textoEnPantalla = GameObject.Find (TEXTO_GUI).GetComponentInChildren<Text> ();
			textoEnPantalla.text = textosAMostrar [0];
			textosAMostrar.RemoveAt (0);
		}

		/// <summary>
		/// Caminar hacia la izquierda
		/// </summary>
		private void CaminarIzquierda ()
		{
			animaciones.SetBool (WALKING, true);
			if (face) {
				face = false;
				RotarObjeto (transform.gameObject);
			}
			body.velocity = new Vector2 (-velocidad, body.velocity.y);
		}

		/// <summary>
		/// Caminar hacia la derecha
		/// </summary>
		private void CaminarDerecha ()
		{
			animaciones.SetBool (WALKING, true);
			if (!face) {
				face = true;
				RotarObjeto (transform.gameObject);
			}
			body.velocity = new Vector2 (velocidad, body.velocity.y);
		}

		/// <summary>
		/// Saltar
		/// </summary>
		private void Saltar ()
		{
			body.velocity = new Vector2 (body.velocity.x, fuerzaSalto);
		}

		/// <summary>
		/// Método que rota un obejo
		/// </summary>
		public static void RotarObjeto (GameObject objeto)
		{
			objeto.transform.Rotate (new Vector2 (0, 180));
		}

		private void Morir ()
		{
			muerto = true;
			MostrarGameOver ();
		}

		private void MostrarGameOver ()
		{
			GameObject gameOver = GameObject.Find ("GameScene/UI/FondoNegro");
			gameOver.SetActive (true);
			StartCoroutine (FuncionesComunes.DesplazarInterfaz (gameOver, Vector3.zero, 1000f));
		}

		private void SetIDLE ()
		{
			animaciones.SetBool (WALKING, false);
			animaciones.SetBool (GROUNDED, true);
		}

		void OnCollisionEnter2D (Collision2D other)
		{
			if (other.collider.tag == "Enemigo" && !invencible) {
				golpePorLaDerecha |= other.transform.position.x > transform.position.x;
				RecibeGolpe ();
				other.collider.GetComponent<Enemigo> ().RecibeGolpe ();
			}
		}

		void OnTriggerEnter2D (Collider2D other)
		{
			if (other.tag == "TextTrigger") {
				controladorDeTexto.AbrirCajaTexto ();

				SetIDLE ();
				Texto textoTrigger = other.gameObject.GetComponent<Texto> ();
				textosAMostrar = textoTrigger.GetTextos ();

				MostrarTexto ();

				Destroy (other.gameObject);
				cajaTextoAbierta = true;
				controlesActivados = false;
			} else if (other.tag == "Muerte") {
				Morir ();
			}
		}

		void OnTriggerExit2D (Collider2D other)
		{
			if (other.tag == "Escalera") {
				frenteAEscalera = false;
				body.drag = agarre;
				body.gravityScale = gravedad;
			}
		}

		void OnTriggerStay2D (Collider2D other)
		{
			if (other.tag == "Escalera") {
				body.gravityScale = 0;
				frenteAEscalera = true;
				body.drag = agarre * 3;
				animaciones.SetBool (GROUNDED, true);
			}
		}

		private void RecibeGolpe ()
		{
			vidas--;
			StartCoroutine (ActivaDesactivaInvencibilidad ());
			recibiendoGolpe = true;
		}

		private IEnumerator ActivaDesactivaInvencibilidad ()
		{
			Color colorJugador = GetComponent<SpriteRenderer> ().color;
			invencible = true;
			GetComponent<SpriteRenderer> ().color = new Color (colorJugador.r, colorJugador.g, colorJugador.b, 0.5f);
			yield return new WaitForSeconds (tiempoInvencibilidad);
			invencible = false;
			GetComponent<SpriteRenderer> ().color = new Color (colorJugador.r, colorJugador.g, colorJugador.b, 1f);
		}
	}
}