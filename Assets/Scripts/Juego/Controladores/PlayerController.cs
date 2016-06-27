using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

// TODO: Problema de las paredes (Parcialmente solucionado, ahora resbala poco a poco, se podría poner una animación y lehto!)
// TODO: Implementar lo de la insulina que se quede registrada entre distintos niveles
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

		public const float MAX_AZUCAR = 250;
		public const float MIN_AZUCAR = 20;

		public float DuracionSalto = 1.5f;
		public float gravedad = 20f;
		public float agarre = 5f;
		public float tiempoInvencibilidad = 1f;
		public float distanciaKnockBack = 0.2f;
		public float ProporcionAnchoSalto = 3f;
		public float ProporcionAltoSalto = 2f;

		public float azucar = 130;

		public static bool face;
		public static bool enSuelo;
		public static bool pausa;

		private float AnchoSalto { get; set; }

		private float AltoSalto { get; set; }

		public float Velocidad;

		public float FuerzaSalto;

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

		public GameObject BarraAzucar;

		private float DescensoAzucar { get; set; }

		void Awake ()
		{
			animaciones = GetComponent<Animator> ();
		}

		// Use this for initialization
		void Start ()
		{
			GameObject[] RespawnPositions = GameObject.FindGameObjectsWithTag ("Respawn");
			gameObject.transform.position = RespawnPositions [RespawnPositions.Length - 1].transform.position;
			pausa = false;
			enSuelo = true;
			invencible = false;
			muerto = false;

			body = GetComponent<Rigidbody2D> ();
			textosAMostrar = new List<string> ();
			body.gravityScale = gravedad;
			body.drag = agarre;
			face = true;
			controlesActivados = true;
			controladorDeTexto = new ControladorTexto ();
			frenteAEscalera = false;
			CalculaVelocidadYSalto ();
			CalculaAnchoYAltoSalto ();
			DescensoAzucar = Time.time;
		}

		// Update is called once per frame
		void FixedUpdate ()
		{
			if (Time.time - DescensoAzucar >= 1f) {
				azucar--;
				DescensoAzucar = Time.time;
			}
			SetAzucar ();
			if (azucar <= MIN_AZUCAR || azucar >= MAX_AZUCAR) {
				Morir ();
			}
			if (enSuelo) {
				animaciones.SetBool (GROUNDED, true);
			} else {
				animaciones.SetBool (GROUNDED, false);
			}
			if (pausa) {
				SetIDLE ();
				return;
			}
			CalculaVelocidadYSalto ();
			CalculaAnchoYAltoSalto ();

			ControlPersonaje ();
			CompruebaBalas ();
		}

		private void SetAzucar ()
		{
			GameObject.Find ("GameScene/UI/HUD/Marco/NivelAzucar/Text").GetComponent<Text> ().text = azucar.ToString ();
			if (azucar > 160 || azucar < 75) {
				BarraAzucar.GetComponent<Image> ().color = Color.red;
			} else {
				BarraAzucar.GetComponent<Image> ().color = Color.green;
			}
			BarraAzucar.GetComponent<RectTransform> ().localScale = new Vector3 (azucar / MAX_AZUCAR, 1, 1);
		}

		/// <summary>
		/// Calcula la velocidad y la fuerza de salto del personaje
		/// </summary>
		private void CalculaVelocidadYSalto ()
		{
			Velocidad = AnchoSalto / DuracionSalto;
			FuerzaSalto = AltoSalto - ((1f / 2f) * (-gravedad) * Mathf.Pow (DuracionSalto, 2));
		}

		/// <summary>
		/// Calcula el ancho y el alto máximo del salto
		/// </summary>
		private void CalculaAnchoYAltoSalto ()
		{
			AnchoSalto = GetComponent<SpriteRenderer> ().bounds.size.x * ProporcionAnchoSalto;
			AltoSalto = GetComponent<SpriteRenderer> ().bounds.size.y * ProporcionAltoSalto;
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
			return Vector2.Distance (transform.position, objeto.transform.position);
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
				FuncionesComunes.SetEscenaPrevia ();
				SceneManager.LoadScene ("MinijuegoInsulina");
			}
		}

		private bool Atascado ()
		{
			return !enSuelo && (Math.Abs (body.velocity.y) < EPSILON);
		}

		private void SubirEscalera ()
		{
			body.velocity = new Vector2 (body.velocity.x, Velocidad);
		}

		private void BajarEscalera ()
		{
			body.velocity = new Vector2 (body.velocity.x, -Velocidad);
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
			body.AddForce (Vector2.left * Velocidad * 8);
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
			body.AddForce (Vector2.right * Velocidad * 8);
			// body.velocity = new Vector2 (Velocidad, body.velocity.y);
		}

		/// <summary>
		/// Saltar
		/// </summary>
		private void Saltar ()
		{
			body.velocity = new Vector2 (body.velocity.x, FuerzaSalto);
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
			StartCoroutine (FuncionesComunes.EjecutarAlFinal (2.5f, () => {
				FuncionesComunes.SetEscenaPrevia ();
				SceneManager.LoadScene ("MinijuegoInsulina");
			}));
		}

		private void SetIDLE ()
		{
			animaciones.SetBool (WALKING, false);
			animaciones.SetBool (GROUNDED, true);
		}

		void OnCollisionEnter2D (Collision2D other)
		{
			if ((other.collider.tag == "Enemigo") && !invencible) {
				azucar += other.gameObject.GetComponent<Enemigo> ().GetAzucarProporcionada ();
				golpePorLaDerecha = other.transform.position.x > transform.position.x;
				RecibeGolpe ();
				other.collider.GetComponent<Enemigo> ().RecibeGolpe ();
			}
		}

		void OnTriggerEnter2D (Collider2D other)
		{
			if (other.tag == "Muerte") {
				Morir ();
			} else if (other.tag == "BalaEnemigo") {
				azucar += other.GetComponent<InformacionBala> ().GetAzucar ();
				golpePorLaDerecha = other.transform.position.x > transform.position.x;
				RecibeGolpe ();
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
				SetIDLE ();
			}
		}

		private void RecibeGolpe ()
		{
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