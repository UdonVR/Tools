
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace UdonVR.Toolkit
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class ColliderToggle : UdonSharpBehaviour
    {
        public Collider[] colliders;
        
        public String tagSingle = "ColliderToggle - Single";
        public String tagParent = "ColliderToggle - Parent";

        public bool isOn = false;

        public void _ToggleColliders()
        {
            isOn = !isOn;
            UpdateColliders();
        }

        private void UpdateColliders()
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = isOn;
            }
        }
    }
}