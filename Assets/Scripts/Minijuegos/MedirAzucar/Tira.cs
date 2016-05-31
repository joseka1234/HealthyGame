using UnityEngine;

public class Tira : MonoBehaviour
{

	private const float VELOCIDAD_TIRA = 3;
	public GameObject LSZonaTira;
	public GameObject LIZonaTira;
	public GameObject LSRanura;
	public GameObject LIRanura;

	private bool Direccion { get; set; }

	private Vector3 vectorDesplazamiento { get; set; }

	void Start ()
	{
		Direccion = true;
		vectorDesplazamiento = Vector3.up * VELOCIDAD_TIRA;
	}

	bool acertaste = false;

	void Update ()
	{
		if (acertaste) {
			return;
		}
		MoverTira ();
		if (Input.GetMouseButtonDown (0)) {
			if (TiraEnRanura ()) {
				StartCoroutine (FuncionesComunes.DesplazarInterfaz (
					GameObject.Find ("UI/ZonaGlucometro"),
					new Vector3 (-700, 0, 0),
					300));
				
				StartCoroutine (FuncionesComunes.DesplazarInterfaz (
					GameObject.Find ("UI/ZonaPinchazo"),
					Vector3.zero,
					300));
				
				Debug.Log ("Has acertado");
				// TODO: Aquí va la animación de la tira entrando
				acertaste = true;
			} else {
				Debug.Log ("Has fallado");
			}
		}
	}

	/// <summary>
	/// Mueve la tira de un lado a otro
	/// </summary>
	private void MoverTira ()
	{
		RectTransform posicion = gameObject.GetComponent<RectTransform> ();
		if (gameObject.transform.position.y >= LSZonaTira.transform.position.y) {
			if (Direccion) {
				vectorDesplazamiento = Vector3.down * VELOCIDAD_TIRA;
				Direccion = false;
			}
		} else if (gameObject.transform.position.y <= LIZonaTira.transform.position.y) {
			if (!Direccion) {
				vectorDesplazamiento = Vector3.up * VELOCIDAD_TIRA;
				Direccion = true;
			}
		}
		posicion.Translate (vectorDesplazamiento);
	}

	/// <summary>
	/// Indica si la tira está en la ranura
	/// </summary>
	/// <returns><c>true</c>, if en ranura was tiraed, <c>false</c> otherwise.</returns>
	private bool TiraEnRanura ()
	{
		return gameObject.transform.position.y <= LSRanura.transform.position.y &&
		gameObject.transform.position.y >= LIRanura.transform.position.y;
	}
}
