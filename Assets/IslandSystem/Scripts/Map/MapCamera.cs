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
					this.camera = IslandSystemManager.Instance.mapCamera;
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
		Transform previousMapItem;
		Vector2 previousCameraPosition;
		float previousCameraOrtho;
		
		// Use this for initialization
		void Start () {
		}

		/// <summary>
		/// Zooms the camera.
		/// Camera orthographics will resized to focus on selected target.
		/// </summary>
		/// <param name="target">Selected Target</param>
		public void ZoomIn (Transform target)
		{
			previousCameraPosition = new Vector2 (mapCamera.transform.position.x, mapCamera.transform.position.y);
			previousCameraOrtho = mapCamera.orthographicSize;

			// Keeping previous target for zooming out
			previousMapItem = target;

			// Zoom in camera
			float orthoSize = Instance.mapCamera.orthographicSize;
			orthoSize -= 2.5f;
			// Sending negative value for Zooming In. Positive value for Zooming Out.
			orthoSize *= -1f;
			CameraContoller.Zoom (orthoSize, 0.5f, EaseType.EaseOut);

			// Move camera to target position
			Vector2 cameraPosition = new Vector2 (previousMapItem.position.x, previousMapItem.position.y);
			CameraContoller.MoveCameraInstantlyToPosition (cameraPosition);

			ToggleCameraControl (false);
		}

		public void ZoomOut ()
		{
			ToggleCameraControl (true);

			// Move camera to target position
//			Vector2 cameraPosition = new Vector2 (previousMapItem.position.x, previousMapItem.position.y);
//			Vector2 cameraPosition = new Vector2 (0, 0);
			CameraContoller.MoveCameraInstantlyToPosition (previousCameraPosition);
//			CameraContoller.MoveCameraInstantlyToPosition (CameraContoller.CameraTargetPosition);

			// Zoom out camera to previous position.
			float orthoSize = Instance.mapCamera.orthographicSize;
			orthoSize = previousCameraOrtho - orthoSize;
			CameraContoller.Zoom (orthoSize, 0.5f, EaseType.EaseOut);
		}

		/// <summary>
		/// Toogles the camera movement.
		/// Can stop the camera Zooming and Movement when Popup UI is enabled
		/// </summary>
		/// <param name="enable">If set to <c>true</c> enable camera zooming and panning.</param>
		public void ToggleCameraControl (bool enable)
		{
			CameraContoller.enabled = enable;
		}
	}
}
