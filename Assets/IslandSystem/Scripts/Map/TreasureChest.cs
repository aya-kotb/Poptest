using UnityEngine;
using System.Collections;

namespace Poptropica2.IslandSystem
{
	/// <summary>
	/// Treasure chest holds the imformation of map specials item which
	/// contains Treasures, Ads or Special Quests.
	/// This Chest could be free, require coins or certain XP level to unlock
	/// </summary>
	public class TreasureChest : MapItem {

		public int chestID;

		// Use this for initialization
		void Start ()
        {
			InitializeComponent ();
		}
		
        /// <summary>
        /// This method used for Initialising the variables and properties.
        /// Initialises the component attached within this gameobject.
        /// </summary>
		public override void InitializeComponent ()
		{
			base.InitializeComponent ();
		}

        /// <summary>
        /// This method is triggered when chest is selected.
        /// </summary>
		public override void OnClickItem ()
		{
			if (canSelect == false)
			{
				return;
			}

			OpenChest ();
		}

        /// <summary>
        /// Initializes the map item.
        /// This method is used for storing the information or details of item
        /// </summary>
        /// <param name="info">object Pass the Information of item if any details has to be stored in Map item.</param>
		public override void InitializeMapItem (object id)
		{
			base.InitializeMapItem (id);
		}

		/// <summary>
        /// This method will call UI handler to Popup the message for Displaying the inforamtion of treasures.
		/// </summary>
		void OpenChest ()
		{
            islandSystemManager.mapHandler.currentItem = this;
            islandSystemManager.islandSystemUI.SwitchUIPanel (IslandSystemUIHandler.PanelState.MapSpecial);
		}
	}
}
