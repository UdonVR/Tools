
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace UdonVR.Toolkit
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class ColliderToggleButton : UdonSharpBehaviour
    {
        public ColliderToggle _colliderToggle;
        public GameObject indicatorObject;

        public void _Toggle()
        {
            _colliderToggle._ToggleColliders();
        }
        
        public void _UpdateIndicator()
        {
            if (indicatorObject != null) indicatorObject.SetActive(_colliderToggle.isOn);
        }

    }
}