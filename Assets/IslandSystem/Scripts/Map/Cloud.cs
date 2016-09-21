using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Poptropica2.IslandSystem
{
	/// <summary>
	/// Cloud class will unlock the area.
	/// </summary>
	public class Cloud : MapItem {

        public int itemValue;
		public MapItem lockedItem;

        public GameObject displayCoin;
        public Text coinText;

		// Use this for initialization
		void Start () {
			InitializeComponent ();
		}
		
        /// <summary>
        /// This method used for Initialising the variables and properties.
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
        /// This method is called when cloud is selected.
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
        /// <param name="info">Pass the Information of item.</param>
		public override void InitializeMapItem (object info)
		{
			base.InitializeMapItem (info);
		}

		/// <summary>
		/// Unluck the selected area.
		/// </summary>
		void UnluckArea ()
		{
			// Only some of the cloud/area can be cleared.
            islandSystemManager.mapHandler.currentItem = this;
            islandSystemManager.islandSystemUI.SwitchUIPanel (IslandSystemUIHandler.PanelState.ClearArea);
		}
	}
}
