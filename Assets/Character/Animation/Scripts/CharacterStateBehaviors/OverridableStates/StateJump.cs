using UnityEngine;
using System.Collections;

namespace Poptropica2.Characters
{
	/// <summary>
	/// This StateBehaviour Class can be used to override jump state at runtime if necessary
	/// </summary>
	public class StateJump : CharacterStateBehavior
	{
		protected override void PreDetermineState ()
		{
			switch (Model.characterModelType) {
			case CharacterModel.CharacterModelTypes.Standard:
				StandardModelProperties ();
				break;
			case CharacterModel.CharacterModelTypes.Alternate:
				AlternateModelProperties ();
				break;
			default:
				break;
			}
		}

		/// <summary>
		/// Properties belonging to the standard character model only
		/// </summary>
		void StandardModelProperties ()
		{
			StandardCharacterModel standardModel = Model as StandardCharacterModel;
			StandardJump standardJump = CreateBehavior<StandardJump> () as StandardJump;

			StandardMovement movement = CreateBehavior<StandardMovement> () as StandardMovement;
			movement.CanMoveOnXAxis = true;
			movement.CanMoveOnYAxis = false;
			movement.SpeedLimitX = standardModel.jumpSpeedLimitX;
		}

		/// <summary>
		/// Alternate functionality of this state. Only with a jump and no movement. This is just an example
		/// </summary>
		void AlternateModelProperties ()
		{
			StandardCharacterModel standardModel = Model as StandardCharacterModel;

			AlternateJump alternateJump =	CreateBehavior<AlternateJump> () as AlternateJump;
		}
	}
}
