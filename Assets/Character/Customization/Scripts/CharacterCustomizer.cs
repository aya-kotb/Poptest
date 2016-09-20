using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;

/*
TODO: The eyes were originally developed as part of the Character Animation System, and were attached to the standard character prefab.
However, due to the customizable nature of the eyelids, pupils, eyelashes, and mouth, they must be moved to this system.
They are a little more complicated than the adornments, however, because they involve scripts to animate blinks and mouse/touch tracking.
The mouths are also both customizeable and animateable, and will need scripts to control this, but as of 12 July 2016 we still need input from designers on how this should work.
*/

namespace Poptropica2.Characters {
	public class CharacterCustomizer : MonoBehaviour {
		public StandardCharacterModel character;
		public Outfit outfit;

#if UNITY_EDITOR
		[Tooltip("Editor only: click to refresh")]
		public bool refreshNow;
#endif

		SkeletonRenderer skelRenderer;
		Transform boneFollowersParent;
		List<AdornPiece> allPieces;

		void Start() {
			skelRenderer = character.View.GetComponent<SkeletonRenderer>();
			boneFollowersParent = skelRenderer.gameObject.transform.Find("BoneFollowers");
			Refresh();
		}

		void Update() {

#if UNITY_EDITOR
			if (refreshNow) {
				Refresh();
				refreshNow = false;
			}
#endif

		}

		/// <summary>
		/// Causes all customizations to be updated on the character. The character should be at rest when this happens so the positional offsets are correct.
		/// </summary>
		public void Refresh() {
			InsertAdornments();
			EnactOverrides();
			ApplySkinColorToRig();
		}

		void ApplySkinColorToRig() {
			skelRenderer.skeleton.SetColor(outfit.skinColor);
		}

		void InsertAdornments() {
			//float timekeep = Time.realtimeSinceStartup;
			DestroyOldAdornments();
			allPieces = new List<AdornPiece>();
			List<Adornment> adornments = outfit.AllAdornments;
			for (int i = 0; i < adornments.Count; i++) {
				if (adornments[i] != null) {
					for (int j = 0; j < adornments[i].pieces.Length; j++) {
						//Instantiate new AdornPiece
						AdornPiece lastPiece = Instantiate(adornments[i].pieces[j]);
						lastPiece.Type = adornments[i].Type;
						allPieces.Add(lastPiece);

						//Put it in the right spot
						lastPiece.transform.localScale = character.transform.localScale;
						lastPiece.transform.localScale = character.transform.localScale;
						Vector3 scaledPosition = lastPiece.transform.position * lastPiece.transform.localScale.x;   //Assumes uniform scale.
																													//Revert the scaled position's z element. This may be used for sorting render order, so it should be preserved.
						scaledPosition = new Vector3(scaledPosition.x, scaledPosition.y, lastPiece.transform.position.z);
						lastPiece.transform.localPosition = character.transform.position + scaledPosition;

						//Place it into the hierarchy
						Transform pieceParent = boneFollowersParent.Find(lastPiece.boneName);
						if (pieceParent != null) {
							lastPiece.transform.SetParent(pieceParent);
						} else {
							Debug.LogWarning("Tried to insert customization piece \"" + lastPiece.name + "\" under BoneFollower \"" + lastPiece.boneName + "\", but the latter was null.");
						}

						//Colorize it, if necessary
						if (lastPiece.ColorizeWith == ColorizeOption.HairColor) {
							lastPiece.Colorize(outfit.hairColor);
						} else if (lastPiece.colorizeWith == ColorizeOption.SkinColor) {
							lastPiece.Colorize(outfit.skinColor);
						}
					}
				}
			}
			//Debug.Log("Time to adorn: " + (Time.realtimeSinceStartup - timekeep));
		}

		void DestroyOldAdornments() {
			if (allPieces != null) {
				for (int i = 0; i < allPieces.Count; i++) {
					if (allPieces[i] != null) {
						//Debug.Log(i + ": Destroy " + allPieces[i].name);
						Destroy(allPieces[i].gameObject);
					}
				}
			}
		}

		/// <summary>
		/// Check each bone to find the highest priority overriding piece following each.
		/// If any of the bones contain an overrider, delete all pieces other than the highest priority overrider.
		/// It's possible that there will be an adornment with multiple pieces attached to the same bone, with override checked for that bone's pieces.
		/// In this case, they will have equal override priority and should all remain, replacing all other lower-priority pieces.
		/// </summary>
		void EnactOverrides() {
			//float timekeep = Time.realtimeSinceStartup;
			BoneFollower[] followers = boneFollowersParent.GetComponentsInChildren<BoneFollower>();

			foreach (var follower in followers) {
				List<AdornPiece> highestPriorityOverriders = new List<AdornPiece>();
				AdornPiece[] pieces = follower.transform.GetComponentsInChildren<AdornPiece>();
				foreach (var piece in pieces) {
					if (piece.overrideOtherPieces) {
						if (highestPriorityOverriders.Count < 1 || piece.Type == highestPriorityOverriders[0].Type) {
							highestPriorityOverriders.Add(piece);
						} else if (piece.Type > highestPriorityOverriders[0].Type) {
							highestPriorityOverriders.Clear();
							highestPriorityOverriders.Add(piece);
						}
					}
					/**
					Debug.Log("Highest priority overriders in bone " + follower.name + ": ");
					highestPriorityOverriders.ForEach(x => Debug.Log("\t" + x.name));
					/**/
				}
				if (highestPriorityOverriders.Count > 0) {  //This bone follower has overriders
					for (int i = 0; i < pieces.Length; i++) {
						if (!highestPriorityOverriders.Contains(pieces[i])) {
							//Debug.Log("Deleting overriden piece: " + pieces[i].name);
							Destroy(pieces[i].gameObject);
						}
					}
				}
			}
			//Debug.Log("Time to override adornments: " + (Time.realtimeSinceStartup - timekeep));
		}
	}
}