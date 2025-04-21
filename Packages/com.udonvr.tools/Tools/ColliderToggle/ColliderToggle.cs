
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
        public ColliderToggleButton[] buttons;
        public String tagSingle = "ColliderToggle - Single";
        public String tagParent = "ColliderToggle - Parent";

        public bool defaultState_Vr = false;
        public bool defaultState_Pc = true;

        public bool isOn = false;

        private void Start()
        {
            if (Networking.LocalPlayer.IsUserInVR()) return;
            isOn = defaultState_Pc;
            UpdateColliders();
        }

        public void _ToggleColliders()
        {
            isOn = !isOn;
            UpdateColliders();
            UpdateButtons();
        }

        private void UpdateColliders()
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = isOn;
            }
        }

        private void UpdateButtons()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i]._UpdateIndicator();
            }
        }
    }
}