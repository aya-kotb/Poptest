using UnityEngine;
using System.Collections;

namespace Poptropica2.Characters
{
    /// <summary>
    /// Standard gravity scaling.
    /// Changes the gravity of the character's rigidbody for a short amount of time for the required jump effect
    /// </summary>
    public class StandardGravityScaling:CharacterPhysicsBehavior
    {
        float gravityScale;

        public float GravityScale
        {
            get
            {
                return gravityScale;
            }
            set
            {
                gravityScale = value;
            }
        }
            
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex = 0)
        {
            Rigidbody2D rigidBody = parentStateBehavior.Rb;
            rigidBody.gravityScale = gravityScale;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex = 0)
        {
            Rigidbody2D rigidBody = parentStateBehavior.Rb;
            if (gravityScale == 0f && Mathf.Approximately(animator.GetFloat(StandardAnimatorHashes.speedPctVert), 0f))
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0f);
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex = 0)
        {
            Rigidbody2D rigidBody = parentStateBehavior.Rb;
            rigidBody.gravityScale = 1f;
        }	
    }
}
