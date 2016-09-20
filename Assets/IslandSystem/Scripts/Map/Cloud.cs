using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Poptropica2.IslandSystem
{
	/// <summary>
	/// Cloud class will unlock the area.
	/// </summary>
	public class Cloud : MapItem {

		public int cloudID;
		public MapItem lockedItem;

		// Use this for initialization
		void Start () {
			InitializeComponent ();
			itemID = cloudID;
		}
		
		public override void InitializeComponent ()
		{
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
			IslandSystemManager.Instance.mapHandler.currentItem = this;
			IslandSystemUIHandler.Instance.SwitchUIPanel (IslandSystemUIHandler.PanelState.ClearArea);
		}


	}
}
