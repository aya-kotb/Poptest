using UnityEngine;
using System.Collections;

namespace Poptropica2.IslandSystem
{
    /// <summary>
    /// Map special panel will show the speacil items achieved
    /// The UI view is dummy.
    /// </summary>
    public class MapSpecialPanel : MonoBehaviour {
        
        public RectTransform childPanel;
        public RectTransform semiTransparentScreen;
        
        // Use this for initialization
        void Start () {
            ShowTransaction();
        }
        
        /// <summary>
        /// Raises the click close button event.
        /// </summary>
        public void OnClickCloseButton ()
        {
            HideTransaction();
        }
        
        /// <summary>
        /// Show the Map Special item panel with transaction effect.
        /// </summary>
        void ShowTransaction ()
        {
            childPanel.transform.localScale = Vector3.zero;
            LeanTween.alpha (semiTransparentScreen, 0.5f, 0.5f);
            LeanTween.scale (childPanel, Vector3.one, 0.5f);
        }
        
        /// <summary>
        /// Hide the Map Special item panel with transaction effect.
        /// </summary>
        void HideTransaction ()
        {
            LeanTween.alpha (semiTransparentScreen, 0.0f, 0.5f);
            LeanTween.scale (childPanel, Vector3.zero, 0.5f).setOnComplete(OnCompleteTransaction);
        }
        
        void OnCompleteTransaction ()
        {
            Destroy (gameObject);
        }
    }
}
