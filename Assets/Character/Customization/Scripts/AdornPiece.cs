using UnityEngine;
using System.Collections;

namespace Poptropica2.Characters {
	public class AdornPiece : MonoBehaviour {
		//TODO - instead of having the piece responsbile for storing the type at the time it is made an asset, have the Adornment assign its type when it's instantiated. 
		//This reduces redundancy and a possible source of inconsistency.
		AdornmentType type;
		[Tooltip("Must be exact, case-sensitive.")]
		public string boneName;
		public ColorizeOption colorizeWith;
		public bool overrideOtherPieces;

		public AdornmentType Type {
			get {
				return type;
			}
			set {
				type = value;
			}
		}

		SpriteRenderer spriteRenderer;

		public ColorizeOption ColorizeWith { get { return colorizeWith; } }

		void Init() {	//Start is not called before initialization is needed in this case, and other components are not guaranteed in Awake, so call this explicitly.
			if (spriteRenderer == null) {
				spriteRenderer = GetComponent<SpriteRenderer>();
			}
		}

		public void Colorize(Color color) {
			Init();
			spriteRenderer.color = color;
		}
	}

	public enum ColorizeOption {
		None,
		HairColor,
		SkinColor
	}
}