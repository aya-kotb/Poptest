using UnityEngine;
using System.Collections;
using Com.LuisPedroFonseca.ProCamera2D;

namespace Poptropica2.IslandSystem
{
	/// <summary>
	/// Map Camera.
	/// This class handles the orthographics camera of Island System Map.
	/// </summary>
	public class MapCamera : MonoBehaviour {

		public static MapCamera Instance
		{
			get
			{
				if (instance == null)
				{
					GameObject go = new GameObject ("MapCameraController");
					instance = go.AddComponent<MapCamera> ();
				}
				return instance;
			}
		}
		
		public Camera mapCamera
		{
			get
			{
				if (this.camera == null)
				{
                    this.camera = SAMApplication.mainInstance.GetService<IslandSystemManager>().mapCamera;
				}
				return this.camera;
			}
		}

		public ProCamera2D CameraContoller
		{
			get
			{
				return ProCamera2D.Instance;
			}
		}

		
		static MapCamera instance;
		Camera camera;

		// Use this for initialization
		void Start () {
			
		}

		/// <summary>
		/// Toogles the camera contol. This function is called to
		/// toggle the map camera control like zooming and panning.
		/// </summary>
		/// <param name="enable">If set to <c>true</c> enable camera zooming and panning.</param>
		/// <param name="enable">If set to <c>false</c> disable camera zooming and panning.</param>
		public void ToggleCameraControl (bool enable)
		{
			CameraContoller.enabled = enable;
		}
	}
}
