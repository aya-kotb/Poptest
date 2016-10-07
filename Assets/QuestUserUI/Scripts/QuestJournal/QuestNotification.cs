using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Poptropica2.QuestUI 
{
	/// <summary>
	/// Used to show the quest notification.
	/// And upon clicking the notification it'll get close.
	/// </summary>
	public class QuestNotification : MonoBehaviour 
	{
		public Text notificationText; //Assign notification Text

		public Image notificationIcon; //Assign notification icon image component

		public Sprite questIcon; //Assign find quest icon
		public Sprite completedQuestIcon; //Assign completed quest icon

		private float speed = 0.4f; //Notification bar movement speed

		/// <summary>
		/// Sets the notification text log.
		/// </summary>
		/// <value>The notification text.</value>
		public string NotificationText
		{
			set
			{ 
				notificationText.text = value;
			}
		}

		/// <summary>
		/// Sets the notification icon
		/// </summary>
		/// <value>The notification icon.</value>
		public Sprite NotificationIcon
		{
			set 
			{
				notificationIcon.sprite = value;
			}
		}

		/// <summary>
		/// Awake this instance.
		/// </summary>
		void Awake()
		{
			//Set scale value in x axis so that transition should happen  
			transform.localScale = new Vector3 (0f, 1f, 1f);
		}

		/// <summary>
		/// Used for transition on enable  
		/// </summary>
		public void OnEnable()
		{
			LeanTween.scale (gameObject, new Vector3 (1f, 1f, 1f), speed).setEase (LeanTweenType.easeOutSine);
		}

		/// <summary>
		/// Used once the player click on notification panel.
		/// </summary>
		public void CloseNotification()
		{
			LeanTween.scale (gameObject, new Vector3 (0f, 1f, 1f), speed).setEase (LeanTweenType.easeOutSine).setOnComplete(DeactiveNotification);
		}

		/// <summary>
		/// Deactives the notification panel.
		/// </summary>
		public void DeactiveNotification()
		{
			gameObject.SetActive (false);
		}
	}
}
