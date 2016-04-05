using UnityEngine;
using System.Collections;
using System.Security.Cryptography;
using System.Collections.Generic;
using UnityEngine.UI;

// TODO: Añadir la vida y el restado de vidas
// TODO: Añadir el contador de coleccionables

namespace AssemblyCSharp
{
	public class PlayerController : MonoBehaviour
	{
		public const string WALKING = "Walking";
		public const string GROUNDED = "Grounded";
		public const string DISPARAR = "Disparar";
		public const string BALAS = "GameScene/Balas";
		public const string TEXTO_GUI = "GameScene/UI/TextBox";

		public float velocidad = 10f;
		public float fuerzaSalto = 10f;
		public float gravedad = 20f;
		public float agarre = 5f;
		public float distanciaDisparo = 10f;
		public int vidas = 3;

		public GameObject prefabBala;

		private Animator animaciones { get; set; }

		private bool controlesActivados { get; set; }

		private Rigidbody2D body { get; set; }

		public static bool face { get; set; }

		private bool cajaTextoAbierta { get; set; }

		private ControladorTexto controladorDeTexto { get; set; }

		private List<string> textosAMostrar;

		public LayerMask layerSuelo;

		private bool frenteAEscalera { get; set; }

		// Use this for initialization
		void Start ()
		{
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
			// Controles de gameplay
			if (controlesActivados) {
				if (Input.GetKey (KeyCode.A)) {
					CaminarIzquierda ();
				} else if (Input.GetKey (KeyCode.D)) {
					CaminarDerecha ();
				} else {
					animaciones.SetBool (WALKING, false);
				}
				if (Input.GetKey (KeyCode.W)) {
					if (frenteAEscalera) {
						SubirEscalera ();
					} else {
						if (EstaEnSuelo ()) {
							Saltar ();
						}
					}
				}
				if (Input.GetKey (KeyCode.S)) {
					if (frenteAEscalera) {
						BajarEscalera ();
					}
				}
				if (Input.GetKeyDown (KeyCode.J)) {
					animaciones.SetBool (DISPARAR, true);
				}
				if (Input.GetKeyDown (KeyCode.R)) {
					Respawn ();
				}
				if (EstaEnSuelo ()) {
					animaciones.SetBool (GROUNDED, true);
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
			transform.position = GameObject.Find ("GameScene/Mapa/Respawn").transform.position;
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
			animaciones.SetBool (GROUNDED, false);
			body.velocity = new Vector2 (body.velocity.x, fuerzaSalto);
		}

		/// <summary>
		/// Método que rota un obejo
		/// </summary>
		public static void RotarObjeto (GameObject objeto)
		{
			objeto.transform.Rotate (new Vector2 (0, 180));
		}

		/// <summary>
		/// Lanza el muestreo de las cajas de texto con las que choquemos
		/// </summary>
		/// <param name="other">Other.</param>
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
				Respawn ();
			} else if (other.tag == "Escalera") {
				body.gravityScale = 0;
				frenteAEscalera = true;
				body.drag = agarre * 3;
				animaciones.SetBool (GROUNDED, true);
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

		private void SetIDLE ()
		{
			animaciones.SetBool (WALKING, false);
			animaciones.SetBool (GROUNDED, true);
		}

		/// <summary>
		/// Método que dice si el personaje está en el suelo
		/// </summary>
		/// <returns><c>true</c>, if grounded was ised, <c>false</c> otherwise.</returns>
		private bool EstaEnSuelo ()
		{
			return body.velocity.y <= 0.01f && body.velocity.y >= -0.01f;
		}
	}
}