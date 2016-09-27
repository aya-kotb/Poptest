using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Poptropica2.IslandSystem
{
	/// <summary>
	/// Clear area panel. A panel which show coin value for a selected Unlocked area.
	/// Can use a coin to unlock/clear area.
	/// </summary>
    public class ClearAreaPanel : PanelTransition {

		public Text coinValueText;

        IslandSystemManager islandSystemManager;

		// Use this for initialization
		void Start () {

            islandSystemManager = SAMApplication.mainInstance.GetService<IslandSystemManager>();

            Cloud cloud = islandSystemManager.mapHandler.currentItem as Cloud;
            InitializeClearArea(cloud.itemValue);
			
            //UI Animation for displaying panel
            AlphaTween(semiTransparentScreen, alpha);
            ScaleOne(childPanel);
		}

		/// <summary>
		/// Initialize the variables of clear area panel.
		/// </summary>
		/// <param name="coinValue">Coin value.</param>
		/// <param name="lockerId">Locker/Cloud ID.</param>
		public void InitializeClearArea (int coinValue)
		{
			coinValueText.text = coinValue.ToString ();
		}

		/// <summary>
		/// When Yes button is pressed for Clear Area this function is called.
		/// Unlock the area from map and reduce the coin.
		/// </summary>
		public void OnClickYesButton ()
		{
            //UI Animation for hiding panel
            AlphaTween(semiTransparentScreen, 0);
            ScaleZero(childPanel, this.OnCompleteTransitionYesButton);
		}

		/// <summary>
		/// When No button is pressed for Clear Area this function is called.
		/// Diable Clear Area panel.
		/// </summary>
		public void OnClickNoButton ()
		{
            //UI Animation for hiding panel
            AlphaTween(semiTransparentScreen, 0);
            ScaleZero(childPanel, this.OnCompleteTransitionNoButton);
		}

        /// <summary>
        /// Callback after completing UI transition effect when Yes button pressed.
        /// </summary>
        void OnCompleteTransitionYesButton ()
        {
            Cloud cloud = islandSystemManager.mapHandler.currentItem as Cloud;
            // if not null the selected map item is cloud
            if (cloud != null)
            {
                //Unlock the item which is under the cloud area.
                cloud.lockedItem.canSelect = true;
            }
            
            // Remove cloud area from map.
            Destroy (islandSystemManager.mapHandler.currentItem.gameObject);
            Destroy (gameObject);
        }

        /// <summary>
        /// Callback after completing UI transition effect when No button is pressed.
        /// </summary>
        void OnCompleteTransitionNoButton ()
        {
            Destroy (gameObject);
        }
	}
}
