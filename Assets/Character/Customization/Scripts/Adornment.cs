using UnityEngine;
using System.Collections;

namespace Poptropica2.Characters {
	public abstract class Adornment : MonoBehaviour {
		public AdornPiece[] pieces;
		public abstract AdornmentType Type { get; }
		public bool isAvailable	= true;
		public bool IsAvailable { get { return IsAvailable; } }
	}

	/// <summary>Higher values in this enum equate to higher override priority.</summary>
	public enum AdornmentType {
		UNSET,
		Pack,
        Hair,
		Shirt,
		Pants,
		Shoes,
		WristFront,
		Bangs,
		Facial,
		Makeup,
		Marks,
		BackhandItem,
		Hat,
		Helmet,
		Overshirt,
		Overpants
		//Pupils,
		//EyelidsLashes,
		//Mouth
	}
}