using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Poptropica2.QuestUI 
{
	/// <summary>
	/// This class contain reference of quest log prefab
	/// </summary>
	public class QuestLogs : MonoBehaviour
	{
		public Text questMessageValueText; //To display quest log text

		public Sprite completedQuestSprite; //Assign completed quest log sprite

		public Image imageRendererComponent; //Assign image component 
		public Image backgroundImage;  //Assign background image

		public Color completedBackgroundColor; //Quest log background color 

		public Quest questDetail;

		public Quest QuestDetail
		{
			set
			{ 
				questDetail = value;

				questMessageValueText.text = questDetail.description;
			}
		}

		/// <summary>
		/// It'll be called when the quest gets completed.
		/// So it'll update the log prefab with completed image & changes background color
		/// </summary>
		/// <param name="isCompleted">If set to <c>true</c> is completed.</param>
		public void UpdateLog(bool isCompleted)
		{
			questDetail.isCompleted = isCompleted;

			if (questDetail.isCompleted) 
			{
				backgroundImage.color = completedBackgroundColor;
				imageRendererComponent.sprite = completedQuestSprite;
			}
		}
	}
}
