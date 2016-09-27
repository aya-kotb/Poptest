using UnityEngine;
using System.Collections;

namespace Poptropica2.IslandSystem
{
	/// <summary>
	/// Island System Manager. This class will keeps all information of all others
	/// class, prefabs and objects of island system.
	/// </summary>
    public class IslandSystemManager : MonoBehaviour, IService
	{
		public IslandSystemUIHandler islandSystemUI;
		public MapHandler mapHandler;

        //For temporary using CharacterInfo class to fetch the character ID
        public CharacterInfo characterInfo;

		void Awake ()
		{
            SAMApplication.mainInstance.AddService("IslandSystemManager", this);
		}
		
		// Use this for initialization
		void Start ()
        {
			characterInfo = new CharacterInfo ();
		}

		/// <summary>
		/// Loads the current selected island.
		/// This method Load the island scene according to the selected island.
		/// </summary>
		void LoadIsland ()
		{
			string sceneName = SceneName(mapHandler.currentItem.GetItemDetail<MapHandler.IslandDetail>().name);
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
		}

        /// <summary>
        /// Visits the island.
        /// This method is called to GameSpark manager to register character
        /// for visited island
        /// </summary>
        public void VisitIsland ()
        {
            GameSparksManager.Instance().VisitIsland(
                characterInfo.characterID,
                mapHandler.currentItem.ID,
                HandleonIslandVisited,
                HandleonRequestFailed);
        }

        /// <summary>
        /// This method handles by receiveing the id of the island visited
        /// from GameSparks API.
        /// </summary>
        /// <param name="island_id">Island identifier.</param>
        void HandleonIslandVisited (string island_id)
        {
            Debug.Log("Handle on Island Visited: " + island_id);
            LoadIsland();
        }

        /// <summary>
        /// receives, GameSparksError, a class which contains a GameSparksErrorMessage, Enum
        /// invalid_username, invalid_password, request_failed, request_timeout
        /// </summary>
        /// <param name="error">Error GameSparksError.</param>
        void HandleonRequestFailed (GameSparksError error)
        {
            Debug.LogError("Handle on Request Failed: " + error.errorMessage);
        }

        /// <summary>
        /// ShowInspectorUI: This is the implemenation method for the IService interface
        /// </summary>
        public void ShowInspectorUI()
        {

        }

		string SceneName (string islandName)
		{
			switch (islandName)
			{
				case "Home Island":
					return "HomeIsland";
				case "24 Carrot":
					return "24Carrot";
				case "Crisis Caverns":
					return "CrisisCaverns";
				case "Lost Pirates":
					return "LostPirates";

				default:
					return "";
			}
		}
	}
}
