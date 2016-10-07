using UnityEngine;
using System.Collections;

namespace Poptropica2.Characters
{
    /// <summary>
    /// Standard jump.
    /// </summary>
    public class StandardJump:CharacterPhysicsBehavior
    {
        float jumpStartTime;
        float currentJumpTime;
        float jumpMultiplier = 1.7f;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex = 0)
        {
            InitializeJump();
            TargetedJump();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex = 0)
        {
           
            BoostJump();

        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex = 0)
        {
            parentStateBehavior.Model.jumpTarget = Vector2.zero;
            parentStateBehavior.Model.canJumpToTarget = false;
        }

        /// <summary>
        /// Initializes the jump.
        /// </summary>
        void InitializeJump()
        {
            if (parentStateBehavior.Model.canJumpToTarget)
            {
                return;
            }

            Rigidbody2D rigidBody = parentStateBehavior.Rb;
            CharacterModel model = parentStateBehavior.Model;

            jumpStartTime = Time.time;

            rigidBody.AddForce(Vector2.up * model.jumpSpeed * 100);
        }

        /// <summary>
        /// The targeted jump functionality. The jump occurs based on the touch/mouse input
        /// </summary>
        void TargetedJump()
        {
            if (!parentStateBehavior.Model.canJumpToTarget)
            {
                return;
            }

            Rigidbody2D rigidBody = parentStateBehavior.Rb;
            CharacterModel model = parentStateBehavior.Model;

            if (model.jumpTarget.magnitude > 0)
            {
                Vector2 velocity = TargetVelocity(model);
                float sign = Mathf.Sign(velocity.x);
                float xVelocity = Mathf.Abs(velocity.x) > parentStateBehavior.Model.maxTargetedJumpVelocity.x ? parentStateBehavior.Model.maxTargetedJumpVelocity.x : Mathf.Abs(velocity.x);
                xVelocity = sign * xVelocity;
                float yVelocity = Mathf.Abs(velocity.y) > parentStateBehavior.Model.maxTargetedJumpVelocity.y ? parentStateBehavior.Model.maxTargetedJumpVelocity.y : Mathf.Abs(velocity.y);
                velocity = new Vector2(xVelocity, yVelocity);

                rigidBody.velocity = velocity;
            }
        }

        /// <summary>
        /// returns the velocity required for the model's rigidbody to reach a particular point in the world
        /// </summary>
        /// <returns>The velocity.</returns>
        /// <param name="model">Model.</param>
        Vector2 TargetVelocity(CharacterModel model)
        {
            Vector2 direction = model.jumpTarget - (Vector2)model.transform.position;
            float angleInRadians = Mathf.Atan2(direction.y, direction.x);
            float angle =  angleInRadians * Mathf.Rad2Deg;
            if (angle > 90)
            {
                angle = 180 - angle;
            }
            float height = direction.y;  // get height difference
            direction.y = 0;  // retain only the horizontal direction
            float distance = direction.magnitude;  // get horizontal distance
            angleInRadians = angle * Mathf.Deg2Rad;  // convert angle to radians
            direction.y = distance * Mathf.Tan(angleInRadians);  // set dir to the elevation angle
            distance += height / Mathf.Tan(angleInRadians);  // correct for small height differences
            float velocity = Mathf.Sqrt(distance * Physics2D.gravity.magnitude / Mathf.Sin(2 * angleInRadians));
            return velocity * direction.normalized * jumpMultiplier;
        }
            
        /// <summary>
        /// Boosts the jump.
        /// </summary>
        void BoostJump()
        {
            if (parentStateBehavior.Model.canJumpToTarget)
            {
                return;
            }

            Rigidbody2D rigidBody = parentStateBehavior.Rb;
            CharacterModel model = parentStateBehavior.Model;

            Vector2 boostForce = Vector2.up * model.jumpBoost * Time.fixedDeltaTime;

            currentJumpTime = Time.time - jumpStartTime;
            if (currentJumpTime < model.jumpDuration)
            {
                rigidBody.AddForce(boostForce);
            }
        }
    }
}
