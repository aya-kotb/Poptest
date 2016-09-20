using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

// author: Rick Hocker

namespace Poptropica2.PopupSystem
{
    // enum for popup button IDs
    public enum PopupButtonID
    {
        CloseButton,
        OKButton,
        CancelButton
    }

	/// <summary>
	/// Popup manager.
	/// </summary>
	public class PopupManager : MonoBehaviour
    {
	
        public static PopupManager instance = null;                 // static instance of PopupManager which allows it to be accessed by any other scripts

        [Header("Non-UI Shared GameObjects")]
        public GameObject popupHolder;                              // PopupHolder prefab
        public GameObject closeButton;                              // Non-UI close button
        public GameObject blackFrame;                               // Black frame to hide edges of popup

        [Header("UI Shared GameObjects")]
        public GameObject popupHolderUI;                            // UIPopupHolder prefab
        public GameObject closeButtonUI;                            // UI close button

        [HideInInspector]public float screenHeight;                 // height of screen
        [HideInInspector]public float screenRatio;                  // aspect ratio of screen
        [HideInInspector]public float fitScale;                     // fit scale for 960x540 popup
        [HideInInspector]public AssetBundle popupBundle;            // asset bundle for testing in editor

        private List<GameObject> holders = new List<GameObject>();  // list of holders loaded
        private List<GameObject> popups = new List<GameObject>();   // list of popups loaded
        private bool hidingScene = false;                           // flag when scene is hidden by popup
        private UnityEvent closeEvent = null;                       // event to trigger when popup closed

        // dictionary of button listeners
        private Dictionary<string, UnityEvent> closeListeners = new Dictionary<string, UnityEvent>();
        private Dictionary<string, UnityEvent> okListeners = new Dictionary<string, UnityEvent>();
        private Dictionary<string, UnityEvent> cancelListeners = new Dictionary<string, UnityEvent>();

        void Awake()
        {
            // check if instance already exists
            if (instance == null)
            {
                // if not, set instance to this
                instance = this;
            }
            // if instance already exists and it's not this:
            else if (instance != this)
            {
                // then destroy this - this enforces our singleton pattern, meaning there can only ever be one instance of PopupManager
                Destroy(gameObject);
            }

            // sets this to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);

            // get fit scale for 960x540 popup
            fitScale = GetFitScale(960f, 540f);

            // load popups asset bundle for testing in editor
            #if UNITY_EDITOR

             // get platform folder
            string platformFolder = "";
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    platformFolder = "Windows";
                    break;
                case RuntimePlatform.OSXEditor:
                    platformFolder = "OSX";
                    break;
                case RuntimePlatform.WebGLPlayer:
                    platformFolder = "Web";
                    break;
            }

            // construct path to asset bundle
            string path = Application.dataPath;
            // back up one directory
            int index = path.LastIndexOf("/");
            path = path.Substring(0, index);
            // complete path with platform and popup bundle name
            path += ("/AssetBundles/" + platformFolder + "/popups");

            // get asset bundle from disk
            if (System.IO.File.Exists(path))
            {
                popupBundle = AssetBundle.LoadFromFile(path);
            }
            if (popupBundle == null)
            {
                Debug.Log("PopupManager: popup bundle not found");
            }

            #endif
         }

        /// <summary>
        /// Loads a non-UI popup. Dialog must have PopupNonUI script attached.
        /// </summary>
        /// <param name="obj">Asset bundle or popup.</param>
        /// <param name="assetName">Asset name of popup.</param>
        public void LoadPopup(Object obj, string assetName)
        {
            InitPopup(PopupBase.PopupType.Popup, obj, assetName);
        }

        /// <summary>
        /// Loads a canvas UI popup. Popup must have PopupUI script attached.
        /// </summary>
        /// <param name="obj">Asset bundle or popup.</param>
        /// <param name="assetName">Asset name of popup.</param>
        public void LoadUIPopup(Object obj, string assetName)
        {
            InitPopup(PopupBase.PopupType.UIPopup, obj, assetName);
        }

