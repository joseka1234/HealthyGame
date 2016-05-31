using UnityEngine;

public class NoDestruir : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		DontDestroyOnLoad (gameObject);
	}
}
