using UnityEngine;
using System.Collections;

namespace Poptropica2.IslandSystem
{
    /// <summary>
    /// Video trailer panel is a UI class
    /// which contain video screen to play video.
    /// </summary>
    public class VideoTrailerPanel : PanelTransition
    {
        // Use this for initialization
        void Start () {
            //UI Animation for displaying panel
            AlphaTween(semiTransparentScreen, alpha);
            ScaleOne(childPanel);
        }

        /// <summary>
        /// Triggers the close button event.
        /// </summary>
        public void OnClickCloseButton ()
        {
            //UI Animation for hiding panel
            AlphaTween(semiTransparentScreen, 0);
            ScaleZero(childPanel, this.OnCompleteTransition);
        }

        /// <summary>
        /// Callback after completing UI transition effect.
        /// </summary>
        void OnCompleteTransition ()
        {
            Destroy (gameObject);
        }
    }
}
