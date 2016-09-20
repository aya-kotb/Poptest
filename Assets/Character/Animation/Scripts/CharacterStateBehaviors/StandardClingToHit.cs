using UnityEngine;
using System.Collections;

namespace Poptropica2.Characters {
	public class StandardClingToHit : CharacterStateBehavior {
		[Tooltip("Distance within which the character will be pulled back toward the hit position")]
		public float clingThreshhold = 0.1f;
		[Tooltip("How quickly the character will be pulled in toward the hit position")]
		public float clingSpeed = 3f;
		[Tooltip("How much force to apply to the character when the player tries to move horizontally")]
		public float lateralForce = 2000f;

		// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			Vector2 stoppedVelocity = new Vector2(0f, Rb.velocity.y);
			if (ValidVelocity(stoppedVelocity)) {
				Rb.velocity = stoppedVelocity;
			}
		}

		// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
		override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
			float speedHorizInput = animator.GetFloat(StandardAnimatorHashes.speedPctHoriz);
            if (speedHorizInput > 0.1f) { //Allow lateral movement
				Rb.AddForce(Vector2.right * lateralForce * speedHorizInput * Time.fixedDeltaTime * (Model.FacingRight ? 1f : -1f));
			} else if (Mathf.Abs(Model.HitPosition.x - Model.transform.position.x) > clingThreshhold) { //But if no lateral movement, cling to the center
				Model.transform.position = Vector3.MoveTowards(Model.transform.position,
																new Vector3(Model.HitPosition.x, Model.transform.position.y, Model.transform.position.z),
																Time.fixedDeltaTime * clingSpeed);	/**/
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