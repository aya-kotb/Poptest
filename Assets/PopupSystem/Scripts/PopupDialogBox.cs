using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// author: Rick Hocker

namespace Poptropica2.PopupSystem
{
    /// <summary>
    /// Class for dialog box popups.
    /// </summary>
	public class PopupDialogBox : PopupUIBase
    {
        /// <summary>
        /// Initializes the popup.
        /// </summary>
        /// <param name="holderInstance">Holder instance.</param>
        /// <param name="index">Index in holder list.</param>
        public override void InitPopup(GameObject holderInstance, int index)
        {
            // set background
            SetBackgroundColor(holderInstance);

            // get ok button UI sprite
            Transform okButton = transform.Find("OkButton");
            if (okButton != null)
            {
                // set button click listener
                okButton.GetComponent<Button>().onClick.AddListener(delegate
                    {
                        PopupManager.instance.ClickOKButton(gameObject.name);
                        OnOkButtonClicked();
                    });
            }

             // get cancel button UI sprite
            Transform cancelButton = transform.Find("CancelButton");
            if (cancelButton != null)
            {
                // set button click listener
                cancelButton.GetComponent<Button>().onClick.AddListener(delegate
                    {
                        PopupManager.instance.ClickCancelButton(gameObject.name);
                        OnCancelButtonClicked();
                    });
            }
        }

		/// <summary>
		/// Raises the ok button clicked event and closes popup. Can be overriden.
		/// </summary>
        public virtual void OnOkButtonClicked()
        {
            Close(transitionSystem.okIgnoresTransition);
 		}

        /// <summary>
        /// Raises the cancel button clicked event and closes popup. Can be overriden.
        /// </summary>
        public virtual void OnCancelButtonClicked()
        {
            Close(transitionSystem.cancelIgnoresTransition);
        }

		/// <summary>
		/// Sets the title and text for the popup.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="message">Message.</param>
		public void SetTextMessage(string title, string message)
        {
            Transform titleTransform = transform.Find("TitleText");
            titleTransform.GetComponent<Text>().text = title;
            Transform messageTransform = transform.Find("MessageText");
            messageTransform.GetComponent<Text>().text = message;
 		}
	}
}