        /// <summary>
        /// Loads a dialog box popup. Dialog must have PopupDialogBox script attached.
        /// </summary>
        /// <param name="obj">Asset bundle or popup.</param>
        /// <param name="assetName">Asset name of popup.</param>
        /// <param name="title">Title.</param>
        /// <param name="message">Message.</param>
        public void LoadDialogBox(Object obj, string assetName, string title, string message)
        {
            PopupDialogBox popupHandler = (PopupDialogBox)InitPopup(PopupBase.PopupType.DialogBox, obj, assetName);
            // set text for title and message
            popupHandler.SetTextMessage (title, message);
        }

        /// <summary>
        /// Loads the popup from popup data for testing.
        /// </summary>
        /// <param name="data">Popup data object.</param>
        public void LoadPopupFromData(PopupData data)
        {
            // get prefab object
            Object obj = data.prefab;
            // get popup name
            string popupName = (obj == null) ? data.asset : obj.name;

            // if no prefab, then get testing popup bundle
            if (obj == null)
                obj = popupBundle;

            // init popup
            // if dialog box
            if (data.type == PopupBase.PopupType.DialogBox)
            {
                LoadDialogBox(obj, data.asset, "Error Title", "Error Message");
            }
            else
            {
                InitPopup(data.type, obj, data.asset);
            }

            // add event listeners for each button
            foreach(PopupButtonData button in data.buttons)
            {
                AddEvent(button.clickListener, button.listenerType, popupName);
            }
        }

        /// <summary>
        /// Initializes the popup.
        /// </summary>
        /// <param name="type">Popup type.</param>
        /// <param name="obj">AssetBundle or Popup.</param>
        /// <param name="assetName">Asset name of popup.</param>
        private PopupBase InitPopup(PopupBase.PopupType type, Object obj, string assetName)
        {
            // get popup from prefab or asset bundle
            GameObject popup;
            if (obj is AssetBundle)
            {
                popup = (GameObject)((AssetBundle)obj).LoadAsset(assetName);
            }
            else
            {
                popup = (GameObject)obj;
            }

            // instantiate popup holder
            GameObject holderInstance = LoadPopupHolder(type);
            // load popup and return popup instance
            GameObject popupInstance = LoadPopupPrefab(popup, holderInstance.transform);
            // get PopupUI script attached to popup
            PopupBase popupHandler = popupInstance.GetComponent<PopupBase>();
            // init popup
            popupHandler.InitPopup(holderInstance, holders.Count);
            // get transition system
            PopupTransitionSystem transition = popupInstance.GetComponent<PopupBase>().transitionSystem;
            // apply transition
            transition.TransitionIn(transition.transitionType);
            // return handler
            return popupHandler;
        }

        /// <summary>
        /// Loads popup holder.
        /// </summary>
        /// <param name="type">Popup type.</param>
        private GameObject LoadPopupHolder(PopupBase.PopupType type)
        {
            // pause game by setting time scale to zero
            Time.timeScale = 0f;

            // get correct prefab
            GameObject prefab;
            if (type == PopupBase.PopupType.Popup)
            {
                prefab = popupHolder;
            }
            else
            {
                prefab = popupHolderUI;
            }

            // instantiate popup holder
            GameObject holderInstance = (GameObject)Instantiate(prefab, Vector3.zero, Quaternion.identity);

            // if non-UI popup
            if (type == PopupBase.PopupType.Popup)
            {
                // align to camera
                Vector3 camPos = Camera.main.transform.position;
                holderInstance.transform.position = new Vector3(camPos.x, camPos.y, 0f);
            }

            // parent to PopupManager transform
            holderInstance.transform.SetParent(transform, false);
            // make popup active
            holderInstance.SetActive(true);
            // add to list
            holders.Add(holderInstance);
            // return prefab instance
            return holderInstance;
        }

        /// <summary>
        /// Loads a popup prefab. Must have transition script attached.
        /// </summary>
        /// <param name="popup">Popup.</param>
        /// <param name="background">Background transform.</param>
        private GameObject LoadPopupPrefab(GameObject popup, Transform background)
        {
            // instantiate popup
            GameObject popupInstance = (GameObject)Instantiate (popup, background.position, background.rotation);
            // add to list
            popups.Add(popupInstance);
            // parent to background
            popupInstance.transform.SetParent(background, true);
            // make active
            popupInstance.SetActive(true);
            // return popup
            return popupInstance;
        }

