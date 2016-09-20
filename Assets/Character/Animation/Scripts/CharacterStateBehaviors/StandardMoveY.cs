using UnityEngine;
using System.Collections;

namespace Poptropica2.Characters {
	/// <summary>This is not unified with StandardMoveX because this does not have to account for horizontal facing direction.</summary>
	public class StandardMoveY : CharacterStateBehavior {
		[Tooltip("Positive for moving up, negative for moving down.")]
		public float speedLimit;

		// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
		//override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		//
		//}

		// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			//Only add force if between zero and the speed limit, whether positive or negative.
			if ((speedLimit > 0 && Rb.velocity.y < speedLimit) || (speedLimit < 0 && Rb.velocity.y > speedLimit)) {
                Vector2 moveForce = Vector2.up * Model.runAccel * animator.GetFloat(StandardAnimatorHashes.speedPctVert) * Time.fixedDeltaTime * 1000;
				Rb.AddForce(moveForce);
				//Debug.Log("Vertical move: " + moveForce);
			}
			

		}

		// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
		//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		//
		//}

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