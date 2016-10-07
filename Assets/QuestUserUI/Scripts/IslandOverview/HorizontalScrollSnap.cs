using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Poptropica2.QuestUI
{
	/// <summary>
	/// Note: This class is part of unity extension UI which is used for horizontal scroll panels.
	/// It's used to make smooth movement of panels 
	/// And it'll handle dot visibility and left and right button functionality on the episode panel
	/// </summary>
    public class HorizontalScrollSnap : MonoBehaviour
    {
		private Transform screensContainer;

		private int screens = 1;

        private System.Collections.Generic.List<Vector3> positions;
		private ScrollRect scrollRect;
		private Vector3 lerpTarget;
		private bool isLerp;

        [Tooltip("The gameobject that contains toggles which suggest pagination. (optional)")]
        public GameObject Pagination;

        [Tooltip("Button to go to the next page. (optional)")]
        public GameObject NextButton;
        [Tooltip("Button to go to the previous page. (optional)")]
        public GameObject PrevButton;
        [Tooltip("Transition speed between pages. (optional)")]
        public float transitionSpeed = 7.5f;

        [Tooltip("The currently active page")]
        [SerializeField]
        private int currentScreen;

        [Tooltip("The screen / page to start the control on")]
        public int StartingScreen = 1;

        [Tooltip("The distance between two pages, by default 3 times the height of the control")]
        public int PageStep = 0;

		private Vector2 firstPressPosition; 
		private Vector2 secondPressPosition;
		private Vector2 currentSwipe;

		private float swipeValue = 0.5f; //To check swipe left or right
		private float touchDuration = 0;
		private float touchDurationMaxLimit = 0.5f;

		/// <summary>
		/// Gets the current page.
		/// </summary>
		/// <value>The current page.</value>
        public int CurrentPage
        {
            get
            {
				return currentScreen;
            }
        }
        
        void Start()
        {
            //Get ScrollRect for adding episode panels
			scrollRect = gameObject.GetComponent<ScrollRect>();

			//Get RectTransform for get its size
			RectTransform rectTransform = GetComponent<RectTransform>();

			//Get BoxCollider2D for define swipe area 
			BoxCollider2D swipeArea = GetComponent<BoxCollider2D> ();

			//Add Swipe area to the collider
			Vector2 swipeAreaSize = swipeArea.size;
			swipeAreaSize.x = rectTransform.rect.width;
			swipeAreaSize.y = rectTransform.rect.height;
			swipeArea.size = swipeAreaSize;

            if (scrollRect.horizontalScrollbar || scrollRect.verticalScrollbar)
            {
                Debug.LogWarning("Warning, using scrollbors with the Scroll Snap controls is not advised as it causes unpredictable results");
            }

            screensContainer = scrollRect.content;
            if (PageStep == 0)
            {
                PageStep = (int)scrollRect.GetComponent<RectTransform>().rect.width * 1;
            }
			 
            DistributePages();

            isLerp = false;
            currentScreen = StartingScreen;

            scrollRect.horizontalNormalizedPosition = (float)(currentScreen - 1) / (screens - 1);

            ChangeBulletsInfo(currentScreen-1);

            if (NextButton)
                NextButton.GetComponent<Button>().onClick.AddListener(() => { NextScreen(); });

            if (PrevButton)
                PrevButton.GetComponent<Button>().onClick.AddListener(() => { PreviousScreen(); });

			PrevButton.SetActive (false);
        }

		void Update()
        {
			//Swipe episode panels left or right
			CheckSwipe ();

            if (isLerp)
            {
                screensContainer.localPosition = Vector3.Lerp(screensContainer.localPosition, lerpTarget, transitionSpeed * Time.deltaTime);
                if (Vector3.Distance(screensContainer.localPosition, lerpTarget) < 0.005f)
                {
                    isLerp = false;
                }

                //Change the info bullets at the bottom of the screen. Just for visual effect
                if (Vector3.Distance(screensContainer.localPosition, lerpTarget) < 10f)
                {
                    ChangeBulletsInfo(CurrentScreen());
                }
            }

			ChangeButtonState ();
        }

		/// <summary>
		/// Changes the state of left & right button.
		/// </summary>
		public void ChangeButtonState() 
		{
			if (CurrentScreen () > 0) 
			{
				PrevButton.SetActive (true);
			} else {
				PrevButton.SetActive (false);
			}

			if (CurrentScreen () < screens - 1) {
				NextButton.SetActive (true);
			} else {
				NextButton.SetActive (false);
			}
		}

		/// <summary>
		/// Function for switching screens with buttons
		/// </summary>
        public void NextScreen()
        {
			ChangeButtonState ();
			if (currentScreen < screens - 1) {
				currentScreen++;
				isLerp = true;
				lerpTarget = positions [currentScreen];

				ChangeBulletsInfo (currentScreen);
			} 
        }

		/// <summary>
		/// Function for switching screens with buttons
		/// </summary>
        public void PreviousScreen()
        {
			ChangeButtonState ();
			if (currentScreen > 0) {
				
				currentScreen--;
				isLerp = true;
				lerpTarget = positions [currentScreen];

				ChangeBulletsInfo (currentScreen);
			} 
        }

		/// <summary>
		/// Used to move to next screen
		/// </summary>
        private void NextScreenCommand()
        {
            if (currentScreen < screens - 1)
            {
                isLerp = true;
                lerpTarget = positions[currentScreen + 1];

                ChangeBulletsInfo(currentScreen + 1);
            }
        }

		/// <summary>
		/// Used to move to previous screen
		/// </summary>
        private void PreviousScreenCommand()
        {
            if (currentScreen > 0)
            {
                isLerp = true;
				lerpTarget = positions[currentScreen - 1];

                ChangeBulletsInfo(currentScreen - 1);
            }
        }

		/// <summary>
		/// Finds the closest registered point to the releasing point
		/// </summary>
		/// <returns>The closest from.</returns>
		/// <param name="start">Start.</param>
		/// <param name="positions">Positions.</param>
        private Vector3 FindClosestFrom(Vector3 start, System.Collections.Generic.List<Vector3> positions)
        {
            Vector3 closest = Vector3.zero;
            float distance = Mathf.Infinity;

            foreach (Vector3 position in positions)
            {
                if (Vector3.Distance(start, position) < distance)
                {
                    distance = Vector3.Distance(start, position);
                    closest = position;
                }
            }

            return closest;
        }

		/// <summary>
		/// Used to keep track of screens 
		/// </summary>
		/// <returns>The screen.</returns>
        public int CurrentScreen()
        {
            var pos = FindClosestFrom(screensContainer.localPosition, positions);
            return currentScreen = GetPageforPosition(pos);
        }

		/// <summary>
		/// Changes the bullets on the bottom of the page - pagination
		/// </summary>
		/// <param name="currentScreen">Current screen.</param>
        private void ChangeBulletsInfo(int currentScreen)
        {
			if (Pagination) {
				for (int i = 0; i < Pagination.transform.childCount; i++) 
				{
					Pagination.transform.GetChild (i).GetComponent<Toggle> ().isOn = (currentScreen == i) ? true : false;
				}
			}
        }

		/// <summary>
		/// Used for changing between screen resolutions
		/// </summary>
        private void DistributePages()
        {
			int offset = 0;
            int dimension = 0;
            Vector2 panelDimensions = gameObject.GetComponent<RectTransform>().sizeDelta;
            int currentXPosition = 0;

            for (int i = 0; i < screensContainer.transform.childCount; i++)
            {
                RectTransform child = screensContainer.transform.GetChild(i).gameObject.GetComponent<RectTransform>();
                currentXPosition = offset + i * PageStep;
                child.sizeDelta = new Vector2(panelDimensions.x, panelDimensions.y);
                child.anchoredPosition = new Vector2(currentXPosition, 0f);
            }

			dimension = currentXPosition + offset * -1;

            screensContainer.GetComponent<RectTransform>().offsetMax = new Vector2(dimension, 0f);

            screens = screensContainer.childCount;

            positions = new System.Collections.Generic.List<Vector3>();

            if (screens > 0)
            {
                for (float i = 0; i < screens; ++i)
                {
                    scrollRect.horizontalNormalizedPosition = i / (screens - 1);
                    positions.Add(screensContainer.localPosition);
                }
            }
        }

		/// <summary>
		/// Gets the pagefor position.
		/// </summary>
		/// <returns>The page for position.</returns>
		/// <param name="pos">Position.</param>
        int GetPageforPosition(Vector3 pos)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                if (positions[i] == pos)
                {
                    return i;
                }
            }
            return 0;
        }

		/// <summary>
		/// Swipes to the left or right based on user touch
		/// </summary>
		public void CheckSwipe()
		{
			if(Input.GetMouseButtonDown(0))
			{
				touchDuration = 0;
				//Save began touch 2d point
				firstPressPosition = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
			}
			//Set the touch duration
			touchDuration += Time.deltaTime;

			if(Input.GetMouseButtonUp(0))
			{
				//Raycasting to check player swipe on the scroll panel
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
				if (hit.collider != null) 
				{
					if(!hit.collider.name.Equals(gameObject.name))
					{
						return;
					}
				} 
				else 
				{
					return;
				}

				if (touchDuration > touchDurationMaxLimit) 
				{
					return;
				}

				//Save ended touch 2d point
				secondPressPosition = new Vector2(Input.mousePosition.x,Input.mousePosition.y);

				//Create vector from the two points
				currentSwipe = new Vector2(secondPressPosition.x - firstPressPosition.x, secondPressPosition.y - firstPressPosition.y);

				//Normalize the 2d vector
				currentSwipe.Normalize();

				//Swipe left
				if(currentSwipe.x < 0  && currentSwipe.y > -swipeValue && currentSwipe.y < swipeValue)
				{
					//Call Next panel episode from current panel episode 
					NextScreenCommand(); 
				}
				//Swipe right
				if(currentSwipe.x > 0 && currentSwipe.y > -swipeValue  && currentSwipe.y < swipeValue)
				{
					//Call Previous panel episode from current panel episode 
					PreviousScreenCommand();
				}
			}
		}

    }
}