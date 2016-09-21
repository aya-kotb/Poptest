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

		public override void OnClickItem ()
		{
			if (canSelect == false)
			{
				return;
			}

			UnluckArea ();
		}

		public override void InitializeMapItem (object id)
		{
			base.InitializeMapItem (id);
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
