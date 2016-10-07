using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Poptropica2;

public class StateMachineEditorWindow : EditorWindow {
	[MenuItem("Window/State Machine Editor")]
	public static void OpenWindow()
	{
		EditorWindow.GetWindow<StateMachineEditorWindow>();
	}

	public State rootState;
	bool isSelectingLocationForNewState = false;
	string creatingNewStateName = "";
	DefaultAsset creatingNewStateLocation;
	private static Type[] allConditionTypes;
	void OnGUI()
	{
		if (allConditionTypes == null) allConditionTypes = GetAllConditionTypes();

		GUILayout.BeginHorizontal();
		if (!isSelectingLocationForNewState)
		{
			rootState = EditorGUILayout.ObjectField("Root state", rootState, typeof(State), false) as State;
			if (rootState == null)
			{
				if (GUILayout.Button("Create new"))
				{
					creatingNewStateName = "New state";
					isSelectingLocationForNewState = true;
				}
			}
		}
		else
		{
			GUILayout.Label("Creating new state with name");
			creatingNewStateName = GUILayout.TextField(creatingNewStateName);
			creatingNewStateLocation = EditorGUILayout.ObjectField("Create in folder", creatingNewStateLocation, typeof(DefaultAsset), false) as DefaultAsset;

			GUI.enabled = creatingNewStateLocation != null && creatingNewStateName != "New state";
			if (GUILayout.Button("Create"))
			{
				rootState = CreateNewState(creatingNewStateName, AssetDatabase.GetAssetPath(creatingNewStateLocation));
				
				isSelectingLocationForNewState = false;
			}
			GUI.enabled = true;
			if (GUILayout.Button("X"))
			{
				isSelectingLocationForNewState = false;
			}
		}
		GUILayout.EndHorizontal();

		Rect stateMachineWindow = GUILayoutUtility.GetRect(new GUIContent(""), GUI.skin.label, GUILayout.ExpandHeight(true));

		DrawStatesInEditor(stateMachineWindow, viewOffset, rootState, true);

		if (Event.current.type == EventType.MouseDown)
		{
			draggingState = null;
			foreach (var stateKVP in statesDrawn)
			{
				Rect thisRect = GetRectFor(stateKVP.Key, viewOffset);
				if (thisRect.Contains(Event.current.mousePosition))
				{
					draggingState = stateKVP.Key;
				}
			}
			dragStart = Event.current.mousePosition;
		}
		if (Event.current.type == EventType.MouseDrag)
		{
			if (draggingState != null)
			{
				draggingState.editorWindowPosition += Event.current.mousePosition - dragStart;
				dragStart = Event.current.mousePosition;
			}
			else
			{
				viewOffset += Event.current.mousePosition - dragStart;
				dragStart = Event.current.mousePosition;
			}
		}

		GUILayout.BeginHorizontal();
		GUILayout.Label("Tooltip here");
		GUILayout.EndHorizontal();
	}

	public static State CreateNewState(string creatingNewStateName, string creatingNewStatePath)
	{
		GameObject newGO = new GameObject(creatingNewStateName);
		State newState = newGO.AddComponent<State>();
		newState.stateName = creatingNewStateName;
		FileAttributes attr = File.GetAttributes(Application.dataPath+creatingNewStatePath.Substring(6) );

		//detect whether its a directory or file
		if ((attr & FileAttributes.Directory) != FileAttributes.Directory)
			creatingNewStatePath = creatingNewStatePath.Substring(0, creatingNewStatePath.LastIndexOf("/"));

		GameObject prefabGO = PrefabUtility.CreatePrefab(creatingNewStatePath + "/" + creatingNewStateName + ".prefab", newGO);
		DestroyImmediate(newGO);
		return prefabGO.GetComponent<State>();
	}

	private State draggingState;
	private Vector2 dragStart;
	private Vector2 viewOffset = Vector2.zero;

	public static void DrawStatesInEditor(Rect guiRect, Vector2 offset, State drawingState, bool drawConnected)
	{
		statesDrawn = new Dictionary<State, bool>();
		stateLinksToDraw = new List<LinkDrawingData>();
		if (actionViewsExpanded == null) actionViewsExpanded = new Dictionary<State, bool>();

		DrawStateHelper(guiRect, offset, drawingState, drawConnected);

		foreach (var linkDraw in stateLinksToDraw)
		{
			ShowStateLinkEditor(linkDraw.window, linkDraw.link, linkDraw.stateLinkIndex);
		}

	}

	public void Update()
	{
		Repaint();
	}

	private static List<LinkDrawingData> stateLinksToDraw;
	private static Dictionary<State, bool> statesDrawn;
	private static Dictionary<State, bool> actionViewsExpanded;

