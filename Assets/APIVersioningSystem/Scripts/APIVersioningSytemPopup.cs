using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.EventSystems;

namespace Poptropica2.APIVersioningSystem
{
    /// <summary>
    /// This class open the popup and display the messages.
    /// </summary>
    public class APIVersioningSytemPopup : MonoBehaviour
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
        /// This method set the gameobject to canvas parent.
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
        /// Triggers the button event.
        /// Close the popup screen.
        /// </summary>
        public void OnClickClose ()
        {
            HidePopup();
        }

        /// <summary>
        /// This method will initialize the paremeter and display the popup message.
        /// Can hide close button so that user will not close it without completing the process.
        /// 
        /// </summary>
        /// <param name="message">string Message to display in popup screen.</param>
        /// <param name="header">string Header to display at top of popup screen.</param>
        /// <param name="setCloseButton">bool If set to <c>true</c>Dislpay close button in popup screen.</param>
        /// <param name="setUpdateButton">bool If set to <c>true</c>Display update button in popup screen.</param>
        /// <param name="onCallback">callback after completing the transition.</param>
        public void DisplayPopup(
            string message,
            string header = "",
            bool setCloseButton = true,
            bool setUpdateButton = false,
            System.Action onCallback = null)
        {
            messageText.text = message;
            headerText.text = header;
            #if UNITY_WEBGL
            updateButtonText.text = "Restart";
            #elif UNITY_IPHONE || UNITY_IOS || UNITY_ANDROID
            updateButtonText.text = "Update";
            #endif

            if (callbackAction != null)
            {
                callbackAction = onCallback;
            }

            closeButton.gameObject.SetActive(setCloseButton);
            updateButton.gameObject.SetActive(setUpdateButton);

            ShowPopup();
        }

        /// <summary>
        /// Giving a fade effects to the pop screen screen before Display in screen
        /// </summary>
        void ShowPopup ()
        {
            LeanTween.alpha(overlayScreen, alphaFade, tweenTime);
            LeanTween.scale(popUpScreen, Vector3.one, tweenTime);
        }

        /// <summary>
        /// Giving a fade effects to the pop screen screen before hiding
        /// </summary>
        public void HidePopup ()
        {
            LeanTween.alpha(overlayScreen, 0, tweenTime);
            LeanTween.scale(popUpScreen, Vector3.zero, tweenTime).setOnComplete(OnCompleteHideTween);
        }

        /// <summary>
        /// Callback after completing UI transition effect.
        /// Trigger a callback and destriy the gameobject.
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
        /// This method search for Canvas if the canvas is not present in hirerarcy it will create new Cnavas
        /// </summary>
        /// <returns>Transform which contain Canvas object.</returns>
        static Transform GetCanvasTransform()
        {
            Canvas canvas = null;
            // Attempt to find a canvas anywhere
            canvas = FindObjectOfType<Canvas>();
            if (canvas != null) return canvas.transform;                

            // if we reach this point, we haven't been able to locate a canvas
            // ...So we'd better create one

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
