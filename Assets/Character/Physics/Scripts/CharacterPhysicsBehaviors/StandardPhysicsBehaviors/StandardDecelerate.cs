using UnityEngine;
using System.Collections;

namespace Poptropica2.Characters
{
    /// <summary>
    /// Standard decelerate.
    /// Slows down the character's rigidbody eventually
    /// </summary>
    public class StandardDecelerate:CharacterPhysicsBehavior
    {
        bool affectX, affectY;
        AnimationCurve slowDownCurve = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 0f));
        float startTime, progress;
        Vector2 startVelocity;

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

        public AnimationCurve SlowDownCurve
        {
            get
            {
                return slowDownCurve;
            }
            set
            {
                slowDownCurve = value;
            }
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex = 0)
        {
            Rigidbody2D rigidBody = parentStateBehavior.Rb;
            startTime = Time.time;
            startVelocity = rigidBody.velocity;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex = 0)
        {
            float curTime = Time.time - startTime;
            float progress = curTime / stateInfo.length;
            progress = progress < 1f ? progress : 1f;
            Rigidbody2D rigidBody = parentStateBehavior.Rb;

            float newVelocityX = affectX ? SlowDown(startVelocity.x, progress) : rigidBody.velocity.x;
            float newVelocityY = affectY ? SlowDown(startVelocity.y, progress) : rigidBody.velocity.y;

            Vector2 newVelocity = new Vector2(newVelocityX, newVelocityY);
				

            if (ValidVelocity(newVelocity))
            {
                CharacterModel model = parentStateBehavior.Model;
                if (!model.canJumpToTarget)
                {
                    rigidBody.velocity = newVelocity;
                }

            }


        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex = 0)
        {
            
        }

        /// <summary>
        /// Slows down the character by modifying it's rigidbody's velocity on a particular axis
        /// </summary>
        /// <returns>The modified velocity on a particular axis</returns>
        /// <param name="startValue">initial velocity value on a particular axis</param>
        /// <param name="progressToEnd">Progress to end.</param>
        float SlowDown(float startValue, float progressToEnd)
        {
            return slowDownCurve.Evaluate(progressToEnd) * startValue;
        }
    }
}
