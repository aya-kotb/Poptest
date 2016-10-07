using UnityEngine;
using System.Collections;

// author: Rick Hocker

namespace Poptropica2.PopupSystem
{
	/// <summary>
	/// popup transitions class attach this script to the popup gameObject, select type of transition type 
	/// </summary>
	public class PopupTransitionSystem : MonoBehaviour
    {
		// Types of popup transitions 
		public enum TransitionType
        {
            None,
			MoveLeft,
			MoveRight,
			MoveDown,
			MoveUp,
			ScaleIn,
            ScaleInBack,
			FadeIn,
		}

		public TransitionType transitionType;           // type of popup transitions
		public float transitionTime = 1;                // time of transition
		public float delay = 0;                         // delay before transition starts

        [Header("To Suppress Transitions When Clicking On Buttons")]
        public bool closeIgnoresTransition = false;     // clicking close button ignores transition
        public bool okIgnoresTransition = false;        // clicking ok button ignores transition
        public bool cancelIgnoresTransition = true;     // clicking cancel button ignores transition

		private RectTransform rectTransform;            // rect transform for UI popup used for transition
        private Vector3 startScale;                     // start scale for non-UI popup
        private float startX;                           // start x pos for non-UI popup
        private float startY;                           // start y pos for non-UI popup
        private float offsetX;                          // x offset for transition
        private float offsetY;                          // y offset for transition

		void Awake()
		{
			// Get the transforms
			rectTransform = gameObject.GetComponent<RectTransform> ();
		}

		/// <summary>
		/// Executes transition in.
		/// </summary>
		/// <param name="_transitionType">Transition type.</param>
		public void TransitionIn(TransitionType _transitionType)
        {
            // if UI popup
            if (rectTransform != null)
            {
                rectTransform.localScale = new Vector3(1, 1, 1);
                offsetX = Screen.width * 2.0f;
                offsetY = Screen.height * 2.0f;
            }
            else
            {
                startX = transform.position.x;
                startY = transform.position.y; 
                startScale = transform.localScale;
                offsetY = PopupManager.instance.screenHeight;
                offsetX = offsetY * PopupManager.instance.screenRatio;
            }

			switch(_transitionType) 
			{
                case TransitionType.None:
                    PopupOpenCallback();
                    break;
    			case TransitionType.MoveLeft:
                    MovementModeXAxisIn(1);
    				break;
    			case TransitionType.MoveRight:
                    MovementModeXAxisIn(-1);
    				break;
    			case TransitionType.MoveDown:
                    MovementModeYAxisIn(1);
    				break;
    			case TransitionType.MoveUp:
                    MovementModeYAxisIn(-1);
    				break;
    			case TransitionType.ScaleIn:
                    ScaleIn(LeanTweenType.easeOutSine);
    				break;
                case TransitionType.ScaleInBack:
                    ScaleIn(LeanTweenType.easeOutBack);
                    break;
    			case TransitionType.FadeIn:
    				FadeIn();
    				break;
			}
		}

        /// <summary>
        /// Executes transition out by doing reverse of in transition.
        /// </summary>
        /// <param name="_transitionType">Transition type.</param>
        public void TransitionOut(TransitionType _transitionType)
        {
            // notify popup manager
            PopupManager.instance.TransitionOut(gameObject);

            switch(_transitionType) 
            {
                case TransitionType.None:
                    PopupCloseCallback();
                    break;
                case TransitionType.MoveLeft:
                    MovementModeXAxisOut(1);
                    break;
                case TransitionType.MoveRight:
                    MovementModeXAxisOut(-1);
                    break;
                case TransitionType.MoveDown:
                    MovementModeYAxisOut(1);
                    break;
                case TransitionType.MoveUp:
                    MovementModeYAxisOut(-1);
                    break;
                case TransitionType.ScaleIn:
                    ScaleOut(LeanTweenType.easeInSine);
                    break;
                case TransitionType.ScaleInBack:
                    ScaleOut(LeanTweenType.easeInBack);
                    break;
                case TransitionType.FadeIn:
                    FadeOut();
                    break;
            }
        }

		/// <summary>
		/// Move in popup X axis.
		/// </summary>
        /// <param name="dirX">Direction value x.</param>
		void MovementModeXAxisIn(float dirX)
        {
            if (rectTransform == null)
            {
                transform.position = new Vector3(startX + dirX * offsetX, startY, 0f);
                LeanTween.moveX(gameObject, startX, transitionTime).setEase(LeanTweenType.easeOutSine).setDelay(delay).setOnComplete(PopupOpenCallback).setIgnoreTimeScale(true);
            }
            else
            {
                rectTransform.anchoredPosition3D += new Vector3(dirX * offsetX, 0, 0f);
                LeanTween.moveX(rectTransform, rectTransform.anchoredPosition3D.x - dirX * offsetX, transitionTime).setEase(LeanTweenType.easeOutSine).setDelay(delay).setOnComplete(PopupOpenCallback).setIgnoreTimeScale(true);
            }
        }

