using UnityEngine;
using System.Collections;

namespace Poptropica2.Characters
{
    /// <summary>
    /// This Behavior calls upon StandardRoll during runtime
    /// There's one StandardRollBehavior and one StandardRoll, so that StandardRoll can also be used during runtime in some other state, if needed
    /// This way only the overridable states will need their own separate state to be created. Others can just have several physical behaviors attached to them
    /// For example - In this project StateJump is an overridable state whose physical behavior can be altered during runtime if needed.
    /// </summary>
    public class StandardRollBehavior : CharacterStateBehavior
    {
        public Vector2 pivotOffset;
        public float rollSpeed = 720f;
        public float unrollTime = 0.25f;

        protected override void PreDetermineState()
        {
            StandardModelProperties();
        }

        /// <summary>
        /// Properties belonging to the standard character model only
        /// </summary>
        void StandardModelProperties()
        {
            StandardRoll standardRoll = CreateBehavior<StandardRoll>() as StandardRoll;
            standardRoll.PivotOffset = pivotOffset;
            standardRoll.RollSpeed = rollSpeed;
            standardRoll.UnrollTime = unrollTime;
        }
    }
}
