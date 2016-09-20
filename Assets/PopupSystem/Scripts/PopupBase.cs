using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// author: Rick Hocker

namespace Poptropica2.PopupSystem
{
    /// <summary>
    /// Base class for all popups.
    /// </summary>
	public class PopupBase : MonoBehaviour
    {
        // types of popups
        public enum PopupType
        {
            Popup,
            UIPopup,
            DialogBox
        }

        [Header("Common Popup Properties")]
        public bool hideScene = false;                              // hide the scene when popup loaded
        public Color backgroundColor = new Color32(0, 0, 0, 127);   // background color (default is black at 50% alpha)

        // transition system attached to prefab
        [HideInInspector]public PopupTransitionSystem transitionSystem;

        void Awake()
        {
            // get transition system component from game object
            transitionSystem = gameObject.GetComponent<PopupTransitionSystem>();
            // if no transition system, then add default component
            if (transitionSystem == null)
            {
                transitionSystem = (PopupTransitionSystem)gameObject.AddComponent<PopupTransitionSystem>();
            }

        }

        /// <summary>
        /// Initializes the popup. To be overriden.
        /// </summary>
        /// <param name="holderInstance">Holder instance.</param>
        /// <param name="index">Index in holder list.</param>
        public virtual void InitPopup(GameObject holderInstance, int index)
        {
            // to be overriden
        }

        /// <summary>
        /// Close popup handler for any buttons
        /// </summary>
        /// <param name="ignoreTransition">If set to <c>true</c> ignore transition.</param>
        public void Close(bool ignoreTransition = true)
        {
            // if ignoring transition, then do none
            if (ignoreTransition)
            {
                transitionSystem.TransitionOut(PopupTransitionSystem.TransitionType.None);
            }
            else
            {
                // else execute reverse transition
                transitionSystem.TransitionOut(transitionSystem.transitionType);
            }               
        }
	}
}