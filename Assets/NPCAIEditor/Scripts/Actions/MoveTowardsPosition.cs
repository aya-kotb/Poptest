using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using Poptropica2.Characters;

#if !(UNITY_4_3 || UNITY_4_4)
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
#endif

namespace Poptropica2.NPCAI
{
    /// <summary>
    /// This class inherits from Action. It can be directly used inside the BehaviorDesigner plugin's editor
    /// This particular Action can be used to take the NPC to a specific point or a target
    /// </summary>
    [TaskCategory("PopTropica/NPCAI")]
    public class MoveTowardsPosition : Action
    {
        [Tooltip("The speed of the nav agent")]
        public SharedFloat moveSpeed;
        [Tooltip("The position that the agent is seeking")]
        public SharedVector3 targetPosition;
        [Tooltip("The target that the agent is seeking")]
        public SharedTransform target;

        Vector3 destination;

        NavMeshAgent agent;

        /// <summary>
        /// Raises the awake event.
        /// </summary>
        public override void OnAwake()
        {
            // cache for quick lookup
            agent = Owner.gameObject.GetComponent<NavMeshAgent>();

            // set the speed and angular speed
            agent.speed = moveSpeed.Value;
        }

        /// <summary>
        /// Raises the update event.
        /// </summary>
        public override TaskStatus OnUpdate()
        {
            CalculateNewPosition();
            SetMovePosition();

            return TaskStatus.Success;
        }
            
        /// <summary>
        /// Calculates the new position.
        /// </summary>
        void CalculateNewPosition()
        {
            if (target == null)
            {
                destination = targetPosition.Value;
                return;
            }

            destination = target.Value.position;
            destination.y = destination.y + 2;
        }

        /// <summary>
        /// Sets the move position.
        /// </summary>
        void SetMovePosition()
        {
            ((PathfindingCharacterController)agent.GetComponent<StandardCharacterModel>().controllerContainer.Result).SetMovePosition(destination);
        }
    }
}
