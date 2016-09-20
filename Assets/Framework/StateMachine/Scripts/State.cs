using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;

namespace Poptropica2
{
    public class State : MonoBehaviour
    {
        public string stateName
		{
			get
			{
				return gameObject.name;
			}
			set
			{
				gameObject.name = value;
			}
		}
        public StateLink[] links;
#if UNITY_EDITOR
		public Vector2 editorWindowPosition;
		public int expandLinkViewIndex = -1;
#endif

		void Reset()
        {
            stateName = gameObject.name;
        }

		public UnityEvent onEnterAction;
		public virtual void OnEnterState()
		{
			onEnterAction.Invoke();
			for (int l=0;l<links.Length;l++)
			{
				if (links[l].conditions == null) links[l].conditions = new IStateTransitionCondition[0];
				for (int c=0;c<links[l].conditions.Length;c++)
				{
					links[l].conditions[c].EnterParentState(this);
				}
			}
		}

		public UnityEvent onUpdateAction;
		public virtual void OnStateUpdate()
		{
			onUpdateAction.Invoke();
		}
		public UnityEvent onExitAction;
		public virtual void OnExitState() {
			onExitAction.Invoke();
		}
    }

    [System.Serializable]
    public class StateLink : ISerializationCallbackReceiver
    {
		public string linkLabel = "Link";
        public State linkedState;
        public bool triggerOnlyOnce;

		[System.NonSerialized]
		public IStateTransitionCondition[] conditions;

		[SerializeField]
		private string[] serializedConditions; //format: "ConditionClassName_SerializeReturnValue"
		private static string serializationDelimiter = "_";
		public void OnBeforeSerialize()
		{
//			Debug.Log("OnBeforeSerialize newer()  " + linkLabel);
			if (conditions == null)
			{
				return; 
			}
			if (serializedConditions == null || serializedConditions.Length != conditions.Length) 
				serializedConditions = new string[conditions.Length];
			for (int c=0;c<conditions.Length;c++) {
				if (conditions[c] != null)
				{
					serializedConditions[c] = conditions[c].GetType().ToString() + serializationDelimiter + conditions[c].Serialize();
				}
				else
				{
					if (serializedConditions[c].Length <= 1) serializedConditions[c] = "ERROR";
				}
			}
		}
		public void OnAfterDeserialize()
		{
//			Debug.Log("OnAfterDeserialize newer "+linkLabel+"; serializedConditions has " + (serializedConditions != null ? serializedConditions.Length.ToString() : "NULL"));
			if (serializedConditions != null)
			{
				conditions = new IStateTransitionCondition[serializedConditions.Length];
				for (int c=0;c<serializedConditions.Length;c++)
				{
//					Debug.Log("     Deserializing " + serializedConditions[c]);
					int delimiterIndex = serializedConditions[c].IndexOf(serializationDelimiter);
					if (delimiterIndex > 0)
					{
						string className = serializedConditions[c].Substring(0, delimiterIndex);
						string serializedClass = serializedConditions[c].Substring(delimiterIndex + 1);
						Type conditionType = Type.GetType(className);
//						Debug.Log("     class " + className + " found as "+conditionType.ToString()+"; contents " + serializedClass);
						IStateTransitionCondition newCondition = System.Activator.CreateInstance(conditionType) as IStateTransitionCondition;
						newCondition.Deserialize(serializedClass);
						conditions[c] = newCondition;
					} 
				}
			}
			else
			{
				conditions = new IStateTransitionCondition[0];
			}
		}

#if UNITY_EDITOR
		public int expandConditionViewIndex = -1;
#endif
	}


	public interface IStateTransitionCondition
	{
		void EnterParentState(State state);
		bool Evaluate();
#if UNITY_EDITOR
		void ShowInspectorUI();
		int GetInspectorLineCount();
#endif
		string Serialize();
		void Deserialize(string serializedContents);
	}
}