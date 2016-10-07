using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Poptropica2.Characters {
	public class Outfit : MonoBehaviour {
		[Tooltip("The player may not wear unavailable Adornments, but NPCs may.")]
		public bool isPlayerOutfit;

		public Color skinColor, hairColor;
		public bool reactiveEyelids;

		[Header("Mandatory")]
		public EyelidSpriteAnimator eyes;
		public MouthAnimator mouth;
		public Hair hair;
		public Shirt shirt;
		public Pants pants;
		public Shoes shoes;
		[Header("Optional")]
		public WristFront wristFront;
		public Bangs bangs;
		public Helmet helmet;
		public Facial facial;
		public Makeup makeup;
		public Marks marks;
		public Overshirt overshirt;
		public Overpants overpants;
		public BackhandItem backhandItem;
		public Hat hat;
		public Pack pack;

		List<Adornment> allAdornments;
		/// <summary>Generates a new list every time it is fetched to assure the references are fresh.
		/// Be careful to only do this when it won't weigh things down (i.e. not in a loop).</summary>
		public List<Adornment> AllAdornments {
			get {
				allAdornments = new List<Adornment>(15); //number of possible customizations - set initial capacity, as it probably shouldn't need resizing.
				allAdornments.Add(hair);
				allAdornments.Add(shirt);
				allAdornments.Add(pants);
				allAdornments.Add(shoes);
				allAdornments.Add(wristFront);
				allAdornments.Add(bangs);
				allAdornments.Add(helmet);
				allAdornments.Add(facial);
				allAdornments.Add(makeup);
				allAdornments.Add(marks);
				allAdornments.Add(overshirt);
				allAdornments.Add(overpants);
				allAdornments.Add(backhandItem);
				allAdornments.Add(hat);
				allAdornments.Add(pack);
				return allAdornments;
			}
		}
	}
}