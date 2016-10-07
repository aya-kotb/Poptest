using UnityEngine;
using System.Collections;
using BehaviorDesigner.Runtime;

namespace Poptropica2.NPCAI
{
    /// <summary>
    /// Shared box cast info.
	/// This class inherits from SharedVariable<T>. It can be directly used inside the BehaviorDesigner plugin's editor
	/// This particular SharedVariable<T> is of type BoxCastInfo.
    /// </summary>
    [System.Serializable]
    public class SharedBoxCastInfo : SharedVariable<BoxCastInfo>
    {
        public static implicit operator SharedBoxCastInfo(BoxCastInfo value)
        {
            return new SharedBoxCastInfo { mValue = value };
        }
    }

    /// <summary>
    /// Contains Info for Physics2D.Boxcast
    /// </summary>
    [System.Serializable]
    public class BoxCastInfo
    {
        public Vector2 origin;
        public Vector2 size;
        public float angle;
        public Vector2 direction;
        public float distance = Mathf.Infinity;
        public LayerMask layerMask = Physics2D.DefaultRaycastLayers;
        public float minDepth = -Mathf.Infinity;
        public float maxDepth = Mathf.Infinity;
    }
}