	private static void DrawStateHelper(Rect guiRect, Vector2 offset, State drawingState, bool drawConnected)
	{
		if (drawingState == null) return;
		if (!statesDrawn.ContainsKey(drawingState)) statesDrawn.Add(drawingState, false);
		if (!actionViewsExpanded.ContainsKey(drawingState)) actionViewsExpanded.Add(drawingState, false);

		Rect stateRect = GetRectFor(drawingState, offset);
		GUI.Box(stateRect, "");
		List<Vector3> linesToDrawFrom = new List<Vector3>();
		List<Vector3> linesToDrawTo = new List<Vector3>();
		GUILayout.BeginArea(stateRect);
		GUILayout.Label("State");
		GUILayout.Label(drawingState.stateName);
		int removingLinkIndex = -1;
		int insertingLinkIndex = -1;
		for (int l=0;l<drawingState.links.Length;l++)
		{
			StateLink thisLink = drawingState.links[l];
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("-", GUILayout.Width(20f))) {
				removingLinkIndex = l;
			}
			thisLink.linkLabel = GUILayout.TextField(thisLink.linkLabel, GUILayout.Width(60f) );
			thisLink.linkedState = EditorGUILayout.ObjectField(thisLink.linkedState, typeof(State), false, GUILayout.Width(60f)) as State;
			if (thisLink.linkedState == null)
			{
				if (GUILayout.Button("New State"))
				{
					thisLink.linkedState = CreateNewState(drawingState.stateName + "_" + thisLink.linkLabel, AssetDatabase.GetAssetPath(drawingState.gameObject ) );
				}
			}
			GUILayout.FlexibleSpace();
			if (l == drawingState.expandLinkViewIndex) {
				if (GUILayout.Button("<", GUILayout.Width(20f)))
				{
					drawingState.expandLinkViewIndex = -1;
				}
			}
			else
			{

				if (GUILayout.Button(">", GUILayout.Width(20f)) )
				{
					drawingState.expandLinkViewIndex = l;
				}
			}
			Rect thisLinkButtonRect = GUILayoutUtility.GetLastRect();
			Vector2 linkFromPoint = new Vector2(thisLinkButtonRect.xMax, thisLinkButtonRect.center.y);
			if (l == drawingState.expandLinkViewIndex)
			{
				Rect conditionWindow = GetRectFor(thisLink, linkFromPoint + offset + drawingState.editorWindowPosition);
				linkFromPoint = conditionWindow.max - offset - drawingState.editorWindowPosition;
				stateLinksToDraw.Add(new LinkDrawingData(thisLink, conditionWindow, l));
			}
			if (thisLink.linkedState != null)
			{
				linesToDrawFrom.Add(drawingState.editorWindowPosition + linkFromPoint + offset);
				linesToDrawTo.Add(thisLink.linkedState.editorWindowPosition + offset);
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("+") )
		{
			insertingLinkIndex = drawingState.links.Length;
		}
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Actions");
		GUILayout.FlexibleSpace();
		if (actionViewsExpanded[drawingState] == true)
		{
			if (GUILayout.Button("<")) actionViewsExpanded[drawingState] = false;
		}
		else
		{
			if (GUILayout.Button(">")) actionViewsExpanded[drawingState] = true;
		}
		GUILayout.EndHorizontal();

		if (actionViewsExpanded[drawingState])
		{
			SerializedObject serializedObject = new SerializedObject(drawingState);
			SerializedProperty onEnter = serializedObject.FindProperty("onEnterAction");
			EditorGUILayout.PropertyField(onEnter);
			SerializedProperty onUpdate = serializedObject.FindProperty("onUpdateAction");
			EditorGUILayout.PropertyField(onUpdate);
			SerializedProperty onExit = serializedObject.FindProperty("onExitAction");
			EditorGUILayout.PropertyField(onExit);
			if (GUI.changed) serializedObject.ApplyModifiedProperties();
		}

		GUILayout.EndArea();

		if (removingLinkIndex != -1)
		{
			StateLink[] newArray = new StateLink[drawingState.links.Length - 1];
			for (int l=0;l<newArray.Length;l++)
			{
				newArray[l] = drawingState.links[l < removingLinkIndex ? l : l + 1];
			}
			drawingState.links = newArray;
		}
		if (insertingLinkIndex != -1)
		{

			StateLink[] newArray = new StateLink[drawingState.links.Length + 1];
			for (int l = 0; l < drawingState.links.Length; l++)
			{
				newArray[l] = drawingState.links[l < insertingLinkIndex ? l : l+1];
			}
			newArray[insertingLinkIndex] = new StateLink();
			newArray[insertingLinkIndex].linkLabel = "Link" + insertingLinkIndex;
			drawingState.links = newArray;
		}


		for (int l=0;l<linesToDrawFrom.Count;l++)
		{
			Vector3 line1 = linesToDrawFrom[l];
			Vector3 line2 = linesToDrawTo[l];
			Vector3 mid1 = new Vector3(Mathf.Max( 0.5f * (line1.x + line2.x), line1.x + 50f), line1.y);
			Vector3 mid2 = new Vector3(Mathf.Min( 0.5f * (line1.x + line2.x), line2.x - 50f), line2.y);
			Handles.DrawBezier(line1, line2, mid1, mid2, Color.gray, null, 2f);
		}



		if (drawingState.links != null && drawConnected)
		{
			for (int l = 0; l < drawingState.links.Length; l++)
			{
				if (drawingState.links[l] != null && drawingState.links[l].linkedState != null)
				{
					if (!statesDrawn.ContainsKey(drawingState.links[l].linkedState))
					{
						DrawStateHelper(guiRect, offset, drawingState.links[l].linkedState, drawConnected);
					}
				}
			}
		}
	}

	
	private static StateLink contextMenuModifyingStateLink;
	private static int contextMenuInsertingIndex = -1;

