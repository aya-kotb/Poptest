using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// author: Rick Hocker

namespace Poptropica2.PopupSystem
{
    /// <summary>
    /// Interface for common popup buttons.
    /// </summary>
    public interface IPopupButtons
    {
        void OnCloseButtonClicked();
	}
}