using UnityEngine;
using System.Collections;

namespace Poptropica2.Characters
{
    /// <summary>
    /// This Behavior calls upon StandardGravityScaling during runtime
    /// There's one StandardGravityScalingBehavior and one StandardGravityScaling, so that StandardGravityScaling can also be used during runtime in some other state, if needed
    /// This way only the overridable states will need their own separate state to be created. Others can just have several physical behaviors attached to them
    /// For example - In this project StateJump is an overridable state whose physical behavior can be altered during runtime if needed.
    /// </summary>
    public class StandardGravityScalingBehavior : CharacterStateBehavior
    {
        public float gravityScale;

        protected override void PreDetermineState()
        {
            StandardModelProperties();
        }

        /// <summary>
        /// Properties belonging to the standard character model only
        /// </summary>
        void StandardModelProperties()
        {
            StandardGravityScaling standardGravityScaling = CreateBehavior<StandardGravityScaling>() as StandardGravityScaling;
            standardGravityScaling.GravityScale = gravityScale;
        }
    }
}
