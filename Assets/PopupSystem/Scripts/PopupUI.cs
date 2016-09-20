using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// author: Rick Hocker

namespace Poptropica2.PopupSystem
{
    /// <summary>
    /// Class for UI canvas popups.
    /// </summary>
    public class PopupUI : PopupUIBase, IPopupButtons
    {
        private PopupCloseButtonUIProps closeButtonProps;   // close button properties

        /// <summary>
        /// Initializes the popup.
        /// </summary>
        /// <param name="holderInstance">Holder instance.</param>
        /// <param name="index">Index in holder list.</param>
        public override void InitPopup(GameObject holderInstance, int index)
        {
            // set background
            SetBackgroundColor(holderInstance);

            // add close button if script attached
            closeButtonProps = GetComponent<PopupCloseButtonUIProps>();
            if (closeButtonProps != null)
            {
                AddCloseButton();
            }
        }

        /// <summary>
        /// Adds the close button.
        /// </summary>
        private void AddCloseButton()
        {
            // instantiate close button
            GameObject closeButtonInstance = (GameObject)Instantiate(PopupManager.instance.closeButtonUI, Vector3.zero, Quaternion.identity);
            // parent to popup instance
            closeButtonInstance.transform.SetParent(transform, false);
            // get close button UI sprite
            Button button = closeButtonInstance.transform.GetChild(0).GetComponent<Button>();
            // position button from upper right
            button.GetComponent<RectTransform>().localPosition = new Vector3(closeButtonProps.closePosition.x, closeButtonProps.closePosition.y, 0f);
            // set button click listener
            button.onClick.AddListener(delegate
                {
                    OnCloseButtonClicked();
                });
        }

        /// <summary>
        /// Raises the close button clicked event and closes popup. Can be overriden.
        /// </summary>
        public void OnCloseButtonClicked()
        {
            PopupManager.instance.ClickCloseButton(gameObject.name);
            Close(transitionSystem.closeIgnoresTransition);
        }
	}
}