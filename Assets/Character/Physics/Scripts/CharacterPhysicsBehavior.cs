using UnityEngine;
using System.Collections;

namespace Poptropica2.Characters
{
    /// <summary>
    /// Character's physical behaviour. This class is the parent to all the classes which contain functionalities related to physics
    /// </summary>
    public abstract class CharacterPhysicsBehavior
    {
        protected CharacterStateBehavior parentStateBehavior;

        public virtual void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex = 0)
        {
			
        }

        public virtual void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex = 0)
        {
			
        }

        public virtual void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex = 0)
        {
			
        }

        public CharacterStateBehavior ParentStateBehavior
        {
            get
            {
                return parentStateBehavior;
            }
            set
            {
                parentStateBehavior = value;
            }
        }
            
        /// <summary>
        /// On the first frame of execution (and under other circumstances), calculations involving Rigidbody2D velocity directly (without using forces) can be invalid.
        /// If a state behavior might be executed right at start up and adjusts velocity, that possible velocity should be tested first.
        /// </summary>
        /// <param name="velocity">The candidate for Rigidbody2D velocity.</param>
        /// <returns></returns>
        protected bool ValidVelocity(Vector2 velocity)
        {
            return !(float.IsNaN(velocity.x) || float.IsNaN(velocity.y));
        }
    }
}
