using UnityEngine;
using System.Collections;

namespace Poptropica2.IslandSystem
{
    /// <summary>
    /// Map special panel will show the speacil items achieved
    /// The UI view is dummy.
    /// </summary>
    public class MapSpecialPanel : PanelTransition {
        
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
            AlphaTween(semiTransparentScreen, alpha);
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
