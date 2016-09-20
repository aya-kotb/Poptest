using UnityEngine;
using System.Collections;

// author: Rick Hocker

namespace Poptropica2.PopupSystem
{
    /// <summary>
    /// Popup close button properties.
    /// </summary>
    public class PopupCloseButtonUIProps : MonoBehaviour
    {
        [Header("Close button position")]
        public Vector2 closePosition = new Vector2(-30f, -30f);     // position for close button from upper right
    }
}