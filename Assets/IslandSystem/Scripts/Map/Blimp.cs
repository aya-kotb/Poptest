using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Poptropica2.IslandSystem
{
    /// <summary>
    /// This Blimp class controls the Blimp object.
    /// It moves the Blimp object towards the position where mouse is clicked.
    /// Or towards the given target
    /// </summary>
    public class Blimp : MonoBehaviour {

        public float speed = 10f;
        public LeanTweenType leanTweentype = LeanTweenType.easeInExpo;
        public MapItem targetItem;
        RectTransform rectTransform;
        Image image;
        int leanTweenUniqueID = 0;

        // Use this for initialization
        void Start ()
        {
            rectTransform = GetComponent<RectTransform>();
            image = GetComponent<Image>();
        }
        
        /// <summary>
        /// Moves the blimp.
        /// It will move towards given target and Triggers callback after reaching target position.
        /// </summary>
        /// <param name="target">Target for Blimp to reach.</param>
        public void MoveBlimp (MapItem target)
        {
            if (this.targetItem != target)
            {
                if (leanTweenUniqueID != 0)
                {
                    LeanTween.cancel(leanTweenUniqueID);
                }
                
                this.targetItem = target;
                leanTweenUniqueID = LeanTween.move(gameObject, target.transform.position, 10f).setEase(leanTweentype).setSpeed(speed).setOnComplete(OnCompleteMovement).uniqueId;
            }
        }

        /// <summary>
        /// Triggers this method when Blimp reach the given target..
        /// </summary>
        void OnCompleteMovement ()
        {
            Island targetIsland = targetItem as Island;
            targetIsland.ViewIsland();
            targetItem = null;
            leanTweenUniqueID = 0;
        }

        /// <summary>
        /// Moves the blimp.
        /// It will move the blimp towards the mouse position;
        /// </summary>
        public void MoveBlimpToMousePos ()
        {
            Vector3 mousePos = Camera.current.ScreenToWorldPoint(CheckForBoundary(Input.mousePosition));
            mousePos.z = this.transform.position.z;
            LeanTween.move(gameObject, mousePos, 10f).setEase(leanTweentype).setSpeed(speed);
        }

        /// <summary>
        /// Checks for boundary screen boundary where mouse is clicked.
        /// Since Blimp should not cross the screen boundary
        /// </summary>
        /// <returns>Return the calculated new postion.</returns>
        /// <param name="mousePosition">position of Mouse click.</param>
        Vector3 CheckForBoundary (Vector3 mousePosition)
        {
            Vector3 position = mousePosition;

            //Check for start corner of Screen width
            if (position.x < (image.sprite.texture.width / 2))
            {
                position.x = (float)(image.sprite.texture.width / 2);
            }
            //Check for end corner of Screen width
            else if (position.x > (Screen.width - (image.sprite.texture.width / 2)))
            {
                position.x = (Screen.width - (image.sprite.texture.width / 2));
            }

            //Check for button of Screen height
            if (position.y < (image.sprite.texture.height / 2))
            {
                position.y = (float)(image.sprite.texture.height / 2);
            }
            //Check for top of Screen height
            else if (position.y > (Screen.height - (image.sprite.texture.height / 2)))
            {
                position.y = (Screen.height - (image.sprite.texture.height / 2));
            }

            return position;
        }

    }
}
