using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Poptropica2.IslandSystem.Utility
{
	/// <summary>
	/// Ignore image alpha is for ignoring the image alpha value which is not visible or 0.
	/// Can use for button which have different shape for ignoring raycast.
	/// </summary>
	public class IgnoreImageAlpha : MonoBehaviour {
		
		Image buttonImage;
		
		void Awake () {

			if (buttonImage == null) {
				buttonImage = GetComponent<Image> ();
			}

			//The alpha threshold specifying the minimum alpha a pixel must have for the event to be passed through.
			buttonImage.eventAlphaThreshold = 0.5f;
		}
	}
}
