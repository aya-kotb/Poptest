using UnityEngine;
using System.Collections.Generic;
using Com.LuisPedroFonseca.ProCamera2D;
using UnityEngine.UI;


	public class GUIEffectsController : MonoBehaviour
    {
		public List<Transform> mainTargets;
		private bool isAi;

		public InputField RinputField;
		public InputField GinputField;
		public InputField BinputField;

		public InputField panRateTextField;

		public InputField camShakeIntensityTextField;
		public InputField CamShakeDurationTextField;

		public InputField lerpTextField;
		public InputField deadZoneTextField;

		public InputField ZoomRateTextField;

		public InputField playerPriorityTextField;
		public InputField Ai1PriorityTextField;
		public InputField Ai2PriorityTextField;

		public InputField viewPortTextField;

		private ProCamera2DCameraWindow proCamera2DCameraWindow;
		private ProCamera2DPanAndZoom proCamera2DPanAndZoom;
		private Camera cam;
		public ProCamera2DTriggerZoom proCamera2DTriggerZoom;
		private ProCamera2DGeometryBoundaries proCamera2DGeometryBoundaries;

		public enum CameraStationaryStatus
		{
			CameraMovement,
			CameraStationary
		}
		CameraStationaryStatus cameraStationaryStatus;
		public Text cameraStatusText;
		private string camStationary = "CamStationary";
		private string camMovement = "CamFollowX&Y";

		void Start(){

			RinputField.text = "0";
			GinputField.text = "0";
			BinputField.text = "0";

			camShakeIntensityTextField.text = "5";
			CamShakeDurationTextField.text = "0.5";

			lerpTextField.text = "0.15";
			deadZoneTextField.text = "0.05";

			playerPriorityTextField.text = "1";
			Ai1PriorityTextField.text = "2";
			Ai2PriorityTextField.text = "2";

			viewPortTextField.text = "5.32";
			panRateTextField.text = "1";

			cam = ProCamera2DShake.Instance.GetComponent<Camera>().GetComponent<Camera> () as Camera;
			proCamera2DCameraWindow = cam.GetComponent<ProCamera2DCameraWindow> () as ProCamera2DCameraWindow;
			proCamera2DPanAndZoom = cam.GetComponent<ProCamera2DPanAndZoom> () as ProCamera2DPanAndZoom;
			proCamera2DGeometryBoundaries = cam.GetComponent<ProCamera2DGeometryBoundaries> () as ProCamera2DGeometryBoundaries;


			for (int i = 0; i < ProCamera2D.Instance.CameraTargets.Count; i++) {
				if (i == 0) {
					ProCamera2D.Instance.CameraTargets [i].TargetPriority = 1;
				} else {
					ProCamera2D.Instance.CameraTargets [i].TargetPriority = 2;
				}
			}
		}
	

		/// <summary>
        /// Makes the camera stationary or move.
        /// </summary>
		public void MakeCameraStationaryorMovement()
        {
			if (cameraStationaryStatus == CameraStationaryStatus.CameraMovement) 
            {
				cameraStatusText.text = camMovement;
				ProCamera2D.Instance.FollowHorizontal = false;
				ProCamera2D.Instance.FollowVertical = false;
				proCamera2DPanAndZoom.AllowPan = false;
				proCamera2DPanAndZoom.AllowZoom = false;
				cameraStationaryStatus = CameraStationaryStatus.CameraStationary;
			} 
            else 
            {
				cameraStatusText.text = camStationary;
				ProCamera2D.Instance.FollowHorizontal = true;
				ProCamera2D.Instance.FollowVertical = true;
				proCamera2DPanAndZoom.AllowPan = true;
				proCamera2DPanAndZoom.AllowZoom = true;
				cameraStationaryStatus = CameraStationaryStatus.CameraMovement;
			}

		}
            
		/// <summary>
        /// Adds the target to make camera follow.
        /// </summary>
        /// <param name="trans">Trans.</param>
		public void AddTargetToMakeCameraFollow(Transform trans)
        {
			ProCamera2D.Instance.AddCameraTarget (trans);
		}

        /// <summary>
        /// Follows the target in X direction.
        /// 
		public void FollowTargetInXDirection()
        {
			ProCamera2D.Instance.FollowHorizontal = true;
			ProCamera2D.Instance.FollowVertical = false;
		}


        /// <summary>
        /// Follows the target in Y direction.
        /// </summary>
		
		public void FollowTargetInYDirection()
        {
			mainTargets[0].transform.position = new Vector3 (mainTargets[0].transform.position.x, 15, mainTargets[0].transform.position.z);
			ProCamera2D.Instance.FollowVertical = true;
			ProCamera2D.Instance.FollowHorizontal = false;
		}


        /// <summary>
        /// Updates the zoom value.
        /// </summary>
		public void UpdateZoomValue()
        {
			proCamera2DTriggerZoom.TargetZoom = float.Parse(ZoomRateTextField.text);
		}

        /// <summary>
        /// Updates the camera shake.
        /// </summary>
		public void UpdateCameraShake()
        {
			ProCamera2DShake.Instance.Strength = new Vector2 (float.Parse(camShakeIntensityTextField.text), 2);
			ProCamera2DShake.Instance.Duration = float.Parse(CamShakeDurationTextField.text);
			ProCamera2DShake.Instance.Shake();
		}

        /// <summary>
        /// Updates the pan speed.
        /// </summary>
		public void UpdatePanSpeed(){
            proCamera2DPanAndZoom.DragPanSpeedMultiplier = new Vector3 (float.Parse(panRateTextField.text), 0);
		}

        /// <summary>
        /// Updates the camera movement. Lerping.
        /// </summary>
		public void UpdateCameraMovement()
        {
			ProCamera2D.Instance.VerticalFollowSmoothness = float.Parse(lerpTextField.text);
			ProCamera2D.Instance.HorizontalFollowSmoothness = float.Parse(lerpTextField.text);

			proCamera2DCameraWindow.CameraWindowRect = new Rect (0, 0, float.Parse(deadZoneTextField.text), 0.1f);
		}

        /// <summary>
        /// Camera Fade in.
        /// </summary>
		public void FadeIn()
        {
			ProCamera2DTransitionsFX.Instance.BackgroundColorExit = new Color (float.Parse(RinputField.text)/255f, float.Parse(GinputField.text)/255f, float.Parse(BinputField.text)/255f);
			ProCamera2DTransitionsFX.Instance.CreateMaterials ();
			ProCamera2DTransitionsFX.Instance.TransitionExit();
		}

        /// <summary>
        /// Camera Fade Out.
        /// </summary>
		public void FadeOut()
        {
			ProCamera2DTransitionsFX.Instance.BackgroundColorEnter = new Color (float.Parse(RinputField.text)/255f, float.Parse(GinputField.text)/255f, float.Parse(BinputField.text)/255f);
			ProCamera2DTransitionsFX.Instance.CreateMaterials ();
			ProCamera2DTransitionsFX.Instance.TransitionEnter();
		}

        /// <summary>
        /// Updates the Camera view port.
        /// </summary>
		public void UpdateViewPort()
        {
			cam.orthographicSize = float.Parse (viewPortTextField.text);
		}

        /// <summary>
        /// Updates the player priority.
        /// </summary>
		public void UpdatePlayerPriority()
        {
			//ProCamera2D.Instance.isChangePriority = false;
			ProCamera2D.Instance.CameraTargets [0].TargetPriority = int.Parse(playerPriorityTextField.text);
		}

        /// <summary>
        /// Updates A i1 priority.
        /// </summary>
		public void UpdateAI1Priority()
        {
			ProCamera2D.Instance.CameraTargets [1].TargetPriority = int.Parse(Ai1PriorityTextField.text);
		}

        /// <summary>
        /// Updates A i2 priority.
        /// </summary>
		public void UpdateAI2Priority()
        {
			ProCamera2D.Instance.CameraTargets [2].TargetPriority = int.Parse(Ai2PriorityTextField.text);
		}
            
    }
