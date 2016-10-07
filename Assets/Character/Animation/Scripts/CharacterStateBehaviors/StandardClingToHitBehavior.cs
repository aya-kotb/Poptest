using UnityEngine;
using System.Collections;

namespace Poptropica2.Characters
{
    /// <summary>
    /// This Behavior calls upon StandardClingToHit during runtime
    /// There's one StandardClingToHitBehavior and one StandardClingToHit, so that StandardClingToHit can also be used during runtime in some other state, if needed
    /// This way only the overridable states will need their own separate state to be created. Others can just have several physical behaviors attached to them
    /// For example - In this project StateJump is an overridable state whose physical behaviour can be altered during runtime if needed.
    /// </summary>
    public class StandardClingToHitBehavior : CharacterStateBehavior
    {
        public float clingThreshhold = 0.1f;
        public float clingSpeed = 3f;
        public float lateralForce = 2000f;

        protected override void PreDetermineState()
        {
            StandardModelProperties();
        }

        /// <summary>
        /// Properties belonging to the standard character model only
        /// </summary>
        void StandardModelProperties()
        {
            StandardClingToHit standardClingToHit = CreateBehavior<StandardClingToHit>() as StandardClingToHit;
            standardClingToHit.ClingThreshhold = clingThreshhold;
            standardClingToHit.ClingSpeed = clingSpeed;
            standardClingToHit.LateralForce = lateralForce;
        }
    }
}
