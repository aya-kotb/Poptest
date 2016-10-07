using UnityEngine;
using System.Collections;

namespace Poptropica2.Characters
{
    /// <summary>
    /// This Behavior calls upon StandardFreezeAxis during runtime
    /// There's one StandardFreezeAxisBehavior and one StandardFreezeAxis, so that StandardFreezeAxis can also be used during runtime in some other state, if needed
    /// This way only the overridable states will need their own separate state to be created. Others can just have several physical behaviors attached to them
    /// For example - In this project StateJump is an overridable state whose physical behavior can be altered during runtime if needed.
    /// </summary>
    public class StandardFreezeAxisBehavior : CharacterStateBehavior
    {
        public bool affectX = false;
        public bool affectY = true;

        protected override void PreDetermineState()
        {
            StandardModelProperties();
        }

        /// <summary>
        /// Properties belonging to the standard character model only
        /// </summary>
        void StandardModelProperties()
        {
            StandardFreezeAxis standardFreezeAxis = CreateBehavior<StandardFreezeAxis>() as StandardFreezeAxis;
            standardFreezeAxis.AffectX = affectX;
            standardFreezeAxis.AffectY = affectY;
        }
    }
}