        /// <summary>
        /// Move out popup X axis.
        /// </summary>
        /// <param name="dirX">Direction value x.</param>
        void MovementModeXAxisOut(float dirX)
        {
            if (rectTransform == null)
            {
                LeanTween.moveX(gameObject, startX + dirX * offsetX, transitionTime).setEase(LeanTweenType.easeInSine).setOnComplete(PopupCloseCallback).setIgnoreTimeScale(true);
            }
            else
            {
                LeanTween.moveX(rectTransform, rectTransform.anchoredPosition3D.x + dirX * offsetX, transitionTime).setEase(LeanTweenType.easeInSine).setOnComplete(PopupCloseCallback).setIgnoreTimeScale(true);
            }
        }

        /// <summary>
        /// Move in popup Y axis.
        /// </summary>
        /// <param name="dirY">Direction value y.</param>
        void MovementModeYAxisIn(float dirY) 
        {
            if (rectTransform == null)
            {
                transform.position = new Vector3(startX, startY + dirY * offsetY, 0f);
                LeanTween.moveY(gameObject, startY, transitionTime).setEase(LeanTweenType.easeOutSine).setDelay(delay).setOnComplete(PopupOpenCallback).setIgnoreTimeScale(true);
            }
            else
            {
                rectTransform.anchoredPosition3D += new Vector3(0, dirY * offsetY, 0f);
                LeanTween.moveY(rectTransform, rectTransform.anchoredPosition3D.y - dirY * offsetY, transitionTime).setEase(LeanTweenType.easeOutSine).setDelay(delay).setOnComplete(PopupOpenCallback).setIgnoreTimeScale(true);
            }
        }

        /// <summary>
        /// Move out popup Y axis.
        /// </summary>
        /// <param name="dirY">Direction value y.</param>
        void MovementModeYAxisOut(float dirY)
        { 
            if (rectTransform == null)
            {
                LeanTween.moveY(gameObject, startY + dirY * offsetY, transitionTime).setEase(LeanTweenType.easeInSine).setOnComplete(PopupCloseCallback).setIgnoreTimeScale(true);
            }
            else
            {
                LeanTween.moveY(rectTransform, rectTransform.anchoredPosition3D.y + dirY * offsetY, transitionTime).setEase(LeanTweenType.easeInSine).setOnComplete(PopupCloseCallback).setIgnoreTimeScale(true);
            }
        }

        /// <summary>
        /// Scales popup in.
        /// </summary>
        void ScaleIn(LeanTweenType type)
        {
            if (rectTransform == null)
            {
                gameObject.transform.localScale = Vector3.zero;
                LeanTween.scale(gameObject, startScale, transitionTime).setEase(type).setDelay(delay).setOnComplete(PopupOpenCallback).setIgnoreTimeScale(true);
            }
            else
            {
                rectTransform.localScale = Vector3.zero;
                LeanTween.scale(rectTransform, new Vector3(1f, 1f, 1f), transitionTime).setEase(type).setDelay(delay).setOnComplete(PopupOpenCallback).setIgnoreTimeScale(true);
            }
        }

        /// <summary>
        /// Scales popup out.
        /// </summary>
        void ScaleOut(LeanTweenType type)
        {
            if (rectTransform == null)
            {
                LeanTween.scale(gameObject, new Vector3(0f, 0f, 0f), transitionTime).setEase(type).setOnComplete(PopupCloseCallback).setIgnoreTimeScale(true);
            }
            else
            {
                LeanTween.scale(rectTransform, new Vector3(0f, 0f, 0f), transitionTime).setEase(type).setOnComplete(PopupCloseCallback).setIgnoreTimeScale(true);
            }
        }

        /// <summary>
        /// Fades in the complete popup. Requires a canvas group.
        /// </summary>
        public void FadeIn()
        {
            if (rectTransform == null)
            {
                // TODO: add fade in support
                PopupOpenCallback();
            }
            else
            {
                CanvasGroup canvasGroup = rectTransform.GetComponent<CanvasGroup>();
                LeanTween.value(gameObject, 0f, 1f, transitionTime).setOnUpdate(
                    (float value) =>
                    {
                        canvasGroup.alpha = value;
                    }
                ).setOnComplete(PopupOpenCallback).setIgnoreTimeScale(true);
            }
        }

        /// <summary>
        /// Fades out the complete popup. Requires a canvas group.
        /// </summary>
        public void FadeOut()
        {
            if (rectTransform == null)
            {
                // TODO: add fade out support
                PopupCloseCallback();
            }
            else
            {
                // requires a canvas group
                CanvasGroup canvasGroup = rectTransform.GetComponent<CanvasGroup>();
                LeanTween.value(gameObject, 1f, 0f, transitionTime).setOnUpdate(
                    (float value) =>
                    {
                        canvasGroup.alpha = value;
                    }
                ).setOnComplete(PopupCloseCallback).setIgnoreTimeScale(true);
            }
        }

		/// <summary>
		/// Popup open callback.
		/// If needed fire an event from here to notify other subscribers about Minigame game popup
		/// </summary>
		public void PopupOpenCallback()
        {
            PopupManager.instance.PopupLoaded();
		}

		/// <summary>
		/// Popup close callback.
		/// If needed fire an event from here to notify other subscribers about Minigame game popup
		/// </summary>
		public void PopupCloseCallback()
        {
            // notify popup manager (will destroy game object)
            PopupManager.instance.ClosePopup(gameObject);
		}
	}
}