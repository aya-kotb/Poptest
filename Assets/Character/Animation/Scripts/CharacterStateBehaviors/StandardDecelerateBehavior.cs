using UnityEngine;
using System.Collections;

namespace Poptropica2.Characters
{
    /// <summary>
    /// This Behavior calls upon StandardDecelerate during runtime
    /// There's one StandardDecelerateBehavior and one StandardDecelerate, so that StandardDecelerate can also be used during runtime in some other state, if needed
    /// This way only the overridable states will need their own separate state to be created. Others can just have several physical behaviors attached to them
    /// For example - In this project StateJump is an overridable state whose physical behavior can be altered during runtime if needed.
    /// </summary>
    public class StandardDecelerateBehavior : CharacterStateBehavior
    {
        public bool affectX, affectY;
        public AnimationCurve slowDownCurve = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 0f));

        protected override void PreDetermineState()
        {
            StandardModelProperties();
        }

        /// <summary>
        /// Properties belonging to the standard character model only
        /// </summary>
        void StandardModelProperties()
        {
            StandardDecelerate standardDecelerate = CreateBehavior<StandardDecelerate>() as StandardDecelerate;
            standardDecelerate.AffectX = affectX;
            standardDecelerate.AffectY = affectY;
            standardDecelerate.SlowDownCurve = slowDownCurve;
        }
    }
}
