using UnityEngine;
using System.Collections;

namespace Poptropica2.IslandSystem
{
    public class PanelTransition : MonoBehaviour
    {
        [Header("Transition Variables")]
        public float alpha = 0.5f;
        public float scaleTime = 0.5f;          // Time to complete the scale tweeen
        public float alphaTransTime = 0.5f;     // Time  to complete the alpha tween
        public float moveTime = 0.5f;           // Time to complete the tween movement
        public RectTransform semiTransparentScreen;
        public RectTransform childPanel;
        public CanvasGroup canvasGroup;

        /// <summary>
        /// Scales the local scale of rect transform to one. Using Lean Tween effects.
        /// </summary>
        /// <param name="rectTransform">RectTransform that you wish to attach the tween to.</param>
        /// <param name="OnCompleteCallback">callback after completing the transition.</param>
        public virtual void ScaleOne (RectTransform rectTransform, System.Action onCompleteCallback = null)
        {
            rectTransform.localScale = Vector3.zero;
            LTDescr scaleTween = LeanTween.scale(rectTransform, Vector3.one, scaleTime);

            if (onCompleteCallback != null)
            {
                scaleTween.setOnComplete(onCompleteCallback);
            }
        }

        /// <summary>
        /// Scales the local scale of rect transform to zero. Using Lean Tween effects.
        /// </summary>
        /// <param name="rectTransform">RectTransform that you wish to attach the tween to.</param>
        /// <param name="OnCompleteCallback">callback after completing the transition.</param>
        public virtual void ScaleZero (RectTransform rectTransform, System.Action onCompleteCallback = null)
        {
            LTDescr scaleTween = LeanTween.scale(rectTransform, Vector3.zero, scaleTime);

            if (onCompleteCallback != null)
            {
                scaleTween.setOnComplete(onCompleteCallback);
            }
        }

        /// <summary>
        /// Scales the local scale of rect transform to given scale. Using Lean Tween effects.
        /// </summary>
        /// <param name="rectTransform">RectTransform that you wish to attach the tween to.</param>
        /// <param name="to">Vector3 The final Vector3 with which to tween to (localScale).</param>
        /// <param name="onCompleteCallback">callback after completing the transition.</param>
        public virtual void ScaleTo (RectTransform rectTransform, Vector3 to, System.Action onCompleteCallback = null )
        {
            LTDescr scaleTween = LeanTween.scale(rectTransform, to, scaleTime);

            if (onCompleteCallback != null)
            {
                scaleTween.setOnComplete(onCompleteCallback);
            }           
        }

        /// <summary>
        /// Fade in/out an alpha of color conatin in RectTransform object. Usinging Lean Tween effects.
        /// Must Contain Unity UI Image component in object
        /// </summary>
        /// <param name="semiTransparentScreen">RectTransform that you wish to attach the tween to.</param>
        /// <param name="alpha">float the final alpha value (0-1).</param>
        /// <param name="OnCompleteCallback">callback after completing the transition.</param>
        public virtual void AlphaTween (RectTransform semiTransparentScreen, float alpha, System.Action OnCompleteCallback = null)
        {
            LTDescr alphaTween = LeanTween.alpha(semiTransparentScreen, alpha, alphaTransTime);

            if (OnCompleteCallback != null)
            {
                alphaTween.setOnComplete(OnCompleteCallback);
            }
        }

        /// <summary>
        /// Fade in/out an alpha of every component under canvas group of an object.
        /// </summary>
        /// <param name="canvasGroup">RectTransform that the CanvasGroup is attached to.</param>
        /// <param name="alpha">float the final alpha value (0-1).</param>
        /// <param name="OnCompleteCallback">callback after transition completes.</param>
        public virtual void CanvasGroupTween (CanvasGroup canvasGroup, float alpha = 1f, System.Action OnCompleteCallback = null)
        {
            LTDescr alphaTween = LeanTween.alphaCanvas(canvasGroup, alpha, alphaTransTime);

            if (OnCompleteCallback != null)
            {
                alphaTween.setOnComplete(OnCompleteCallback);
            }
        }

        /// <summary>
        /// Moves a RectTransform object.
        /// </summary>
        /// <param name="rectTransform">RectTransform that you wish to attach the tween to.</param>
        /// <param name="to">Vector3 The final Vector3 with which to tween to.</param>
        /// <param name="time">float The time to complete the tween in.</param>
        /// <param name="onCompleteCallback">callback after completing the transition.</param>
        public virtual void MoveTween (RectTransform rectTransform, Vector3 to, float time = 0.5f, System.Action onCompleteCallback = null)
        {
            LTDescr moveTween = LeanTween.move(rectTransform, to, time);

            if (onCompleteCallback != null)
            {
                moveTween.setOnComplete(onCompleteCallback);
            }
        }
    }
}
