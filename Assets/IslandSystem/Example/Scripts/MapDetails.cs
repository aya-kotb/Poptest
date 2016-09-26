using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Poptropica2.IslandSystem
{
	/// <summary>
	/// Map Details is a dummy class for getting inforamtion of items.
	/// This class contains details of all items (Island, Treasure Chest, Cloud etc) present inside map.
	/// </summary>
	public static class MapDetails
	{
		public static List<MapHandler.IslandDetail> GetAvailableIslands ()
		{
			// Fetching island details
			List<MapHandler.IslandDetail> islandDetailList = new List<MapHandler.IslandDetail> ();
			for (int i = 0; i < 4; i++)
			{
				MapHandler.IslandDetail islandDetail = new MapHandler.IslandDetail ();
                islandDetail.id = (i + 1).ToString();
				islandDetail.name = GetIslandName (islandDetail.id);
				islandDetail.difficulty = (3 - i);
				islandDetail.trophy = Random.Range (0, 10);
				islandDetail.progress = Random.Range (5, 90);

				islandDetailList.Add (islandDetail);
			}


			return islandDetailList;
		}

        static string GetIslandName (string id)
		{
			switch (id)
			{
				case "1":
					return "Home Island";
				case "2":
					return "24 Carrot";
				case "3":
					return "Crisis Caverns";
				case "4":
					return "Lost Pirates";
				default:
					return "";
			}
		}
	}
}
