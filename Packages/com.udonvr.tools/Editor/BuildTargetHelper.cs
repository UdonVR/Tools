using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using VRC.SDKBase.Editor.BuildPipeline;

namespace UdonVR.ToolkitEditor
{
    public class BuildTargetHelper : IVRCSDKBuildRequestedCallback
    {
        public int callbackOrder => -9999;

        private static string _questTag = "Build Target - Android";
        private static string _pcTag = "Build Target - PC";

        public bool OnBuildRequested(VRCSDKRequestedBuildType requestedBuildType)
        {
            return true;
        }
        
        [PostProcessScene(-9999)]
        private static void ProcessScene()
        {
            bool buildPC = false;
            #if UNITY_64
                buildPC = true;
            #endif
                
            
            TagHelper.DoesTagExist(_questTag);
            {
                GameObject[] _QuestObjects = GameObject.FindGameObjectsWithTag(_questTag);
                if (buildPC)
                {
                    LogHelper_Toolkit.Log($"BuildTargetHelper Building for <color=#888888>PC</color>");
                    for (int i = 0; i < _QuestObjects.Length; i++)
                    {
                        Object.DestroyImmediate(_QuestObjects[i]);
                    }
                }
                LogHelper_Toolkit.Log($"BuildTargetHelper Finished on tag: <color=#888888>{_questTag}</color>");
            }
            TagHelper.DoesTagExist(_pcTag);
            {
                GameObject[] _PcObjects = GameObject.FindGameObjectsWithTag(_pcTag);
                if (!buildPC)
                {
                    LogHelper_Toolkit.Log($"BuildTargetHelper Building for <color=#888888>ANDROID</color>");
                    for (int i = 0; i < _PcObjects.Length; i++)
                    {
                        Object.DestroyImmediate(_PcObjects[i]);
                    }
                }
                LogHelper_Toolkit.Log($"BuildTargetHelper Finished on tag: <color=#888888>{_pcTag}</color>");
            }

            MaterialChecker(buildPC);
        }

        [MenuItem("Tools/UdonVR Toolkit/Create BuildHelper Tags")]
        public static void BuildTags()
        {
            TagHelper.AddTag(_questTag);
            TagHelper.AddTag(_pcTag);
        }

        private static void MaterialChecker(bool _buildPc)
        {
            MaterialHelper matHelper = GameObject.FindAnyObjectByType<MaterialHelper>();
            if (matHelper == null)
            {
                LogHelper_Toolkit.Log($"Material Helper not found.");
                return;
            }
            else
            {
                LogHelper_Toolkit.Log($"Material Helper found.");
            }

            Material[] pcMats = matHelper.pcMats;
            Material[] questMats = matHelper.questMats;
            GameObject[] excluded = matHelper.excludedObjects;
            MeshRenderer[] renderers = GameObject.FindObjectsByType<MeshRenderer>(FindObjectsSortMode.None);
            
            LogHelper_Toolkit.Log($"Found <color=#888888>{renderers.Length}</color> renderers");
            
            if (_buildPc)
            {
                for (int j = 0; j < renderers.Length; j++)
                {
                    for (int i = 0; i < pcMats.Length; i++)
                    {
                        for (int k = 0; k < renderers[j].sharedMaterials.Length; k++)
                        {
                            if (excluded.Contains(renderers[j].gameObject))
                            {
                                LogHelper_Toolkit.Log($"Skipping object <color=#888888>{renderers[j].gameObject.name}</color>");
                                continue;
                            }
                            //LogHelper_Toolkit.Log($"checking material <color=#888888>{renderers[j].sharedMaterials[k].name}</color> object <color=#888888>{renderers[j].gameObject.name}</color>");
                            if (renderers[j].sharedMaterials[k] == questMats[i])
                            {
                                LogHelper_Toolkit.Log($"swapping material <color=#888888>{questMats[i].name}</color> to <color=#888888>{pcMats[i].name}</color> on object <color=#888888>{renderers[j].gameObject.name}</color>");
                                renderers[j].sharedMaterials[k] = pcMats[i];
                            }
                        }
                    }
                }
            }
            else
            {
                for (int j = 0; j < renderers.Length; j++)
                {
                    for (int i = 0; i < pcMats.Length; i++)
                    {
                        for (int k = 0; k < renderers[j].sharedMaterials.Length; k++)
                        {
                            if (excluded.Contains(renderers[j].gameObject))
                            {
                                LogHelper_Toolkit.Log($"Skipping object <color=#888888>{renderers[j].gameObject.name}</color>");
                                continue;
                            }

                            if (renderers[j].sharedMaterials[k] == pcMats[i])
                            {
                                LogHelper_Toolkit.Log($"swapping material <color=#888888>{pcMats[i].name}</color> to <color=#888888>{questMats[i].name}</color> on object <color=#888888>{renderers[j].gameObject.name}</color>");
                                renderers[j].sharedMaterials[k] = questMats[i];
                            }
                        }
                    }
                }
            }
        }
        
    }
}