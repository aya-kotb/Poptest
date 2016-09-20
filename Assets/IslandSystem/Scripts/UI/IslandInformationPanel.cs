using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Poptropica2.IslandSystem
{
	/// <summary>
	/// Island information panel. A panel which shows the detail information
	/// of selected island.
	/// </summary>
	public class IslandInformationPanel : MonoBehaviour {

		public Button playButton;
		public Button restartButton;
		public Button videoTrailerButton;
		public Button closeButton;
		public Toggle[] difficultyToggle;
		public Image progressBarImage;
		public Text progressText;
		public Text trophyText;

		public RectTransform lowerPanel;
		public RectTransform semiBlackScreen;

		public float transactionTime = 1f;

		RectTransform selectedIsland;
		Vector3 previousIslandPosition;
		Vector3 previousIslandScale;
		Vector3 lowerPanelPosition;

		MapItem item;

		// Use this for initialization
		void Start () {
			AssignButtonListener ();
			ShowPanel ();
		}
		
		public void InitalizeIslandInformation ()
		{
			MapHandler.IslandDetail islandDetail = IslandSystemManager.Instance.mapHandler.currentItem.itemDetail as MapHandler.IslandDetail;
			ProgressBarTransaction (islandDetail.progress);
			DifficultyTransaction (islandDetail.difficulty);
			trophyText.text = islandDetail.trophy.ToString ();
		}

		void ShowPanel ()
		{
			GameObject go = Instantiate (IslandSystemManager.Instance.mapHandler.currentItem.gameObject);
			selectedIsland = go.GetComponent<RectTransform> ();

			previousIslandPosition = selectedIsland.anchoredPosition3D;
			previousIslandScale = selectedIsland.localScale;

			selectedIsland.transform.SetParent (transform, false);
			selectedIsland.SetAsFirstSibling ();

			lowerPanelPosition = lowerPanel.anchoredPosition3D;
			Vector3 position = lowerPanelPosition;
			position.y -= 120f;
			lowerPanel.anchoredPosition3D = position;

			ShowTransaction ();
		}

		#region Button Click Events

		void AssignButtonListener ()
		{
			playButton.onClick.AddListener (OnClickPlayButton);
			restartButton.onClick.AddListener (OnClickRestartButton);
			videoTrailerButton.onClick.AddListener (OnClickVideoTrailerButton);
			closeButton.onClick.AddListener (OnClickClosePanelButton);
		}

		void OnClickPlayButton ()
		{
			Debug.Log ("On Click Play Button.");
		}

		void OnClickRestartButton ()
		{
			Debug.Log ("On Click Restart Button.");
		}

		void OnClickVideoTrailerButton ()
		{
			Debug.Log ("On Click Video railer Button.");
		}

		void OnClickClosePanelButton ()
		{
			Debug.Log ("On Click Close Panel Button");

			HideTransaction ();
		}

		#endregion

		#region UI Transaction

		/// <summary>
		/// Show the Information Panel with transaction effect.
		/// </summary>
		void ShowTransaction ()
		{
			Vector3 targetPosition = new Vector3 (0, 0, selectedIsland.anchoredPosition3D.z);

			LeanTween.move (selectedIsland, targetPosition, transactionTime);
			LeanTween.scale (selectedIsland, new Vector3 (2f, 2f, 1f), transactionTime);
			LeanTween.alpha (semiBlackScreen, 1f, transactionTime);
			LeanTween.alphaCanvas (lowerPanel.GetComponent<CanvasGroup> (), 1f, transactionTime);
			LeanTween.move (lowerPanel, lowerPanelPosition, transactionTime).setOnComplete(InitalizeIslandInformation);
		}

		/// <summary>
		/// Hide the Information Panel with transaction effect.
		/// </summary>
		void HideTransaction ()
		{
			LeanTween.move (selectedIsland, previousIslandPosition, transactionTime);
			LeanTween.scale (selectedIsland, previousIslandScale, transactionTime);
			LeanTween.alpha (semiBlackScreen, 0f, transactionTime);
			LeanTween.alphaCanvas (lowerPanel.GetComponent<CanvasGroup> (), 0f, transactionTime);

			Vector3 position = lowerPanelPosition;
			position.y -= 120f;
			LeanTween.move (lowerPanel, position, transactionTime).setOnComplete(OnCompleteTransaction);
		}
		/// <summary>
		/// Giving transaction in progress bar.
		/// And increasing transcation in percentage text
		/// </summary>
		/// <param name="percentage">Island progress percentage.</param>
		void ProgressBarTransaction (float percentage)
		{
			float fillAmount = percentage / 100f;
			if (fillAmount > 0)
			{
				LeanTween.value (gameObject, 0, fillAmount, (transactionTime * 2f)).setOnUpdate (
					(float value) => {
						progressBarImage.fillAmount = value;
						progressText.text = (Mathf.RoundToInt(value * 100f)).ToString () + " %";
					}
				);
			}
			else 
			{
				progressBarImage.fillAmount = fillAmount;
				progressText.text = (Mathf.RoundToInt (percentage)).ToString () + " %";
			}
		}

		/// <summary>
		/// Showing transaction while showing difficulty icon.
		/// </summary>
		/// <param name="difficulty">Total Island Difficulty.</param>
		void DifficultyTransaction (int difficulty)
		{
			float delayTime = 0;
			for (int i = 0; i < difficulty; i++)
			{
				difficultyToggle [i].isOn = true;
				LeanTween.scale (
					difficultyToggle [i].graphic.GetComponent<RectTransform> (),
					Vector3.one,
					transactionTime).setDelay (delayTime);

				delayTime += 0.1f;
			}
		}

		#endregion UI Transaction

		void OnCompleteTransaction ()
		{
			Destroy (selectedIsland.gameObject);
			Destroy (gameObject);
		}
	}
}
