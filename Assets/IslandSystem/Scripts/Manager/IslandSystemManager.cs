using UnityEngine;
using System.Collections;

namespace Poptropica2.IslandSystem
{
	/// <summary>
	/// Island System Manager. This class will keeps all information of all others
	/// class, prefabs and objects of island system.
	/// </summary>
	public class IslandSystemManager : MonoBehaviour
	{
		public static IslandSystemManager Instance
		{
			get
			{
				return instance;
			}
		}

		static IslandSystemManager instance;

		public Camera mapCamera;
		public IslandSystemUIHandler islandSystemUI;
		public MapHandler mapHandler;

		void Awake ()
		{
			if (instance == null)
			{
				instance = this;
			}
			else
			{
				DestroyImmediate (this);
			}
		}
		
		// Use this for initialization
		void Start () {
			
		}
	}
}
