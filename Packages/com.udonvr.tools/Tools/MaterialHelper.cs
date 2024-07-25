
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace UdonVR.ToolkitEditor
{

    public class MaterialHelper : UdonSharpBehaviour
    {
        public Material[] pcMats;
        public Material[] questMats;

        public GameObject[] excludedObjects;
    }
}