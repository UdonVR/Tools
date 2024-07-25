using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using VRC.SDKBase.Editor.BuildPipeline;
using UdonVR.Toolkit;

namespace UdonVR.ToolkitEditor
{
    public class ColliderToggle_Editor : IVRCSDKBuildRequestedCallback
    {
        public int callbackOrder => -100;

        public bool OnBuildRequested(VRCSDKRequestedBuildType requestedBuildType)
        {
            return true;
        }
        
        [PostProcessScene(-100)]
        private static void ProcessScene()
        {
            UdonVR.Toolkit.ColliderToggle[] _scripts = Resources.FindObjectsOfTypeAll<UdonVR.Toolkit.ColliderToggle>();
            if (_scripts == null) return;
            if (_scripts.Length == 0) return;
            for (int i = 0; i < _scripts.Length; i++)
            {
                _ColliderSetup(_scripts[i]);
            }
        }


        private static void _ColliderSetup(ColliderToggle _script)
        {
            
            Collider _tmp;

            List<Collider> _colliders = new List<Collider>();
            
            GameObject[] _tmpObj = GameObject.FindGameObjectsWithTag(_script.tagSingle);
            LogHelper_Toolkit.Log($"Found <color=#888888>{_tmpObj.Length}</color> with tag <color=#888888>{_script.tagSingle}</color>");
            for (int i = 0; i < _tmpObj.Length; i++)
            {
                if (_tmpObj[i].TryGetComponent<Collider>(out _tmp))
                {
                    _colliders.Add(_tmp);
                }
            }
            
            _tmpObj = GameObject.FindGameObjectsWithTag(_script.tagParent);
            LogHelper_Toolkit.Log($"Found <color=#888888>{_tmpObj.Length}</color> with tag <color=#888888>{_script.tagParent}</color>");
            Collider[] _tmps;
            for (int i = 0; i < _tmpObj.Length; i++)
            {
                _tmps = _tmpObj[i].GetComponentsInChildren<Collider>();
                for (int j = 0; j < _tmps.Length; j++)
                {
                    //Debug.Log($"Checking object <color=#888888>{_tmps[j].name}</color>");
                    _colliders.Add(_tmps[j]);
                }
            }
            _script.colliders = Array.Empty<Collider>();
            _script.colliders = _colliders.ToArray();
            LogHelper_Toolkit.Log($"Built Collider Array on <color=#888888>{_script.name}</color> with amount of <color=#888888>{_script.colliders.Length}</color>");

            for (int i = 0; i < _script.colliders.Length; i++)
            {
                _script.colliders[i].enabled = _script.isOn;
            }
        }
    }

    [CustomEditor(typeof(ColliderToggle))]
    public class ColliderToggle_EditorInspector : Editor
    {
        
        private SerializedProperty _tagSingle;
        private SerializedProperty _tagParent;
        private SerializedProperty _isOn;

        private static string _defaultSingle = "ColliderToggle - Single";
        private static string _defaultParent = "ColliderToggle - Parent";
        public override void OnInspectorGUI()
        {
            _tagSingle = serializedObject.FindProperty("tagSingle");
            _tagParent = serializedObject.FindProperty("tagParent");
            _isOn = serializedObject.FindProperty("isOn");

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
                    {
                        EditorGUILayout.PropertyField(_tagSingle, new GUIContent(_tagSingle.displayName));
                        if (_tagSingle.stringValue != _defaultSingle)
                        {
                            if (GUILayout.Button("Reset", GUILayout.MaxWidth(100f)))
                            {
                                _tagSingle.stringValue = _defaultSingle;
                            }
                        }

                        if (!UdonVR.ToolkitEditor.TagHelper.DoesTagExist(_tagSingle.stringValue))
                        {
                            if (GUILayout.Button("Create Tag", GUILayout.MaxWidth(100f)))
                            {
                                UdonVR.ToolkitEditor.TagHelper.AddTag(_tagSingle.stringValue);
                            }
                        }
                    }

                    using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
                    {
                        EditorGUILayout.PropertyField(_tagParent, new GUIContent(_tagParent.displayName));
                        if (_tagParent.stringValue != _defaultParent)
                        {
                            if (GUILayout.Button("Reset", GUILayout.MaxWidth(100f)))
                            {
                                _tagParent.stringValue = _defaultParent;
                            }
                        }

                        if (!UdonVR.ToolkitEditor.TagHelper.DoesTagExist(_tagParent.stringValue))
                        {
                            if (GUILayout.Button("Create Tag", GUILayout.MaxWidth(100f)))
                            {
                                UdonVR.ToolkitEditor.TagHelper.AddTag(_tagParent.stringValue);
                            }
                        }
                    }
                    
                }

                EditorGUILayout.PropertyField(_isOn, new GUIContent("Default State"));
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}