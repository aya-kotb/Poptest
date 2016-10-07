using UnityEngine;
using System.Collections;

namespace Poptropica2.Characters
{
    /// <summary>
    /// Standard freeze axis.
    /// Stops the rigidbody on the axes which can be affected
    /// </summary>
    public class StandardFreezeAxis:CharacterPhysicsBehavior
    {
        bool affectX = false;
        bool affectY = true;

        public bool AffectX
        {
            get
            {
                return affectX;
            }
            set
            {
                affectX = value;
            }
        }

        public bool AffectY
        {
            get
            {
                return affectY;
            }
            set
            {
                affectY = value;
            }
        }
            
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex = 0)
        {

        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex = 0)
        {
            Rigidbody2D rigidBody = parentStateBehavior.Rb;
            float stoppedVelocityX = affectX ? 0f : rigidBody.velocity.x;
            float stoppedVelocityY = affectY ? 0f : rigidBody.velocity.y;
            Vector2 stoppedVelocity = new Vector2(stoppedVelocityX, stoppedVelocityY);

            if (ValidVelocity(stoppedVelocity))
            {
                rigidBody.gravityScale = 0f;
                rigidBody.velocity = stoppedVelocity;
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex = 0)
        {

        }	
    }
}
