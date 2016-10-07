using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.Linq;
using System.IO;

namespace Poptropica2
{
	/// <summary>
	/// This displays all the services' custom editors.
	/// </summary>
	public static class ServiceEditorList
	{
		public static Type[] allServiceTypes;

		/// <summary>
		/// Gets all service types by searching all loaded assemblies for any classes that are IServices.
		/// </summary>
		/// <returns>All classes that derrive from IService.</returns>
		public static Type[] GetAllServiceTypes()
		{
			return (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
				from assemblyType in domainAssembly.GetTypes()
				where (typeof(IService).IsAssignableFrom(assemblyType) && !assemblyType.IsAbstract )
				select assemblyType).ToArray();
		}

		/// <summary>
		/// Show the specified serialized service list on the serialized object (game configuration).
		/// </summary>
		/// <param name="list">List of services as a SerializedProperty.</param>
		/// <param name="sobj">The serialized object i.e. game configuration.</param>
		public static bool Show(SerializedProperty list, SerializedObject sobj)
		{
			bool modified = false;
			if(allServiceTypes == null) allServiceTypes = GetAllServiceTypes();
			int toRemove = -1;
			for(int x = 0; x < list.arraySize; ++x)
			{
				SerializedProperty prop = list.GetArrayElementAtIndex(x);
				var rect = EditorGUILayout.BeginVertical();
				EditorGUILayout.Space();
				EditorGUILayout.Space();
				if(prop.objectReferenceValue as ServiceConfiguration != null)
					(prop.objectReferenceValue as ServiceConfiguration).ShowEditorUI();
				EditorGUILayout.Space();
				if (GUILayout.Button("Remove Service"))
				{
					toRemove = x;
				}
				EditorGUILayout.Space();
				EditorGUILayout.EndVertical();
				if(prop.objectReferenceValue as ServiceConfiguration != null)
					GUI.Box(rect, (prop.objectReferenceValue as ServiceConfiguration).GetType().Name.Replace("Config", ""));
			}
			if(toRemove >= 0)
			{
				modified = true;
				int oldSize = list.arraySize;
				AssetDatabase.DeleteAsset(AssetDatabase.GetAssetOrScenePath(list.GetArrayElementAtIndex(toRemove).objectReferenceValue));
				list.DeleteArrayElementAtIndex(toRemove);
				if(oldSize == list.arraySize)
					list.DeleteArrayElementAtIndex(toRemove);
			}
			if (GUILayout.Button("Add Service"))
			{
				GenericMenu servicesMenu = new GenericMenu();
				modified = true;
				foreach (var serviceType in allServiceTypes)
				{
					servicesMenu.AddItem(new GUIContent(serviceType.ToString()), false, delegate(object service)
					{
							Type styp = service as Type;
							foreach(var assy in AppDomain.CurrentDomain.GetAssemblies())
							{
								foreach(var typ in assy.GetTypes())
								{
									if(typ.Name == styp.Name + "Config" || typ.Name == styp.Name + "Configuration")
									{
										list.InsertArrayElementAtIndex(0);
										sobj.ApplyModifiedProperties();
										object thing = ScriptableObject.CreateInstance(typ);
										if(thing == null || (thing as ServiceConfiguration) == null) return;
										sobj.ApplyModifiedProperties();
										string path = AssetDatabase.GetAssetOrScenePath(sobj.targetObject);
										path = Path.GetDirectoryName(path) + Path.DirectorySeparatorChar + styp.Name + ".asset";
										AssetDatabase.CreateAsset(thing as ServiceConfiguration, path);
										(sobj.targetObject as GameConfiguration).services[0] = thing as ServiceConfiguration; 
										return;
									}
								}
							}

						}, serviceType);
				}
				servicesMenu.ShowAsContext();
			}
			return modified;
		}

	}

	/// <summary>
	/// Custom Game configuration editor. This displays the add/remove service buttons along with the editor for each service.
	/// </summary>
	[CustomEditor(typeof(GameConfiguration))]
	public class GameConfigurationEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			ServiceEditorList.Show(serializedObject.FindProperty("services"), serializedObject);
			serializedObject.ApplyModifiedProperties();

			EditorUtility.SetDirty(target);
		}
	}
}