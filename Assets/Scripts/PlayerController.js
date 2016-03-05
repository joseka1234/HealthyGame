#pragma strict

private var playerAnimator : Animator;
private var body : Rigidbody2D;

var jumpForce : float;
var speed : float;
var gravity : float;
var drag : float;

private var face = true;


function Start () {
	playerAnimator = GetComponent(Animator);
	body = GetComponent(Rigidbody2D);
	body.gravityScale = gravity;
	body.drag = drag;
}

function FixedUpdate () {
	if (Input.GetKey(KeyCode.D)) {
		if(!face) {
			face = true;
			rotarPersonaje();
		}
		playerAnimator.SetBool("Walking", true);
		body.velocity.x = speed;
	}
	else if (Input.GetKey(KeyCode.A)) {
		if(face) {
			face = false;
			rotarPersonaje();
		}
		playerAnimator.SetBool("Walking", true);
		body.velocity.x = -speed;
	}
	else {
		playerAnimator.SetBool("Walking", false);
	}
}

/**
* Funcion que rota el personaje
*/
function rotarPersonaje () {
	transform.Rotate(Vector3(0, 180, 0));
}