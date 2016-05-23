using UnityEngine;
using System.Collections;

public abstract class ElementoCinematica : MonoBehaviour
{
	public static bool Ejecutando;

	public abstract IEnumerator Mostrar ();
}
