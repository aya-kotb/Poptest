using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Poptropica2.IslandSystem
{
	/// <summary>
	/// Clear area panel. A panel which show coin value for a selected Unlocked area.
	/// Can use a coin to unlock/clear area
	/// </summary>
	public class ClearAreaPanel : MonoBehaviour {

		public Button yesButton;
		public Button noButton;
		public Text coinValueText;
		public RectTransform childPanel;
		public RectTransform semiTransparentScreen;

		int lockID;

		// Use this for initialization
		void Start () {
			AssignButtonListener ();
			ShowTransaction ();
		}

		/// <summary>
		/// Initialize the variables of clear area panel.
		/// </summary>
		/// <param name="coinValue">Coin value.</param>
		/// <param name="lockerId">Locker/Cloud ID.</param>
		public void InitializeClearArea (int coinValue, int lockId)
		{
			coinValueText.text = coinValue.ToString ();
			lockID = lockId;
		}

		#region Button Click Events

		void AssignButtonListener ()
		{
			yesButton.onClick.AddListener (OnClickYesButton);
			noButton.onClick.AddListener (OnClickNoButton);
		}
		
		/// <summary>
		/// When Yes button is pressed for Clear Area this function is called.
		/// Unlock the area from map and reduce the coin.
		/// </summary>
		public void OnClickYesButton ()
		{
			Debug.Log ("Clicked Yes Button");
			HideTransaction (true);
		}

		/// <summary>
		/// When No button is pressed for Clear Area this function is called.
		/// Diable Clear Area panel.
		/// </summary>
		public void OnClickNoButton ()
		{
			Debug.Log ("Clicked No Button");
			HideTransaction (false);
		}

		#endregion

		/// <summary>
		/// Show the Clear Area Panel with transaction effect.
		/// </summary>
		void ShowTransaction ()
		{
			LeanTween.alpha (semiTransparentScreen, 0.5f, 0.5f);
			LeanTween.scale (childPanel, Vector3.one, 0.5f);
		}

		/// <summary>
		/// Hide the Clear Area Panel with transaction effect.
		/// </summary>
		void HideTransaction (bool canClear)
		{
			LeanTween.alpha (semiTransparentScreen, 0.0f, 0.5f);
			LeanTween.scale (childPanel, Vector3.zero, 0.5f).setOnComplete(OnCompleteTransaction).setOnCompleteParam(canClear);
		}

		void OnCompleteTransaction (object canClear)
		{
			if ((bool)canClear)
			{
				Cloud cloud = IslandSystemManager.Instance.mapHandler.currentItem as Cloud;
				// if not null the selected map item is cloud
				if (cloud != null)
				{
					//Unlock the item which is under the cloud area.
					cloud.lockedItem.canSelect = true;
				}

				// Remove cloud area from map.
				Destroy (IslandSystemManager.Instance.mapHandler.currentItem.gameObject);
			}

			Destroy (gameObject);
		}
	}
}
