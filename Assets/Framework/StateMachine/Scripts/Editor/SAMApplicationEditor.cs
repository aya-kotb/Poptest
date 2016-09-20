using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Poptropica2
{
    [CustomEditor(typeof(SAMApplication))]
    public class SAMApplicationEditor : Editor
    {
        private Type[] allServiceTypes;
        private Vector2 servicesScrollPos = Vector2.zero;
        private Dictionary<string, bool> expandServiceUIs;
        SAMApplication applicationTarget;
        public override void OnInspectorGUI()
        {
            applicationTarget = target as SAMApplication;
            
            if (allServiceTypes == null) allServiceTypes = GetAllServiceTypes();

            applicationTarget.configFile = EditorGUILayout.ObjectField("Config File", applicationTarget.configFile, typeof(TextAsset), false) as TextAsset;

            if (GUILayout.Button("Load Config"))
            {
                //TODO
            }
            GUILayout.Label("Services");

            servicesScrollPos = GUILayout.BeginScrollView(servicesScrollPos, GUILayout.Height(200f) );
            var allServices = applicationTarget.CopyServiceDictionary();
            if (expandServiceUIs == null) expandServiceUIs = new Dictionary<string, bool>();
            float simpleButtonWidth = 20f;
            foreach (var serviceKVP in allServices)
            {
                if (!expandServiceUIs.ContainsKey(serviceKVP.Key)) expandServiceUIs.Add(serviceKVP.Key, false);

                GUILayout.BeginHorizontal();
                if (expandServiceUIs[serviceKVP.Key] == false)
                {
                    if (GUILayout.Button("+", GUILayout.Width(simpleButtonWidth)))
                    {
                        expandServiceUIs[serviceKVP.Key] = true;
                    }

                }
                else
                {
                    if (GUILayout.Button("-", GUILayout.Width(simpleButtonWidth)))
                    {
                        expandServiceUIs[serviceKVP.Key] = false;
                    }
                }
                string newKey = GUILayout.TextField(serviceKVP.Key, GUILayout.Width(120f) );
                if (newKey != serviceKVP.Key)
                {
                    applicationTarget.RenameService(serviceKVP.Key, newKey);
                }
                GUILayout.Label(serviceKVP.Value.GetType().ToString() );

                if (GUILayout.Button("X", GUILayout.Width(simpleButtonWidth) ) ) {
                    applicationTarget.RemoveService(serviceKVP.Key);
                }
                GUILayout.EndHorizontal();

                if (expandServiceUIs[serviceKVP.Key])
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(simpleButtonWidth);
                    GUILayout.BeginVertical();
                    serviceKVP.Value.ShowInspectorUI();
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndScrollView();
            if (GUILayout.Button("Add Service..."))
            {
                GenericMenu servicesMenu = new GenericMenu();
                foreach (var serviceType in allServiceTypes)
                {
                    servicesMenu.AddItem(new GUIContent(serviceType.ToString()), false, AddServiceHelper, serviceType);
                }
                servicesMenu.ShowAsContext();
            }
        }

        public void AddServiceHelper(object service)
        {
            Type serviceType = service as Type;
            var newService = Activator.CreateInstance(serviceType);
            if (applicationTarget == null) applicationTarget = SAMApplication.mainInstance;
            string serviceTypeName = serviceType.ToString();
            if (serviceTypeName.IndexOf(".") >= 0)
            {
                serviceTypeName = serviceTypeName.Substring(serviceTypeName.IndexOf(".")+1);
            }
            applicationTarget.AddService(serviceTypeName, newService as IService);
        }

        public static Type[] GetAllServiceTypes()
        {
           return (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                            from assemblyType in domainAssembly.GetTypes()
                            where (typeof(IService).IsAssignableFrom(assemblyType) && !assemblyType.IsAbstract )
                            select assemblyType).ToArray();
        }
    }

}