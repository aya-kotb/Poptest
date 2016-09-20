using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// author: Rick Hocker

namespace Poptropica2.PopupSystem
{
    /// <summary>
    /// Class to override button functions for dialog box popups.
    /// </summary>
    public class PopupDialogBoxOverride : PopupDialogBox
    {
		/// <summary>
		/// Raises the ok button clicked event.
		/// </summary>
		public override void OnOkButtonClicked()
        {
            Debug.Log("Click OK button");
            base.OnOkButtonClicked();
		}

        /// <summary>
        /// Raises the cancel button clicked event.
         /// </summary>
        public override void OnCancelButtonClicked()
        {
            Debug.Log("Click Cancel button");
            base.OnCancelButtonClicked();
        }
	}
}