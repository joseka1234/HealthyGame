using UnityEngine;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class Texto : MonoBehaviour
	{
		public string nombreFichero;
		private const string PATH = "Assets/Textos/";

		public List<string> GetTextos ()
		{
			List<string> textos;
			textos = new List<string> ();
			string linea;
			StreamReader lector = new StreamReader (PATH + nombreFichero + ".txt", Encoding.Default);
			using (lector) {
				do {
					linea = lector.ReadLine ();
					if (linea != null) {
						textos.Add (linea);
					}
				} while (linea != null);
			}
			return textos;
		}
	}
}