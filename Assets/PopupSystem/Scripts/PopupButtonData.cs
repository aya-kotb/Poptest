using UnityEngine;
using UnityEngine.Events;

// author: Scott Gilman

namespace Poptropica2.PopupSystem
{
    /// <summary>
    /// Button data for popup
    /// </summary>
    [System.Serializable]
    public class PopupButtonData
    {
        public PopupButtonID listenerType;      // popup button ID
        public UnityEvent clickListener;        // listener event
    }
}