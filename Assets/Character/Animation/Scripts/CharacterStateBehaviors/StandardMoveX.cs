using UnityEngine;
using System.Collections;

namespace Poptropica2.Characters {

	/// <summary>This is not unified with StandardMoveY because this has to account for facing direction, whereas the latter does not.</summary>
	public class StandardMoveX : CharacterStateBehavior {
		public float speedLimit;

		// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
		//override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		//}

		// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			float sign = Model.FacingRight ? 1f : -1f;
			//Rb.velocity = Model.runSpeed * animator.GetFloat(hashSpeedPctHoriz) * (Vector2.right * sign);
			if (Mathf.Abs(Rb.velocity.x) < speedLimit || Mathf.Sign(Rb.velocity.x) != sign) {
				Vector2 moveForce = Vector2.right * sign * Model.runAccel * animator.GetFloat(StandardAnimatorHashes.speedPctHoriz) * Time.fixedDeltaTime * 1000;
				//Debug.Log("Moving" + moveForce);
				Rb.AddForce(moveForce);
				if (Mathf.Abs(Rb.velocity.x) > speedLimit) {    //Correct sudden overacceleration
					//Debug.Log("Too fast:" + Rb.velocity.x + " corrected to " + (speedLimit * sign));
					Rb.velocity = new Vector2(sign * speedLimit, Rb.velocity.y);
				}
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