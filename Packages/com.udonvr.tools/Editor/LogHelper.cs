using System.Collections;
using System.Collections.Generic;
using Serilog;
using UnityEditor;
using UnityEngine;

namespace UdonVR.ToolkitEditor
{
    public class LogHelper : Editor
    {
        public static string _udonVr = "<color=#111111>[UdonVR]</color>";
        public static void Log(string _log) { Debug.Log($"{_udonVr} {_log}"); }

        public static void LogWarning(string _log) { Debug.LogWarning($"{_udonVr} {_log}"); }

        public static void LogError(string _log) { Debug.LogError($"{_udonVr} {_log}"); }
    }
    public class LogHelper_Toolkit : Editor
    {
        private static string _prefix = "<color=#7979d1>[Toolkit]</color>";
        public static void Log(string _log) { Debug.Log($"{LogHelper._udonVr}{_prefix} {_log}"); }
        public static void LogWarning(string _log) { Debug.LogWarning($"{LogHelper._udonVr}{_prefix} {_log}"); }
        public static void LogError(string _log) { Debug.LogError($"{LogHelper._udonVr}{_prefix} {_log}"); }
    }
}