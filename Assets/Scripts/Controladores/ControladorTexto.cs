using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Threading;
using System.Runtime.ConstrainedExecution;

namespace AssemblyCSharp
{
	public class ControladorTexto : MonoBehaviour
	{
		private const string CAJA_TEXTO = "GameScene/UI/TextBox";

		/// <summary>
		/// Abre la caja de texto
		/// </summary>
		public void AbrirCajaTexto ()
		{
			GameObject.Find (CAJA_TEXTO).SetActive (true);
		}

		/// <summary>
		/// Cierra la caja de texto
		/// </summary>
		public void CerrarCajaTexto ()
		{
			GameObject.Find (CAJA_TEXTO).SetActive (false);
		}

		/// <summary>
		/// Muestra el texto en la caja de texto
		/// </summary>
		/// <param name="texto">Texto.</param>
		public void MostrarTexto (string texto)
		{
			AbrirCajaTexto ();
			GameObject.Find (CAJA_TEXTO).GetComponentInChildren<Text> ().text = texto;
		}
	}
}
