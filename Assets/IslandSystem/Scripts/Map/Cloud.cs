using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Poptropica2.IslandSystem
{
	/// <summary>
    /// Cloud class used to hold the details of the hidden map. 
    /// It handles unlocking of island too.
	/// </summary>
	public class Cloud : MapItem {

        public int itemValue;
		public MapItem lockedItem;

        public GameObject displayCoin;
        public Text coinText;

		void Start ()
        {
			InitializeComponent ();
		}
		
        /// <summary>
        /// This method is used for Initialising the variables and properties related to cloud.
        /// Initialises the component attached within this gameobject.
        /// Can initalise the cloud value cost for clearing.
        /// </summary>
		public override void InitializeComponent ()
		{
            if (canSelect)
            {
                displayCoin.SetActive(true);
                coinText.text = itemValue.ToString();
            }
            else
            {
                if (displayCoin != null)
                {
                    displayCoin.SetActive(false);
                }
            }

			base.InitializeComponent ();
		}

        /// <summary>
        /// This method is triggered when cloud is selected.
        /// </summary>
        public override void OnClickItem ()
		{
			if (canSelect == false)
			{
				return;
			}

			UnluckArea ();
		}

        /// <summary>
        /// Initializes the map item.
        /// This method is used for storing the information or details of item
        /// </summary>
        /// <param name="info">object Pass the Information of item if any details has to be stored in Map item.</param>
		public override void InitializeMapItem (object info)
		{
			base.InitializeMapItem (info);
		}

		/// <summary>
		/// This method will call UI handler to Popup the message for Unlocing/Clearing the area.
		/// </summary>
		void UnluckArea ()
		{
            islandSystemManager.mapHandler.currentItem = this;
            islandSystemManager.islandSystemUI.SwitchUIPanel (IslandSystemUIHandler.PanelState.ClearArea);
		}
	}
}
