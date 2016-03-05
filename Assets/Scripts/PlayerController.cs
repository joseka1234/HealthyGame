using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	Animator playerAnimator { get; set; }
	
	private Rigidbody2D body { get; set; }
	public float jumpForce;
	public float speed;
	
	public float gravity;
	public float drag;
	
	private bool face { get; set; }
	
	// Use this for initialization
	void Start () {
		playerAnimator = GetComponent<Animator> ();
		face = true;
		body = GetComponent<Rigidbody2D>();
		body.gravityScale = gravity;
		body.drag = drag;
	}
	
	// FixedUpdate is called once per frame with physics
	void FixedUpdate () {
		// Movimiento izquierda
		if(Input.GetKey(KeyCode.D)) {
			if(!face) {
				face = true;
				transform.Rotate (new Vector3(0, 180, 0));
			}
			playerAnimator.SetBool("Walking", true);
			body.velocity = new Vector2(speed, body.velocity.y);
		}
		else if(Input.GetKey(KeyCode.A)) {
			if(face) {
				face = false;
				transform.Rotate (new Vector3(0, 180, 0));
			}
			playerAnimator.SetBool("Walking", true);
			body.velocity = new Vector2(-speed, body.velocity.y);
		}
		else {
			playerAnimator.SetBool("Walking", false);
		}
	}
}