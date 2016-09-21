using UnityEngine;
using System.Collections;

namespace Poptropica2.IslandSystem
{
    /// <summary>
    /// Video trailer panel is a UI class
    /// which contain video screen to play video.
    /// Video implemantation is in Hold...
    /// </summary>
    public class VideoTrailerPanel : MonoBehaviour {
        
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
        /// Show the Video Panel with transaction effect.
        /// </summary>
        void ShowTransaction ()
        {
            childPanel.transform.localScale = Vector3.zero;
            LeanTween.alpha (semiTransparentScreen, 0.8f, 0.5f);
            LeanTween.scale(childPanel, Vector3.one, 0.5f).setOnComplete(OnCompleteShowTransaction);
        }

        /// <summary>
        /// Hide the Video Panel with transaction effect.
        /// </summary>
        void HideTransaction ()
        {
            LeanTween.alpha (semiTransparentScreen, 0.0f, 0.5f);
            LeanTween.scale (childPanel, Vector3.zero, 0.5f).setOnComplete(OnCompleteHideTransaction);
        }

        void OnCompleteShowTransaction ()
        {
            Debug.Log("Play Video");
        }

        void OnCompleteHideTransaction ()
        {
            Destroy (gameObject);
        }
    }
}
