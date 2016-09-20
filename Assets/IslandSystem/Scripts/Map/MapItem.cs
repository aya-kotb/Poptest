using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Poptropica2.IslandSystem
{
	public class MapItem : MonoBehaviour {

		public int ID
		{
			get {return itemID;}
		}

		public bool canSelect = false;
		protected int itemID;
		public object itemDetail;

		// Use this for initialization
		void Start () {
			
		}

		public virtual void InitializeMapItem (object detail)
		{
			itemDetail = detail;
		}

		/// <summary>
		/// Initialises the component attached within this gameobject.
		/// </summary>
		public virtual void InitializeComponent ()
		{
			GetComponent<Button>().onClick.AddListener (OnClickItem);
			GetComponent<Button> ().transition = Selectable.Transition.None;

			if (GetComponent<Utility.IgnoreImageAlpha>() == null)
			{
				gameObject.AddComponent<Utility.IgnoreImageAlpha> ();
			}
		}
		
		/// <summary>
		/// Raises the click item event.
		/// </summary>
		public virtual void OnClickItem ()
		{
		}


	}
}
