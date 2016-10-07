using System.Collections.Generic;

namespace Poptropica2.QuestSystem
{

	/// <summary>
	/// Data structure for storing Quest data.
	/// </summary>
	[System.Serializable]
	public class QuestData
	{
		public string questID;
		public string name;
		public string description;
		public bool isActive;
		public List<StageData> stages;
		public List<string> rewards;
		public bool isComplete;
	}

	/// <summary>
	/// Data structure for storing stage Data
	/// </summary>
	[System.Serializable]
	public class StageData
	{
		public string stageID;
		public string nextStageID;
		public string name;
		public bool isActive;
		public List<StepData> steps;
		public List<string> rewards;
		public bool isComplete;
		public bool isInitial;
	}

	/// <summary>
	/// Data structure for storing step condition for each stage
	/// </summary>
	[System.Serializable]
	public class StepData
	{
		public string stepID;
		public string stepType;
		public bool mandatory;
		public bool isComplete;
	}

	[System.Serializable]
	public class PickUpItemStepData : StepData
	{
		public string objectID;
		public int count;
	}

	[System.Serializable]
	public class ObjectiveStepData : StepData
	{
		public string objectiveID;
	}
}