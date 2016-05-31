using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pinchazo : MonoBehaviour
{
	private const float VELOCIDAD_DEDO = 3f;

	private float ComponenteR { get; set; }

	private Color ColorRueda { get; set; }

	public GameObject Rueda;
	public GameObject Dedo;
	public GameObject Aguja;
	public GameObject LIZonaPinchazo;
	public GameObject LDZonaPinchazo;
	public GameObject LIZonaDedo;
	public GameObject LDZonaDedo;

	private Vector3 vectorDesplazamiento;

	// Use this for initialization
	void Start ()
	{
		ColorRueda = Rueda.GetComponent<Image> ().color;
		ComponenteR = 255;
		vectorDesplazamiento = Vector3.left * VELOCIDAD_DEDO;
	}

	bool primeraVez = true;
	// Update is called once per frame
	void Update ()
	{
		if (ComponenteR > 0) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				ComponenteR -= 15;
				ColorRueda = new Color ((ComponenteR / 255f), 1, 0);
				Rueda.GetComponent<Image> ().color = ColorRueda;
			}
		} else {
			if (primeraVez) {
				Debug.Log ("El pinchazo ya está cargado!");
				primeraVez = false;
				StartCoroutine (FuncionesComunes.DesplazarInterfaz (
					Dedo, new Vector3 (-25, -175, 0), 300));
			} else {
				if (FuncionesComunes.EJECUTANDO) {
					return;
				}
				MoverDedo ();
				if (Input.GetMouseButtonDown (0)) {
					if (PinchazoAcierta ()) {
						// TODO: Aquí va la animación de pinchazo
						// TODO: Pensar si se va a hacer minijuego de poner la sangre en la tira.
						SceneManager.LoadScene (FuncionesComunes.GetEscenaPrevia ());
					}
				}
			}
		}
	}

	bool Direccion = true;

	/// <summary>
	/// Mueve el dedo
	/// </summary>
	private void MoverDedo ()
	{
		RectTransform posicion = Dedo.GetComponent<RectTransform> ();
		if (gameObject.transform.position.x >= LDZonaDedo.transform.position.x) {
			if (Direccion) {
				vectorDesplazamiento = Vector3.right * VELOCIDAD_DEDO;
				Direccion = false;
			}
		} else if (gameObject.transform.position.x <= LIZonaDedo.transform.position.x) {
			if (!Direccion) {
				vectorDesplazamiento = Vector3.left * VELOCIDAD_DEDO;
				Direccion = true;
			}
		}
		posicion.Translate (vectorDesplazamiento);
	}

	/// <summary>
	/// Indica si hemos acertado con el pinchazo
	/// </summary>
	/// <returns><c>true</c>, if acierta was pinchazoed, <c>false</c> otherwise.</returns>
	private bool PinchazoAcierta ()
	{
		return Aguja.transform.position.x > LIZonaPinchazo.transform.position.x &&
		Aguja.transform.position.x < LDZonaPinchazo.transform.position.x;
	}
}
