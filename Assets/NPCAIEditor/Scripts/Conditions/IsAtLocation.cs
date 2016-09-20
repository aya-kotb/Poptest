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
    /// This particular Conditional checks whether a particular character is inside a particular rectangular region within the scene
    /// </summary>
    [TaskCategory("PopTropica/NPCAI")]
    public class IsAtLocation : Conditional
    {
        public SharedBoxCastInfo boxCastInfo;

        public SharedTransform targetCharacter;

        public override TaskStatus OnUpdate()
        {
            if (targetCharacter.Value == null)
            {
                return TaskStatus.Failure;
            }

            RaycastHit2D hit = Physics2D.BoxCast(boxCastInfo.Value.origin, boxCastInfo.Value.size, boxCastInfo.Value.angle, 
                                   boxCastInfo.Value.direction, boxCastInfo.Value.distance, boxCastInfo.Value.layerMask, boxCastInfo.Value.minDepth, boxCastInfo.Value.maxDepth);
        
            if (hit.transform == targetCharacter.Value)
            {
                return TaskStatus.Success;
            }
            
            // a target is not within sight so return failure
            return TaskStatus.Running;
        }

        // Draw the line of sight representation within the scene window
        public override void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.matrix = Matrix4x4.TRS(boxCastInfo.Value.origin, Owner.transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector2.zero, boxCastInfo.Value.size);
            Gizmos.matrix = Matrix4x4.TRS(boxCastInfo.Value.origin + (boxCastInfo.Value.direction.normalized * boxCastInfo.Value.distance), Owner.transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector2.zero, boxCastInfo.Value.size);
            Gizmos.color = Color.cyan;
            Gizmos.matrix = Matrix4x4.TRS(boxCastInfo.Value.origin, Quaternion.identity, Vector3.one);
            Gizmos.DrawLine(Vector2.zero, boxCastInfo.Value.direction.normalized * boxCastInfo.Value.distance);
        }
    }
}