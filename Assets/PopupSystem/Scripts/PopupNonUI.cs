using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// author: Rick Hocker

namespace Poptropica2.PopupSystem
{
    /// <summary>
    /// Class for non-UI popups.
    /// </summary>
	public class PopupNonUI : PopupBase, IPopupButtons
    {
        [Header("Scaling popup to fit screen")]
        public bool scaleToFit = false;                             // scale 960x540 popup to fit

        [Header("Scaling popup to fit screen")]
        public Vector2 blackFrame = Vector2.zero;                   // black frame around popup to hide art that extends outside popup

        [HideInInspector] public float scale = 1.0f;                // final scale of scaled popup

        private PopupCloseButtonProps closeButtonProps;             // close button properties

        /// <summary>
        /// Initializes the popup.
        /// </summary>
        /// <param name="holderInstance">Holder instance.</param>
        /// <param name="index">Index in holder list.</param>
        public override void InitPopup(GameObject holderInstance, int index)
        {
            // set background color
            Transform background = holderInstance.transform.GetChild(0);
            background.GetComponent<SpriteRenderer>().color = backgroundColor;

            // add close button if script attached
            closeButtonProps = GetComponent<PopupCloseButtonProps>();
            if (closeButtonProps != null)
            {
                AddCloseButton(holderInstance, index);
            }

            // if black frame requested, then set it up
            if ((blackFrame.x != 0) && (blackFrame.y != 0))
            {
                // size of panel in pixels
                float squareSize = 64f;
                // pixels per unit
                float units = 40f;
                // instantiate frame
                GameObject frameInstance = (GameObject)Instantiate(PopupManager.instance.blackFrame, Vector3.zero, Quaternion.identity);
                // parent to popup
                frameInstance.transform.SetParent(transform, false);
                // for each side (Left, Right, Top, Bottom)
                for (int i = 0; i != 4; i++)
                {
                    Transform side = frameInstance.transform.GetChild(i);
                    // set sort order
                    side.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 32750 + index;
                    // set scale and position of each side
                    float xScale = blackFrame.x / squareSize;
                    float yScale = blackFrame.y / squareSize;
                    float xOffset = 1.5f * blackFrame.x / units;
                    float yOffset = 1.5f * blackFrame.y / units;
                    switch (i)
                    {
                        case 0: // left
                            side.localScale = new Vector3(2 * xScale, 2 * xScale, 0f);
                            side.localPosition = new Vector3(-xOffset, 0f, 0f);
                            break;
                        case 1: // right
                            side.localScale = new Vector3(2 * xScale, 2 * xScale, 0f);
                            side.localPosition = new Vector3(xOffset, 0f, 0f);
                            break;
                        case 2: // top
                            side.localScale = new Vector3(xScale, 2 * yScale, 0f);
                            side.localPosition = new Vector3(0f, yOffset, 0f);
                            break;
                        case 3: // bottom
                            side.localScale = new Vector3(xScale, 2 * yScale, 0f);
                            side.localPosition = new Vector3(0f, -yOffset, 0f);
                            break;
                    }
                }
            }

            // if scaling to fit
            if (scaleToFit)
            {
                scale = PopupManager.instance.fitScale;
                // scale popup
                transform.localScale = new Vector3(scale, scale, 1f);
            }
        }

        /// <summary>
        /// Adds the close button.
        /// </summary>
        /// <param name="holderInstance">Holder instance.</param>
        /// <param name="index">Index in holder list.</param>
        private void AddCloseButton(GameObject holderInstance, int index)
        {
            // instantiate close button
            GameObject closeButtonInstance = (GameObject)Instantiate(PopupManager.instance.closeButton, Vector3.zero, Quaternion.identity);
            // parent to popup instance
            closeButtonInstance.transform.SetParent(transform, true);
            // move button to position
            closeButtonInstance.transform.localPosition = new Vector3(closeButtonProps.closePosition.x, closeButtonProps.closePosition.y, 0f);
            // set button script property to this
            closeButtonInstance.GetComponent<PopupCloseButton>().script = (IPopupButtons)this;
            // set sort order
            closeButtonInstance.GetComponent<SpriteRenderer>().sortingOrder = 32750 + index;
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