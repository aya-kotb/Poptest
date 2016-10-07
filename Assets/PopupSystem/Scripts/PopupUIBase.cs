using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// author: Rick Hocker

namespace Poptropica2.PopupSystem
{
    /// <summary>
    /// Base class for all UI popups.
    /// </summary>
	public class PopupUIBase : PopupBase
    {
        /// <summary>
        /// Sets the color of the background.
        /// </summary>
        /// <param name="holderInstance">Holder instance.</param>
        protected void SetBackgroundColor(GameObject holderInstance)
        {
            Transform background = holderInstance.transform.GetChild(0);
            background.GetComponent<Image>().color = backgroundColor;
        }
    }
}