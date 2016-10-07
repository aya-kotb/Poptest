using UnityEngine;
using System.Collections.Generic;

// author: Scott Gilman

namespace Poptropica2.PopupSystem
{
    /// <summary>
    /// Popup data for testing.
    /// </summary>
    [System.Serializable]
    public class PopupData
    {
        public PopupBase.PopupType type;            // popup type
        public GameObject prefab;                   // prefab
        public string asset;                        // asset name within asset bundle
        public List<PopupButtonData> buttons;       // list of button data objects
    }
}