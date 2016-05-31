using UnityEngine;
using System.Collections;

public class Tira : MonoBehaviour
{
	public GameObject LSZonaTira;
	public GameObject LIZonaTira;
	public GameObject LSRanura;
	public GameObject LIRanura;

	private bool Direccion { get; set; }

	private Vector3 vectorDesplazamiento { get; set; }

	void Start ()
	{
		Direccion = true;
		vectorDesplazamiento = Vector3.up;
	}

	void Update ()
	{
		MoverTira ();
		if (Input.GetMouseButtonDown (0)) {
			if (TiraEnRanura ()) {
				Debug.Log ("Has acertado");
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
				vectorDesplazamiento = Vector3.down;
				Direccion = false;
			}
		} else if (gameObject.transform.position.y <= LIZonaTira.transform.position.y) {
			if (!Direccion) {
				vectorDesplazamiento = Vector3.up;
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
