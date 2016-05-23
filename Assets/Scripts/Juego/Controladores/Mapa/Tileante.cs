using UnityEngine;
using System.Collections;

public class Tileante : MonoBehaviour
{

	public float numeroTilesX = 1;
	public float numeroTilesY = 1;

	// Use this for initialization
	void Start ()
	{
		GetComponent<Renderer> ().material.mainTextureScale = new Vector2 (numeroTilesX, numeroTilesY);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
