using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Poptropica2.Characters
{
    /// <summary>
    /// The states which inherit this class can have one or more CharacterBehavior instances attached to them at runtime
    /// </summary>
    public abstract class CharacterStateBehavior : StateMachineBehaviour
    {
        CharacterModel model;
        Rigidbody2D rb;
        Collider2D primaryCollider, bodyCollider;
        Transform viewTransform;
        Vector2 hitPosition;

        protected List<CharacterPhysicsBehavior> characterPhysicsBehaviors;

        /// <summary>
        /// The CharacterModel component attached to the character
        /// </summary>
        public CharacterModel Model
        {
            get
            {
                return model;
            }
            set
            {
                model = value;
            }
        }

        public Rigidbody2D Rb
        {
            get
            {
                return rb;
            }
            set
            {
                rb = value;
            }
        }

        /// <summary>
        /// The collider on the feet of the character
        /// </summary>
        public Collider2D PrimaryCollider
        {
            get
            {
                return primaryCollider;
            }
            set
            {
                primaryCollider = value;
            }
        }

        /// <summary>
        /// The collider on the body of the character
        /// </summary>
        public Collider2D BodyCollider
        {
            get
            {
                return bodyCollider;
            }
            set
            {
                bodyCollider = value;
            }
        }

        /// <summary>
        /// Transform of the GameObject to which the character's SkeletonAnimator is attached. Allows it to be rotated, etc. for visual effect.
        /// </summary>
        public Transform ViewTransform
        {
            get
            {
                return viewTransform;
            }
            set
            {
                viewTransform = value;
            }
        }

        /// <summary>
        /// Position of "hit" objects from the scene - such as rope colliders and others that affect character motion.
        /// </summary>
        public virtual Vector2 HitPosition
        {
            get
            {
                return hitPosition;
            }
            set
            {
                hitPosition = value;
            }
        }

        public virtual void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            InitializeState();
            PreDetermineState();
            CharacterBehaviorsOnStateEnter(animator, stateInfo, layerIndex);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        public virtual void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            CharacterBehaviorsOnStateUpdate(animator, stateInfo, layerIndex);
        }
            
        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        public virtual void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            CharacterBehaviorsOnStateExit(animator, stateInfo, layerIndex);
        }

        /// <summary>
        /// On the first frame of execution (and under other circumstances), calculations involving Rigidbody2D velocity directly (without using forces) can be invalid.
        /// If a state behavior might be executed right at start up and adjusts velocity, that possible velocity should be tested first.
        /// </summary>
        /// <param name="velocity">The candidate for Rigidbody2D velocity.</param>
        /// <returns>true if velocity is valid</returns>
        protected bool ValidVelocity(Vector2 velocity)
        {
            return !(float.IsNaN(velocity.x) || float.IsNaN(velocity.y));
        }

        /// <summary>
        /// Determines the necessary behaviours to create based on the Character Model Type
        /// </summary>
        protected virtual void PreDetermineState()
        {

        }

        /// <summary>
        /// Initializes the state.
        /// </summary>
        protected void InitializeState()
        {
            characterPhysicsBehaviors = new List<CharacterPhysicsBehavior>();
        }

        /// <summary>
        /// Creates a CharacterBehaviour instance at runtime
        /// Initializes the behaviour instance
        /// Stores them inside a list
        /// </summary>
        /// <returns>A CharacterBehaviour instance</returns>
        /// <typeparam name="T">The T can be any type of CharacterBehaviour</typeparam>
        protected CharacterPhysicsBehavior CreateBehavior<T>() where T:new()
        {
            T behaviour = new T();
            CharacterPhysicsBehavior characterPhysicsBehavior = behaviour as CharacterPhysicsBehavior;
            InitializeCharacterBehavior(characterPhysicsBehavior);
            characterPhysicsBehaviors.Add(characterPhysicsBehavior);
            return characterPhysicsBehavior;
        }

        /// <summary>
        /// Initializes a CharacterBehaviour instance at runtime
        /// </summary>
        /// <param name="behaviour">Behaviour.</param>
        protected void InitializeCharacterBehavior(CharacterPhysicsBehavior behaviour)
        {
            behaviour.ParentStateBehavior = this;
        }

        /// <summary>
        /// Calls the OnStateEnter method of all the CharacterBehaviour instances 
        /// It's made to fit in with the StateMachineBehaviour method - OnStateEnter
        /// </summary>
        /// <param name="animator">Animator.</param>
        /// <param name="stateInfo">State info.</param>
        /// <param name="layerIndex">Layer index.</param>
        protected void CharacterBehaviorsOnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            for (int i = 0; i < characterPhysicsBehaviors.Count; i++)
            {
                characterPhysicsBehaviors[i].OnStateEnter(animator, stateInfo, layerIndex);
            }
        }

        /// <summary>
        /// Calls the OnStateUpdate method of all the CharacterBehaviour instances 
        /// It's made to fit in with the StateMachineBehaviour method - OnStateUpdate
        /// </summary>
        /// <param name="animator">Animator.</param>
        /// <param name="stateInfo">State info.</param>
        /// <param name="layerIndex">Layer index.</param>
        protected void CharacterBehaviorsOnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            for (int i = 0; i < characterPhysicsBehaviors.Count; i++)
            {
                characterPhysicsBehaviors[i].OnStateUpdate(animator, stateInfo, layerIndex);
            }
        }

        /// <summary>
        /// Calls the OnStateExit method of all the CharacterBehaviour instances 
        /// It's made to fit in with the StateMachineBehaviour method - OnStateExit
        /// </summary>
        /// <param name="animator">Animator.</param>
        /// <param name="stateInfo">State info.</param>
        /// <param name="layerIndex">Layer index.</param>
        protected void CharacterBehaviorsOnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            for (int i = 0; i < characterPhysicsBehaviors.Count; i++)
            {
                characterPhysicsBehaviors[i].OnStateExit(animator, stateInfo, layerIndex);
            }
        }
    }
}
