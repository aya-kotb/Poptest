using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Poptropica2.IslandSystem
{
	/// <summary>
	/// Island information panel.
    /// Panel which shows the detailed information of selected island.
	/// </summary>
    public class IslandInformationPanel : PanelTransition {

        [Header("IslandInformation Variables")]
		public Toggle[] difficultyToggle;
        public Slider progressBarSlider;
		public Text progressText;
		public Text trophyText;

        [SerializeField] private float progressFillTime = 1f;
        [SerializeField] private float lowerPanelOffsets = 120f;

        MapItem item;
        IslandSystemManager islandSystemManager;
        RectTransform selectedIsland;
        Vector3 previousIslandPosition;
        Vector3 previousIslandScale;
        Vector3 lowerPanelPosition;

		// Use this for initialization
		void Start () {
            islandSystemManager = SAMApplication.mainInstance.GetService<IslandSystemManager>();
			ShowPanel ();
		}
		
        /// <summary>
        /// Initalizes the island information.
        /// Insert all the island details into UI 
        /// </summary>
		void InitalizeIslandInformation ()
		{
            MapHandler.IslandDetail islandDetail = islandSystemManager.mapHandler.currentItem.itemInfo as MapHandler.IslandDetail;
			ProgressBarTransition (islandDetail.progress);
			DifficultyTransition (islandDetail.difficulty);
			trophyText.text = islandDetail.trophy.ToString ();
		}

        /// <summary>
        /// Displays the panel with transition effect.
        /// </summary>
		void ShowPanel ()
		{
            GameObject go = Instantiate (islandSystemManager.mapHandler.currentItem.gameObject);
			selectedIsland = go.GetComponent<RectTransform> ();

			previousIslandPosition = selectedIsland.anchoredPosition3D;
			previousIslandScale = selectedIsland.localScale;

			selectedIsland.transform.SetParent (transform, false);
			selectedIsland.SetAsFirstSibling ();

            lowerPanelPosition = childPanel.anchoredPosition3D;
			Vector3 position = lowerPanelPosition;
            position.y -= lowerPanelOffsets;
            childPanel.anchoredPosition3D = position;

            Vector3 targetPosition = new Vector3 (0, 0, selectedIsland.anchoredPosition3D.z);
            canvasGroup.alpha = 0;

            //UI Animation for displaying panel
            MoveTween(selectedIsland, targetPosition, moveTime);
            ScaleTo(selectedIsland, (Vector3.one * 2f));
            AlphaTween(semiTransparentScreen, alpha);
            CanvasGroupTween(canvasGroup);
            MoveTween(childPanel, lowerPanelPosition, moveTime, InitalizeIslandInformation);

		}

        /// <summary>
        /// Triggers the play button event.
        /// </summary>
		public void OnClickPlayButton ()
		{
            islandSystemManager.VisitIsland();
		}

        /// <summary>
        /// Triggers the restart button event.
        /// </summary>
		public void OnClickRestartButton ()
		{
			Debug.Log ("On Click Restart Button.");
        }

        /// <summary>
        /// Triggers the video trailer button event.
        /// </summary>
        public void OnClickVideoTrailerButton ()
        {
            islandSystemManager.islandSystemUI.SwitchUIPanel (IslandSystemUIHandler.PanelState.VideoTrailer);
		}

        /// <summary>
        /// Triggers the close panel button event.
        /// </summary>
		public void OnClickClosePanelButton ()
		{
            //UI Animation for hiding panel
            MoveTween(selectedIsland, previousIslandPosition, moveTime);
            ScaleTo(selectedIsland, previousIslandScale);
            AlphaTween(semiTransparentScreen, 0f);
            CanvasGroupTween(canvasGroup, 0f);
            Vector3 position = lowerPanelPosition;
            position.y -= lowerPanelOffsets;
            MoveTween(childPanel, position, moveTime, OnCompleteTransition);
		}

        #region UI Transition
		
		/// <summary>
		/// Showing transition in progress bar from 0 to ggive percantage.
        /// And showing incremental transcation in percentage text.
		/// </summary>
		/// <param name="percentage">float Island progress percentage.</param>
		void ProgressBarTransition (float percentage)
		{
            if (percentage > 0)
			{
                LeanTween.value (gameObject, 0, percentage, progressFillTime).setOnUpdate (
					(float value) => {
                        progressBarSlider.value = value;
						progressText.text = (Mathf.RoundToInt(value)).ToString () + " %";
					}
				);
			}
			else 
			{
                progressBarSlider.value = percentage;
				progressText.text = (Mathf.RoundToInt (percentage)).ToString () + " %";
			}
		}

		/// <summary>
		/// Shows transition while showing difficulty icon.
		/// </summary>
		/// <param name="difficulty">int Total Island Difficulty.</param>
		void DifficultyTransition (int difficulty)
		{
			float delayTime = 0;
			for (int i = 0; i < difficulty; i++)
			{
				difficultyToggle [i].isOn = true;
				LeanTween.scale (
					difficultyToggle [i].graphic.GetComponent<RectTransform> (),
					Vector3.one,
					progressFillTime).setDelay (delayTime);

				delayTime += 0.1f;
			}
		}

        #endregion UI Transition

        /// <summary>
        /// Callback after completing UI transition effect.
        /// </summary>
		void OnCompleteTransition ()
		{
			Destroy (selectedIsland.gameObject);
			Destroy (gameObject);
		}
	}
}
