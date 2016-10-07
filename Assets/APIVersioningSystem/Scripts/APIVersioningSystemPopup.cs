using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.EventSystems;

namespace Poptropica2.APIVersioningSystem
{
    /// <summary>
	/// This class handles the popup dialog to be displayed when an update is available.
    /// </summary>
    public class APIVersioningSystemPopup : MonoBehaviour
    {
        public Text headerText;
        public Text messageText;
        public Text updateButtonText;

        public Button updateButton;
        public Button closeButton;

        public RectTransform overlayScreen;
        public RectTransform popUpScreen;

        public float tweenTime = 0.8f;
        public float alphaFade = 0.8f;

        private System.Action callbackAction;

        /// <summary>
        /// This method sets the canvas as parent to this gameobject.
        /// </summary>
        public void HandleInstantiatedPrefab ()
        {
            this.transform.SetParent(GetCanvasTransform(), false);
            Color color = overlayScreen.GetComponent<Image>().color;
            color.a = 0;
            overlayScreen.GetComponent<Image>().color = color;
            popUpScreen.localScale = Vector3.zero;
            this.gameObject.SetActive(true);
        }

        /// <summary>
        /// Triggers the button event from close button and update button.
		/// It hides the alert popup
        /// </summary>
        public void OnClickClose ()
        {
            HidePopup();
        }

        /// <summary>
        /// This method sets the information to be displayed in the popup
		/// It will be called when there is an update occurs on server
        /// </summary>
        /// <param name="message"> Message to display in popup .</param>
        /// <param name="header"> Header to display at top of popup .</param>
        /// <param name="setCloseButton"> If set to <c>true</c> Displays close button in popup screen.</param>
        /// <param name="setUpdateButton"> If set to <c>true</c>Displays update button in popup screen.</param>
        /// <param name="onCallback">callback after completing the transition.</param>
		public void DisplayPopup(string message, string header = "", bool setCloseButton = true, bool setUpdateButton = false, System.Action onCallback = null)
       	{
            messageText.text = message;
            headerText.text = header;
            
#if UNITY_WEBGL
            updateButtonText.text = "Restart";
#elif UNITY_IPHONE || UNITY_IOS || UNITY_ANDROID
            updateButtonText.text = "Update";
#endif

			if (onCallback != null)
            {
                callbackAction = onCallback;
            }

            closeButton.gameObject.SetActive(setCloseButton);
            updateButton.gameObject.SetActive(setUpdateButton);

            ShowPopup();
        }

        /// <summary>
        /// Giving a fadein effects to the popup screen before displaying
        /// </summary>
        public void ShowPopup ()
        {
            LeanTween.alpha(overlayScreen, alphaFade, tweenTime);
            LeanTween.scale(popUpScreen, Vector3.one, tweenTime);
        }

        /// <summary>
        /// Giving a fadeout effects to the popup screen before hiding
        /// </summary>
        public void HidePopup ()
        {
            LeanTween.alpha(overlayScreen, 0, tweenTime);
            LeanTween.scale(popUpScreen, Vector3.zero, tweenTime).setOnComplete(OnCompleteHideTween);
        }

        /// <summary>
        /// Callback after completing UI transition effect.
        /// Trigger a callback and destroys the gameobject.
        /// </summary>
        void OnCompleteHideTween ()
        {
            if (callbackAction != null)
            {
                callbackAction();
            }
            Destroy(this.gameObject);
        }

        /// <summary>
        /// This method will search for Canvas
		/// if the canvas is not present in hierarchy then it will a create new canvas
        /// </summary>
        /// <returns>Transform which contain Canvas object.</returns>
        Transform GetCanvasTransform()
        {
            Canvas canvas = null;
            // Attempt to find a canvas 
            canvas = FindObjectOfType<Canvas>();
			if (canvas != null) {

				return canvas.transform;                
			}

            // Since the canvas not presents in hierarchy it'll create a new canvas on it's own.
			GameObject canvasGameObject = new GameObject("Canvas");
            canvasGameObject.layer = LayerMask.NameToLayer("UI");
            canvas = canvasGameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGameObject.AddComponent<CanvasScaler>();
            canvasGameObject.AddComponent<GraphicRaycaster>();

            EventSystem eventSystem = UnityEngine.Object.FindObjectOfType<EventSystem>();
            if (eventSystem == null)
            {
                GameObject eventSystemGameObject = new GameObject("EventSystem");
                eventSystem = eventSystemGameObject.AddComponent<EventSystem>();
                eventSystemGameObject.AddComponent<StandaloneInputModule>();

#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
                eventSystemGameObject.AddComponent<TouchInputModule>();
#endif

#if UNITY_EDITOR
                UnityEditor.Undo.RegisterCreatedObjectUndo(eventSystemGameObject, "Create EventSystem");
#endif
            }

            return canvas.transform;
        }
    }
}
