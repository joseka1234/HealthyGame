using UnityEngine;
using System.Collections;

namespace AssemblyCSharp
{
	public class SeguirPersonaje : MonoBehaviour
	{
		public GameObject personaje;
		public float Ajustador = 1f;
	
		// Update is called once per frame
		void Update ()
		{
			float posicionX = personaje.transform.position.x;
			float posicionY = transform.position.y;
			if (personaje.transform.position.y > transform.position.y + Ajustador || personaje.transform.position.y < transform.position.y - Ajustador) {
				posicionY = personaje.transform.position.y;
			}
			transform.position = Vector3.Lerp (transform.position, new Vector3 (posicionX, posicionY, transform.position.z), Time.deltaTime * 2);
			// transform.position = new Vector3 (posicionX, posicionY, transform.position.z);
		}
	}
}