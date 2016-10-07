using UnityEngine;
using System.Collections;

namespace Poptropica2.Characters
{
    /// <summary>
    /// Standard movement.
    /// </summary>
    public class StandardMovement:CharacterPhysicsBehavior
    {
        float speedLimitX;
        float speedLimitY;

        bool canMoveOnXAxis;
        bool canMoveOnYAxis;

        public float SpeedLimitX
        {
            get
            {
                return speedLimitX;
            }
            set
            {
                speedLimitX = value;
            }
        }

        public float SpeedLimitY
        {
            get
            {
                return speedLimitY;
            }
            set
            {
                speedLimitY = value;
            }
        }

        public bool CanMoveOnXAxis
        {
            get
            {
                return canMoveOnXAxis;
            }
            set
            {
                canMoveOnXAxis = value;
            }
        }

        public bool CanMoveOnYAxis
        {
            get
            {
                return canMoveOnYAxis;
            }
            set
            {
                canMoveOnYAxis = value;
            }
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex = 0)
        {
            
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex = 0)
        {

            if (parentStateBehavior.Model.canJumpToTarget)
            {
                return;
            }

            HorizontalMovement(animator);
            VerticalMovement(animator);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex = 0)
        {

        }

        /// <summary>
        /// Movement on the X Axis
        /// </summary>
        /// <param name="animator">Animator.</param>
        void HorizontalMovement(Animator animator)
        {
            if (!canMoveOnXAxis)
            {
                return;
            }

            Rigidbody2D rigidBody = parentStateBehavior.Rb;
            CharacterModel model = parentStateBehavior.Model;

            float sign = model.FacingRight ? 1f : -1f;

            if (Mathf.Abs(rigidBody.velocity.x) < speedLimitX || Mathf.Sign(rigidBody.velocity.x) != sign)
            {
                Vector2 moveForce = Vector2.right * sign * model.runAccel * animator.GetFloat(StandardAnimatorHashes.speedPctHoriz) * Time.fixedDeltaTime * 1000;
                rigidBody.AddForce(moveForce);
                if (Mathf.Abs(rigidBody.velocity.x) > speedLimitX)  												//Correct sudden overacceleration
                {   
                    rigidBody.velocity = new Vector2(sign * speedLimitX, rigidBody.velocity.y);
                }
            }

        }

        /// <summary>
        /// Movement on the Y Axis
        /// </summary>
        /// <param name="animator">Animator.</param>
        void VerticalMovement(Animator animator)
        {
            if (!canMoveOnYAxis)
            {
                return;
            }

            Rigidbody2D rigidBody = parentStateBehavior.Rb;
            CharacterModel model = parentStateBehavior.Model;

            if ((speedLimitY > 0 && rigidBody.velocity.y < speedLimitY) || (speedLimitY < 0 && rigidBody.velocity.y > speedLimitY))
            {
                Vector2 moveForce = Vector2.up * model.runAccel * animator.GetFloat(StandardAnimatorHashes.speedPctVert) * Time.fixedDeltaTime * 1000;
                rigidBody.AddForce(moveForce);
            }
        }
    }
}
