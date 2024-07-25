using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UdonVR.ToolkitEditor
{
    public class TagHelper : Editor
    {
        public static void AddTag(string _tag)
        {
            Object[] asset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
            if ((asset != null) && (asset.Length > 0))
            {
                SerializedObject so = new SerializedObject(asset[0]);
                SerializedProperty tags = so.FindProperty("tags");

                for (int i = 0; i < tags.arraySize; ++i)
                {
                    if (tags.GetArrayElementAtIndex(i).stringValue == _tag)
                    {
                        return; // Tag already present, nothing to do.
                    }
                }

                tags.InsertArrayElementAtIndex(tags.arraySize - 1);
                tags.GetArrayElementAtIndex(tags.arraySize - 1).stringValue = _tag;
                so.ApplyModifiedProperties();
                so.Update();
            }
        }

        public static bool DoesTagExist(string _tag)
        {
            Object[] asset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
            if ((asset != null) && (asset.Length > 0))
            {
                SerializedObject so = new SerializedObject(asset[0]);
                SerializedProperty tags = so.FindProperty("tags");

                for (int i = 0; i < tags.arraySize; ++i)
                {
                    if (tags.GetArrayElementAtIndex(i).stringValue == _tag)
                    {
                        return true;
                    }
                }
            }
            
            
            return false;
        }
    }
}