	public static void ShowStateLinkEditor (Rect linkRect, StateLink link, int linkIndex)
	{
		link.OnAfterDeserialize();
		GUI.Box(linkRect, "");
		linkRect.Set(linkRect.xMin + 240f, linkRect.yMin + (linkIndex + 3) * (GUI.skin.button.CalcHeight(new GUIContent(""), 20f) - 1f), linkRect.width, linkRect.height); //temporary repositioning to work around weird bug
		GUILayout.BeginArea(linkRect);
		GUILayout.Label("Link");
		for (int c=0;c<link.conditions.Length;c++)
		{
			if (link.conditions[c] == null)
			{
				Debug.Log(link.linkLabel + " condition " + c + " was null");
				continue;
			}
			GUILayout.BeginHorizontal();
			GUILayout.Label(string.Format("{0}. {1}", c, link.conditions[c].GetType().ToString() ) );
			GUILayout.FlexibleSpace();
			if (c == link.expandConditionViewIndex)
			{
				if (GUILayout.Button("<", GUILayout.Width(20f)))
				{
					link.expandConditionViewIndex = -1;
				}
			}
			else
			{

				if (GUILayout.Button(">", GUILayout.Width(20f)))
				{
					link.expandConditionViewIndex = c;
				}
			}
			GUILayout.EndHorizontal();
			if (link.expandConditionViewIndex == c)
			{
				link.conditions[c].ShowInspectorUI();
			}
		}
		if (GUILayout.Button("Add Condition..."))
		{
			contextMenuModifyingStateLink = link;
			contextMenuInsertingIndex = link.conditions.Length;
			GenericMenu servicesMenu = new GenericMenu();
			foreach (var conditionType in allConditionTypes)
			{
				servicesMenu.AddItem(new GUIContent(conditionType.ToString()), false, AddConditionHelper, conditionType);
			}
			servicesMenu.ShowAsContext();
		}
		GUILayout.EndArea();
	}
	private static void AddConditionHelper(object addingType)
	{
		AddConditionHelper(addingType, contextMenuInsertingIndex);

	}
	private static void AddConditionHelper(object addingType, int insertingIndex)
	{
		Type conditionType = addingType as Type;
		IStateTransitionCondition newCondition = Activator.CreateInstance(conditionType) as IStateTransitionCondition;
		IStateTransitionCondition[] newArray = new IStateTransitionCondition[contextMenuModifyingStateLink.conditions.Length + 1];
		for (int c=0;c<contextMenuModifyingStateLink.conditions.Length;c++)
		{
			newArray[c] = contextMenuModifyingStateLink.conditions[ c < insertingIndex ? c : c + 1];
		}
		newArray[insertingIndex] = newCondition;
		contextMenuModifyingStateLink.conditions = newArray;
	}


	public static Type[] GetAllConditionTypes()
	{
		return (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
				from assemblyType in domainAssembly.GetTypes()
				where (typeof(IStateTransitionCondition).IsAssignableFrom(assemblyType) && !assemblyType.IsAbstract)
				select assemblyType).ToArray();
	}

	public static Rect GetRectFor(State thisState, Vector2 offset)
	{
		int actionCount = (actionViewsExpanded[thisState] ? thisState.onEnterAction.GetPersistentEventCount() + thisState.onExitAction.GetPersistentEventCount() + thisState.onUpdateAction.GetPersistentEventCount() + 10 : 0);
		return new Rect(thisState.editorWindowPosition.x + offset.x, 
			thisState.editorWindowPosition.y + offset.y, 
			240f, 
			(4 + thisState.links.Length + actionCount )
			     * (GUI.skin.button.CalcHeight(new GUIContent("blah"), 120f) + 1f) );
	}
	public static Rect GetRectFor(StateLink thisLink, Vector2 ulCorner)
	{
		int totalLines = 2;
		if (thisLink.conditions == null) thisLink.conditions = new IStateTransitionCondition[0]; 
		for (int c= 0;c< thisLink.conditions.Length;c++)
		{
			if (thisLink.conditions[c] != null)
				totalLines += thisLink.expandConditionViewIndex == c? thisLink.conditions[c].GetInspectorLineCount() + 1 : 1;
		}
		return new Rect(ulCorner.x, ulCorner.y, 240f, totalLines * (GUI.skin.button.CalcHeight(new GUIContent("blah"), 120f) + 1f) );
	}

	//helper class for drawing the link interface next to the state window
	public class LinkDrawingData
	{
		public LinkDrawingData(StateLink link, Rect win, int index)
		{
			this.link = link;
			window = win;
			stateLinkIndex = index;
		}
		public StateLink link;
		public Rect window;
		public int stateLinkIndex;
	}
}
