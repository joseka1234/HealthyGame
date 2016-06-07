using UnityEngine;
using AssemblyCSharp;

namespace AssemblyCSharp
{
	public class TerminarAnimacion : StateMachineBehaviour
	{
		public GameObject prefabBala;

		// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
		// override public void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		//
		// }

		// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
		override public void OnStateUpdate (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			animator.SetBool (PlayerController.DISPARAR, false);
		}

		// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
		override public void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			Disparar ();	
		}

		/// <summary>
		/// Disparar
		/// </summary>
		public void Disparar ()
		{
			Transform naceBala = GameObject.Find ("NaceBalaPlayer").transform;
			GameObject balaClone = Instantiate (prefabBala, naceBala.position, Quaternion.identity) as GameObject;
			InformacionBala datosBala = balaClone.GetComponent<InformacionBala> ();
			balaClone.GetComponent<InformacionBala> ().posicionOrigen = naceBala.position;
			Rigidbody2D cuerpoBalaClone = balaClone.GetComponent<Rigidbody2D> ();
			if (PlayerController.face) {
				cuerpoBalaClone.velocity = new Vector2 (datosBala.velocidadBala, cuerpoBalaClone.velocity.y);
			} else {
				cuerpoBalaClone.velocity = new Vector2 (-datosBala.velocidadBala, cuerpoBalaClone.velocity.y);
				PlayerController.RotarObjeto (balaClone);
			}
			balaClone.transform.parent = GameObject.Find (PlayerController.BALAS).transform;
		}


		// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
		//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		//
		//}

		// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
		//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		//
		//}
	}

}