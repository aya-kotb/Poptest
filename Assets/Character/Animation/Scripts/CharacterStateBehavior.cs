using UnityEngine;
using System.Collections;

namespace Poptropica2.Characters {
	public abstract class CharacterStateBehavior : StateMachineBehaviour {
		CharacterModel model;
		Rigidbody2D rb;
		Collider2D primaryCollider, bodyCollider;
		Transform viewTransform;
		Vector2 hitPosition;

		//Getters are for access to the model and physical / view info by derived behaviors. Setters are for initialization by the CharacterModel.
		public CharacterModel Model { get {	return model;} 	set { model = value; } }
		public Rigidbody2D Rb { get { return rb; } set { rb = value; } }
		public Collider2D PrimaryCollider { get { return primaryCollider; } set { primaryCollider = value; } }
		public Collider2D BodyCollider { get { return bodyCollider; } set { bodyCollider = value; } }
		/// <summary>Transform of the GameObject to which the character's SkeletonAnimator is attached. Allows it to be rotated, etc. for visual effect.</summary>
		public Transform ViewTransform { get { return viewTransform; } set { viewTransform = value; } }
		/// <summary>Position of "hit" objects from the scene - such as rope colliders and others that affect character motion.</summary>
		public virtual Vector2 HitPosition { get { return hitPosition; } set { hitPosition = value; } }

		/// <summary>
		/// On the first frame of execution (and under other circumstances), calculations involving Rigidbody2D velocity directly (without using forces) can be invalid.
		/// If a state behavior might be executed right at start up and adjusts velocity, that possible velocity should be tested first.
		/// </summary>
		/// <param name="velocity">The candidate for Rigidbody2D velocity.</param>
		/// <returns></returns>
		protected bool ValidVelocity(Vector2 velocity) {
			return !(float.IsNaN(velocity.x) || float.IsNaN(velocity.y));
		}
	}
}