using UnityEngine;
using System.Collections;

namespace AssemblyCSharp
{
	public class SeguirPersonaje : MonoBehaviour
	{
		public GameObject personaje;
	
		// Update is called once per frame
		void Update ()
		{
			transform.position = new Vector3 (personaje.transform.position.x,
				personaje.transform.position.y, transform.position.z);
	
		}
	}
}