        /// <summary>
        /// Gets scale for fitting 960x540 popup to screen. This gives different results if not done at startup.
        /// </summary>
        /// <param name="width">Popup width.</param>
        /// <param name="height">Popup height.</param>
        private float GetFitScale(float width, float height)
        {
            // get scene width based on camera
            screenHeight = Camera.main.orthographicSize * 2.0f;
            screenRatio = (float)Screen.width / (float)Screen.height;
            float screenWidth = screenHeight * screenRatio;
            // get unit dimensions of 960x540 popup
            float popupWidth = width / 40f;
            float popupHeight = height / 40f;
            // calculate scales
            float scaleWidth = screenWidth/popupWidth;
            float scaleHeight = screenHeight/popupHeight;
            // get smallest scale
            float scale = Mathf.Min(scaleWidth, scaleHeight);
            Debug.Log("PopupManager.scaleToFit: " + scale);
            return scale;
        }

        /// <summary>
        /// When popup has loaded and transition is done. Called by PopupTransitionSystem.
        /// </summary>
        public void PopupLoaded()
        {
            Debug.Log("Popup Loaded");

            // if any of the popups hide the scene, then hide it and unpause
            foreach (GameObject popup in popups)
            {
                if (popup.GetComponent<PopupBase>().hideScene)
                {
                    hidingScene = true;
                    SceneManagerTemp.instance.ShowScene(false);
                    Time.timeScale = 1f;
                    break;
                }
            }
        }

        /// <summary>
        /// Subscribe the specified button id to listen to button clicks.
        /// </summary>
        /// <param name="popupName">Popup name.</param>
        /// <param name="id">Button dentifier.</param>
        /// <param name="callback">Callback.</param>
        public void Subscribe(string popupName, PopupButtonID id, UnityAction callback)
        {
            Dictionary<string, UnityEvent> listeners = GetListenerDict(id);
            if (listeners != null)
            {
                Debug.Log("Subscribe to " + popupName + " for " + id);
                // look for existing listener
                UnityEvent thisEvent = null;
                if (listeners.TryGetValue(popupName, out thisEvent))
                {
                    thisEvent.AddListener(callback);
                }
                // else create new one
                else
                {
                    thisEvent = new UnityEvent();
                    thisEvent.AddListener(callback);
                    listeners.Add(popupName, thisEvent);
                }
            }
        }

        /// <summary>
        /// Unsubscribe the specified button id to listen to button clicks.
        /// </summary>
        /// <param name="popupName">Popup name.</param>
        /// <param name="id">Button dentifier.</param>
        /// <param name="callback">Callback.</param>
        public void Unsubscribe(string popupName, PopupButtonID id, UnityAction callback)
        {
            Dictionary<string, UnityEvent> listeners = GetListenerDict(id);
            if (listeners != null)
            {
                // remove listener if found
                UnityEvent thisEvent = null;
                if (listeners.TryGetValue(popupName, out thisEvent))
                {
                    thisEvent.RemoveListener(callback);
                    // if there was a way to know that all listeners were removed, I would remove the event from the dictionary
                }
            }
        }

        /// <summary>
        /// Adds the event listener for the specified button id.
        /// </summary>
        /// <param name="unityEvent">Unity event.</param>
        /// <param name="id">Identifier.</param>
        /// <param name="popupName">Popup name.</param>
        public void AddEvent(UnityEvent unityEvent, PopupButtonID id, string popupName)
        {
            Dictionary<string, UnityEvent> listeners = GetListenerDict(id);
            if (listeners != null)
            {
                // if it doesn't exist add it
                UnityEvent thisEvent = null;
                if (!listeners.TryGetValue(popupName, out thisEvent))
                {
                    listeners.Add(popupName, unityEvent);
                }
                // if the dictionary has an event, then replace it with new one
                else
                {
                    thisEvent.RemoveAllListeners();
                    listeners.Remove(popupName);
                    listeners.Add(popupName, unityEvent);
                }
            }
        }

