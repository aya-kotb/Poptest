using UnityEngine;
using System.Collections;

namespace Poptropica2.Characters
{
    /// <summary>
    /// Alternate jump. 
    /// This is a test class for checking the overriding functionality of a particular state inside the AnimatorController
    /// Overrides the Standard Jump
    /// </summary>
    public class AlternateJump:CharacterPhysicsBehavior
    {
        float jumpStartTime;
        float currentJumpTime;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex = 0)
        {
            InitializeJump();
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex = 0)
        {
            BoostJump();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex = 0)
        {

        }

        /// <summary>
        /// Initializes the jump.
        /// </summary>
        void InitializeJump()
        {
            Rigidbody2D rigidBody = parentStateBehavior.Rb;
            CharacterModel model = parentStateBehavior.Model;

            jumpStartTime = Time.time;
            rigidBody.AddForce(Vector2.up * model.jumpSpeed * 300);
        }
            
        /// <summary>
        /// Boosts the jump.
        /// </summary>
        void BoostJump()
        {
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
