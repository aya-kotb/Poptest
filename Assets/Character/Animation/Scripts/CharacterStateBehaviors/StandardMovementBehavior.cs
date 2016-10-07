using UnityEngine;
using System.Collections;

namespace Poptropica2.Characters
{
    /// <summary>
    /// This Behavior calls upon StandardMovement during runtime
    /// There's one StandardMovementBehavior and one StandardMovement, so that StandardMovement can also be used during runtime in some other state, if needed
    /// This way only the overridable states will need their own separate state to be created. Others can just have several physical behaviors attached to them
    /// For example - In this project StateJump is an overridable state whose physical behavior can be altered during runtime if needed.
    /// </summary>
    public class StandardMovementBehavior : CharacterStateBehavior
    {
        public float speedLimitX;
        public float speedLimitY;

        public bool canMoveOnXAxis;
        public bool canMoveOnYAxis;

        protected override void PreDetermineState()
        {
            StandardModelProperties();
        }

        /// <summary>
        /// Properties belonging to the standard character model only
        /// </summary>
        void StandardModelProperties()
        {
            StandardMovement movement = CreateBehavior<StandardMovement>() as StandardMovement;
            movement.CanMoveOnXAxis = canMoveOnXAxis;
            movement.CanMoveOnYAxis = canMoveOnYAxis;
            movement.SpeedLimitX = speedLimitX;
            movement.SpeedLimitY = speedLimitY;
        }
    }
}