        /// <summary>
        /// Removes all the event listeners for the specified button id.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <param name="popupName">Popup name.</param>
        public void RemoveAllEvents(PopupButtonID id, string popupName)
        {
            Dictionary<string, UnityEvent> listeners = GetListenerDict(id);
            if (listeners != null)
            {
                // remove listener if found
                UnityEvent thisEvent = null;
                if (listeners.TryGetValue(popupName, out thisEvent))
                {
                    thisEvent.RemoveAllListeners();
                    listeners.Remove(popupName);
                }
            }
        }

        /// <summary>
        /// Gets the listener dictionary.
        /// </summary>
        /// <param name="id">Button dentifier.</param>
        /// <returns>The listener dictionary.</returns>
        private Dictionary<string, UnityEvent> GetListenerDict(PopupButtonID id)
        {
            Dictionary<string, UnityEvent> listeners = null;
            // get listeners by button id
            switch (id)
            {
                case PopupButtonID.CloseButton:
                    listeners = closeListeners;
                    break;
                case PopupButtonID.OKButton:
                    listeners = okListeners;
                    break;
                case PopupButtonID.CancelButton:
                    listeners = cancelListeners;
                    break;
            }
            return listeners;
        }

        /// <summary>
        /// When close button clicked
        /// </summary>
        /// <param name="popupName">Popup name.</param>
        public void ClickCloseButton(string popupName)
        {
            UnityEvent thisEvent = null;
            // remove clone if any
            popupName = popupName.Replace("(Clone)", "");
            if (closeListeners.TryGetValue (popupName, out thisEvent))
            {
                closeEvent = thisEvent;
            }
        }

        /// <summary>
        /// When ok button clicked
        /// </summary>
        /// <param name="popupName">Popup name.</param>
        public void ClickOKButton(string popupName)
        {
            UnityEvent thisEvent = null;
            // remove clone if any
            popupName = popupName.Replace("(Clone)", "");
            if (okListeners.TryGetValue (popupName, out thisEvent))
            {
                closeEvent = thisEvent;
            }
        }

        /// <summary>
        /// When cancel button clicked
        /// </summary>
        /// <param name="popupName">Popup name.</param>
        public void ClickCancelButton(string popupName)
        {
            UnityEvent thisEvent = null;
            // remove clone if any
            popupName = popupName.Replace("(Clone)", "");
            if (cancelListeners.TryGetValue (popupName, out thisEvent))
            {
                closeEvent = thisEvent;
            }
        }

        /// <summary>
        /// When popup is closing and transition is starting. Called by PopupTransitionSystem.
        /// </summary>
        /// <param name="popup">Popup.</param>
        public void TransitionOut(GameObject popup)
        {
            bool stillHiding = false;
            // check all loaded popups
            foreach (GameObject testPopup in popups)
            {
                // if not popup that is closing and popup is hiding scene
                if ((testPopup != popup) && (testPopup.GetComponent<PopupBase>().hideScene))
                {
                    stillHiding = true;
                    break;
                }
            }
            // if no more popups hide the sceene, then make scene active and pause
            if (!stillHiding)
            {
                // unpause game by setting time scale to 1
                Time.timeScale = 0.0f;
                // enable scene
                SceneManagerTemp.instance.ShowScene(true);
            }
        }

        /// <summary>
        /// Closes the popup. Transition has completed. Called by PopupTransitionSystem.
        /// </summary>
        /// <param name="popup">Popup.</param>
        public void ClosePopup(GameObject popup)
        {
            Debug.Log("Popup closed");

            // if close event, then invoke it and clear
            if (closeEvent != null)
            {
                closeEvent.Invoke();
                closeEvent = null;
            }

            // get number of active popups
            int count = popups.Count;

            // if only one popup left, then unpause game and make scene and player active
            if (count == 1)
            {
                // unpause game by setting time scale to 1
                Time.timeScale = 1.0f;
                SceneManagerTemp.instance.ShowScene(true);
            }

            // search for popup
            for (int i = 0; i != count; i++)
            {
                // if found popup in list
                if (popup == popups[i])
                {
                    // destroy popup and canvas and remove from lists
                    Destroy(popup);
                    popups.RemoveAt(i);
                    Destroy(holders[i]);
                    holders.RemoveAt(i);
                    return;
                }
            }
        }
    }
}