using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Poptropica2.QuestUI
{
	/// <summary>
	/// This class contain all the UI component of island panel 
	/// Using this it'll set the values for episode name icon, episode display image, medallion image, episode title, difficulty level and so on
	/// </summary>
	public class QuestReference : MonoBehaviour
	{
		#region UI Component
		//Reference UI Component
		public Text episodeTitleText; //Title text of each episode
		public Text episodeDescription; //Description of episode
		public Text medallionCountText; //Medal count display text

		public Image episodeNameIcon; //Episode icons
		public Image episodeDisplayImage; //Episode display images
		public Image medallionImage; //Medal image

		public Image[] difficultyLevelHoverImage; //Used to switch hover image of difficulty levels

		public Button restartButton; //Episode restart button
		public Button playButton; //Episode play button
		private Button restartPopupOkButton; //Episode yes button

		public Slider progressBar; //Island progress bar

		private float progressBarValue = 0;
		private float speed = 0.5f; //Used to animate ui component

		#endregion

		private int difficultyLevel = 0; //To check what is type of difficulty level

		public GameObject restartPopup; //Popup panel to show restart episode

		#region properties

		/// <summary>
		/// Sets the episode name icon.
		/// </summary>
		/// <value>The episode name icon.</value>
		public Sprite EpisodeNameIcon 
		{ 
			set 
			{ 
				episodeNameIcon.GetComponent<Image> ().sprite = value; 
			} 
		}

		/// <summary>
		/// Sets the episode display image.
		/// </summary>
		/// <value>The episode display image.</value>
		public Sprite EpisodeDisplayImage 
		{
			set
			{ 
				episodeDisplayImage.GetComponent<Image> ().sprite = value;
			} 
		}

		/// <summary>
		/// Sets the medallion image.
		/// </summary>
		/// <value>The medallion image.</value>
		public Sprite MedallionImage 
		{ 
			set 
			{ 
				medallionImage.GetComponent<Image> ().sprite = value; 
			} 
		}
	
		/// <summary>
		/// Sets the episode title text.
		/// </summary>
		/// <value>The episode title text.</value>
		public string EpisodeTitleText
		{ 
			set 
			{ 
				episodeTitleText.text = value;
			} 
		}

		/// <summary>
		/// Sets the episode description text.
		/// </summary>
		/// <value>The episode description text.</value>
		public string EpisodeDescriptionText 
		{ 
			set 
			{ 
				episodeDescription.text = value; 
			} 
		}

		/// <summary>
		/// Sets the medallion count text.
		/// </summary>
		/// <value>The medallion count text.</value>
		public string MedallionCountText 
		{ 
			set 
			{ 
				medallionCountText.text = value; 
			} 
		}

		/// <summary>
		/// Sets the difficulty level.
		/// </summary>
		/// <value>The difficulty level.</value>
		public int DifficultyLevel 
		{ 
			set 
			{ 
				difficultyLevel = value; 
			} 
		}

		/// <summary>
		/// Sets the progress level.
		/// </summary>
		/// <value>The progress level.</value>
		public float ProgressLevel 
		{ 
			set 
			{ 
				progressBarValue = value;
			} 
		}

		#endregion

		void OnEnable ()
		{
			//Adding button event listener 
			restartButton.onClick.AddListener (OnRestartProgressButtonClick);
			playButton.onClick.AddListener (OnPlayButtonClick);
		}

		/// <summary>
		/// This method will assign all the details to this particular episode.
		/// </summary>
		/// <param name="episodeDetails">Episode Details.</param>
		public void ActivateQuestDisplay(Episode episodeDetails) 
		{
			EpisodeTitleText = episodeDetails.episodeTitle;
			EpisodeDescriptionText = episodeDetails.description;
			progressBarValue = episodeDetails.progress;
			DifficultyLevel = episodeDetails.difficultyLevel;
			MedallionCountText = episodeDetails.medallionCount;

			ShowDifficultyLevel (); 
			AnimateProgressBar (progressBar.gameObject); 
			SetMedallion (int.Parse(medallionCountText.text)); 
		}

		/// <summary>
		/// It enables the difficulty level hover sprite and animate it.
		/// </summary>
		void ShowDifficultyLevel ()
		{
			for (int i = 0; i < difficultyLevel; i++) 
			{
				difficultyLevelHoverImage [i].gameObject.SetActive (true);
				LeanTween.scale (difficultyLevelHoverImage [i].gameObject, new Vector3 (1f, 1f, 1f), 0.4f).setEase (LeanTweenType.easeOutBack).setDelay(i * speed);
			}
		}

		/// <summary>
		/// On Restart progress button click event.
		/// </summary>
		void OnRestartProgressButtonClick ()
		{
			// Disable the swipe restriction collider
			restartPopup.GetComponent<RestartProgressPopup> ().swipeCollider.enabled = false;
			//Assigning restart button 
			restartPopupOkButton = restartPopup.GetComponent<RestartProgressPopup> ().OkButton;
			//Remove if any listener attached to this
			restartPopupOkButton.onClick.RemoveListener (OnRestartProgressOkButtonClick);
			//Add event listener to this button
			restartPopupOkButton.onClick.AddListener (OnRestartProgressOkButtonClick);

			Debug.Log ("Restart Button Clicked");
			restartPopup.SetActive (true);
		}

		/// <summary>
		///On Restart progress yes button click event.
		/// </summary>
		void OnRestartProgressOkButtonClick ()
		{
			//Enable swipe collider
			restartPopup.GetComponent<RestartProgressPopup> ().swipeCollider.enabled = true;
			Debug.Log ("Restart Ok Button Clicked");
			restartPopup.SetActive (false);
		}
			

		/// <summary>
		/// On Play button click event.
		/// </summary>
		void OnPlayButtonClick ()
		{
			Debug.Log ("Play Button Clicked");
		}

		/// <summary>
		/// Animates the progress bar.
		/// </summary>
		/// <param name="animaitonObject">Animaiton object.</param>
		void AnimateProgressBar(GameObject animaitonObject)
		{
			LeanTween.value(animaitonObject, progressBar.value, progressBarValue, 1).setOnUpdate( ( float val )=>
				{
				progressBar.value = val;
			});
		}

		/// <summary>
		/// It check the medallion count is greater than zero and set the animation
		/// </summary>
		/// <param name="MedallionCount">Medallion count.</param>
		void SetMedallion(int MedallionCount)
		{
			medallionImage.gameObject.SetActive (true);
			if (MedallionCount > 0) 
			{
				LeanTween.scale (medallionImage.gameObject, new Vector3 (1f, 1f, 1f), 0.4f).setEase (LeanTweenType.easeOutBack);
			} 
			else 
			{
				medallionImage.gameObject.transform.localScale = Vector3.one;
			}
		}
	}
}