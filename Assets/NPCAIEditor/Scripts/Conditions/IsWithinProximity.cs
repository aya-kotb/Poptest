using UnityEngine;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

#if !(UNITY_4_3 || UNITY_4_4)
using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
#endif

namespace Poptropica2.NPCAI
{
    /// <summary>
    /// This class inherits from Conditional. It can be directly used inside the BehaviorDesigner plugin's editor
    /// This particular Conditional checks whether a particular character is within the proximity of this NPC.
    /// </summary>
    [TaskCategory("PopTropica/NPCAI")]
    public class IsWithinProximity : Conditional
    {
        public SharedFloat minDistance;
        [Tooltip("Returns success if this object becomes within range")]
        public SharedTransform targetCharacter;

        public override TaskStatus OnUpdate()
        {
            if (targetCharacter.Value == null)
            {
                return TaskStatus.Failure;
            }

            if (Vector3.Distance(targetCharacter.Value.position, Owner.transform.position) < minDistance.Value)
            {
                return TaskStatus.Success;
            }
            // a target is not within sight so return failure
            return TaskStatus.Running;
        }

        // Draw the line of sight representation within the scene window
        public override void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(Owner.transform.position, minDistance.Value);
        }
    }
}