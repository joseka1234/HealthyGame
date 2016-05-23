using UnityEngine;

public class Destructible : MonoBehaviour
{
	// Destruimos el elemento destructible si colisiona con una bala
	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Bala") {
			Destroy (this.gameObject);
		}
	}
}
