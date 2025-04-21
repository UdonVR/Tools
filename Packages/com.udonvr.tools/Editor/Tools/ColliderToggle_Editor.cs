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
            UdonVR.Toolkit.ColliderToggleButton[] _buttons = Resources.FindObjectsOfTypeAll<UdonVR.Toolkit.ColliderToggleButton>();
            if (_scripts == null) return;
            if (_scripts.Length == 0) return;
            
            for (int i = 0; i < _scripts.Length; i++)
            {
                _scripts[i].isOn = _scripts[i].defaultState_Vr;
                _ColliderSetup(_scripts[i]);
            }

            for (int i = 0; i < _buttons.Length; i++)
            {
                for (int j = 0; j < _scripts.Length; j++)
                {
                    if (_buttons[i]._colliderToggle == _scripts[j])
                    {
                        LogHelper_Toolkit.Log($"Found <color=#888888>{_buttons[i].name}</color> binding to controller <color=#888888>{_scripts[j].name}</color>");
                        Array.Resize(ref _scripts[j].buttons, _scripts[j].buttons.Length + 1);
                        _scripts[j].buttons[^1] = _buttons[i];
                        if (_buttons[i].indicatorObject != null) _buttons[i].indicatorObject.SetActive(_scripts[j].isOn);
                        break;
                    }
                }
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
        private SerializedProperty _defaultState_Vr;
        private SerializedProperty _defaultState_Pc;

        private static string _defaultSingle = "ColliderToggle - Single";
        private static string _defaultParent = "ColliderToggle - Parent";

        private bool _displayToggles = false;
        private ColliderToggle _script;
        private bool[] _parentDrawers;
        private GameObject[] __tagSingle;
        private GameObject[] __tagParent;
        private Collider[][] _parentColliders;
        
        private bool onEnable = false;
        void OnEnable()
        {
            _script = (ColliderToggle)target;
            UpdateTagList();
        }

        void UpdateTagList()
        {
            __tagSingle = GameObject.FindGameObjectsWithTag(_script.tagSingle);
            __tagParent = GameObject.FindGameObjectsWithTag(_script.tagParent);
            _parentDrawers = new bool[__tagParent.Length];
            _parentColliders = new Collider[__tagParent.Length][];
            for (int i = 0; i < _parentColliders.Length; i++)
            {
                _parentColliders[i] = __tagParent[i].GetComponentsInChildren<Collider>();
            }
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField(onEnable.ToString());
            _tagSingle = serializedObject.FindProperty("tagSingle");
            _tagParent = serializedObject.FindProperty("tagParent");
            _isOn = serializedObject.FindProperty("isOn");
            _defaultState_Vr = serializedObject.FindProperty("defaultState_Vr");
            _defaultState_Pc = serializedObject.FindProperty("defaultState_Pc");

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

                using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                {
                    EditorGUILayout.LabelField(new GUIContent("Default State", "Determines the Default state of the toggle.\n\nThe scene will build using the default state for VR.\nIf the user is NOT in VR, the \"No VR\" default state will load during Runtime when the user first enters the world."), GUILayout.MinWidth(0f));
                    using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                    {
                        EditorGUILayout.PropertyField(_defaultState_Pc, new GUIContent("No VR", "The default state of the toggle if the user is NOT in VR."));
                        EditorGUILayout.PropertyField(_defaultState_Vr, new GUIContent("VR", "The default state of the toggle if the user is in VR."));
                    }
                }

                //EditorGUILayout.PropertyField(_isOn, new GUIContent("Default State"));

            }

            DisplayToggles();
            
            serializedObject.ApplyModifiedProperties();
        }

        private void DisplayToggles()
        {
            int __parentColliderCount = 0;
            for (int i = 0; i < _parentColliders.Length; i++)
            {
                __parentColliderCount += _parentColliders[i].Length;
            }
            
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.LabelField(new GUIContent($"Total found colliders: [{__parentColliderCount + __tagSingle.Length}]", ""));
                if (GUILayout.Button(_displayToggles ? "Hide Colliders" : "Show Colliders"))
                {
                    _displayToggles = !_displayToggles;
                }

                if (_displayToggles)
                {
                    
                    using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                    {
                        EditorGUILayout.LabelField(new GUIContent($"Single Toggles [{__tagSingle.Length}]", ""), GUILayout.MinWidth(0f));
                        for (int i = 0; i < __tagSingle.Length; i++)
                        {
                            EditorGUI.BeginDisabledGroup(true);
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                Collider collider;
                                if (__tagSingle[i].TryGetComponent<Collider>(out collider))
                                {
                                    //EditorGUILayout.ObjectField(__tagSingle[i].gameObject, typeof(GameObject), true);
                                    EditorGUILayout.ObjectField(collider, typeof(Collider), true, GUILayout.MinWidth(0f));
                                }
                            }
                            EditorGUI.EndDisabledGroup();
                        }
                    }

                    using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                    {

                        EditorGUILayout.LabelField($"Parent Toggles [{__parentColliderCount}]", GUILayout.MinWidth(0f));

                        for (int i = 0; i < _parentColliders.Length; i++)
                        {
                            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                            {
                                using (new EditorGUILayout.HorizontalScope())
                                {
                                    EditorGUI.BeginDisabledGroup(true);
                                    EditorGUILayout.ObjectField(__tagParent[i], typeof(GameObject), true);
                                    EditorGUI.EndDisabledGroup();
                                    EditorGUILayout.LabelField($"{_parentColliders[i].Length}", GUILayout.MinWidth(0f), GUILayout.MaxWidth(100f));
                                    if (GUILayout.Button(_parentDrawers[i] ? "Close" : "Open", GUILayout.Width(100f)))
                                    {
                                        _parentDrawers[i] = !_parentDrawers[i];
                                    }
                                }

                                if (_parentDrawers[i])
                                {
                                    using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
                                    {
                                        for (int j = 0; j < _parentColliders[i].Length; j++)
                                        {
                                            EditorGUI.BeginDisabledGroup(true);
                                            EditorGUILayout.ObjectField(_parentColliders[i][j], typeof(Collider), true);
                                            EditorGUI.EndDisabledGroup();
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }
        }
    }
}