using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Poptropica2.IslandSystem
{
	/// <summary>
	/// Map Handler.
	/// This class will handle all objects belongs to map
	/// and keep the information of all items in map.
	/// </summary>
	public class MapHandler : MonoBehaviour {

		public Island[] island;
		public Cloud[] cloud;
		public TreasureChest[] chest;

        [HideInInspector] public MapItem currentItem;
		public MapItem homeIsland;
        public Blimp blimp;

		// Use this for initialization
		void Start () {
			InitializeMapItems ();
		}

		[System.Serializable]
		public class IslandDetail
		{
			public int id;			// Island ID
			public int difficulty;	// Number od difficulty.
			public int trophy;		// Total trophy achieved.
			public string name;		// Name of the island.
			public float progress;	// Progress made in the island.
		}

        #region Getting DUMMY information

		/// <summary>
		/// Initialize map items.
		/// The following is used for initializing all the variables 
		/// and items which comes under map.
		/// </summary>
		public void InitializeMapItems ()
		{
			// gettting dummy island details...
			List<IslandDetail> islandDetailList = MapDetails.GetAvailableIslands ();
			for (int i = 0; i < islandDetailList.Count; i++)
			{
				if (GetIsland (islandDetailList[i].id) != null)
				{
					GetIsland (islandDetailList [i].id).InitializeMapItem (islandDetailList [i]);
				}
			}
		}

		/// <summary>
		/// Gets the island by island id.
		/// in map Island ID is predefined in gameobject
		/// </summary>
		/// <returns>The island in map.</returns>
		/// <param name="id">pass Island Identifier.</param>
		Island GetIsland (int id)
		{
			for (int i = 0; i < island.Length; i++)
			{
				if (island[i].islandID == id)
				{
					return island [i];
				}
			}

			return null;
		}

        #endregion Getting DUMMY information
	}
}
