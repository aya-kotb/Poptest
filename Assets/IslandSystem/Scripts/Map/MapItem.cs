using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Poptropica2.IslandSystem
{
    /// <summary>
    /// Map item.
    /// This class is the base class of all item presenet inside the map.
    /// </summary>
	public class MapItem : MonoBehaviour {

		public int ID
		{
			get {return itemID;}
		}

		public bool canSelect = false;
		protected int itemID;
        protected IslandSystemManager islandSystemManager;
		public object itemInfo;

		// Use this for initialization
		void Start () {
			
		}

        /// <summary>
        /// Initializes the map item.
        /// This method is used for storing the information or details of item
        /// </summary>
        /// <param name="info">Pass the Information of item.</param>
		public virtual void InitializeMapItem (object info)
		{
			itemInfo = info;
		}

		/// <summary>
		/// Initialises the component attached within this gameobject.
		/// </summary>
		public virtual void InitializeComponent ()
		{
            islandSystemManager = SAMApplication.mainInstance.GetService<IslandSystemManager>();

			GetComponent<Button>().onClick.AddListener (OnClickItem);
			GetComponent<Button> ().transition = Selectable.Transition.None;

			if (GetComponent<Utility.IgnoreImageAlpha>() == null)
			{
				gameObject.AddComponent<Utility.IgnoreImageAlpha> ();
			}
		}

		/// <summary>
		/// Raises the click item event.
        /// This method is called when Map item is selected.
		/// </summary>
        public virtual void OnClickItem ()
		{
            
		}

		/// <summary>
		/// Get item detail.
		/// The following method returns the item detail 
		/// </summary>
		/// <returns>The item detail.</returns>
		/// <typeparam name="T">Can be any class like IslandDetail.</typeparam>
		public T GetItemDetail<T> () where T : class
		{
			return itemInfo as T;
		}
	}
}
