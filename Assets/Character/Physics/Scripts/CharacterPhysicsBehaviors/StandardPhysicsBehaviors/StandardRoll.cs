using UnityEngine;
using System.Collections;

namespace Poptropica2.Characters
{
    /// <summary>
    /// Standard roll.
    /// </summary>
    public class StandardRoll:CharacterPhysicsBehavior
    {
        Vector2 pivotOffset;
        float rollSpeed = 720f;
        float unrollTime = 0.25f;
        float startTime;
        float rollDirection;

        public Vector2 PivotOffset
        {
            get
            {
                return pivotOffset;
            }
            set
            {
                pivotOffset = value;
            }
        }

        public float RollSpeed
        {
            get
            {
                return rollSpeed;
            }
            set
            {
                rollSpeed = value;
            }
        }

        public float UnrollTime
        {
            get
            {
                return unrollTime;
            }
            set
            {
                unrollTime = value;
            }
        }

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex = 0)
        {
            InitializeRoll();
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex = 0)
        {
            RollViewTransform();
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex = 0)
        {
            ResetView();
        }

        /// <summary>
        /// Initializes roll.
        /// </summary>
        void InitializeRoll()
        {
            CharacterModel model = parentStateBehavior.Model;

            startTime = Time.time;
            rollDirection = model.FacingRight ? -1f : 1f;
        }

        /// <summary>
        /// Rolls the view transform only
        /// </summary>
        void RollViewTransform()
        {
            CharacterModel model = parentStateBehavior.Model;
            Transform viewTransform = parentStateBehavior.ViewTransform;

            Vector3 pivotPoint = model.transform.position + new Vector3(pivotOffset.x * viewTransform.localScale.x, pivotOffset.y * viewTransform.localScale.y, 0);
            viewTransform.RotateAround(pivotPoint, Vector3.forward, rollSpeed * rollDirection * Time.fixedDeltaTime);
        }

        /// <summary>
        /// Resets the viewTransform rotation
        /// </summary>
        void ResetView()
        {
            CharacterModel model = parentStateBehavior.Model;
            Transform viewTransform = parentStateBehavior.ViewTransform;

            model.ResetView(viewTransform, unrollTime);
        }
    }
}
