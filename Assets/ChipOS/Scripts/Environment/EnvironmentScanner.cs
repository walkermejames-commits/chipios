using System;
using UnityEngine;

namespace ChipOS.Environment
{
    [Serializable]
    public struct EnvironmentSnapshot
    {
        public bool DeskDetected;
        public bool WallDetected;
        public string UserPosture;
        public float RoomBrightness;
        public Vector3 AudioDirection;
        public string ObjectMemory;
    }

    public class EnvironmentScanner : MonoBehaviour
    {
        [SerializeField] private float simulationSpeed = 0.3f;

        public EnvironmentSnapshot CurrentSnapshot { get; private set; }

        private void Update()
        {
            var t = Time.time * simulationSpeed;
            CurrentSnapshot = new EnvironmentSnapshot
            {
                DeskDetected = Mathf.Sin(t) > -0.5f,
                WallDetected = Mathf.Cos(t * 0.8f) > -0.25f,
                UserPosture = Mathf.Sin(t * 0.4f) > 0f ? "Upright" : "Lean",
                RoomBrightness = Mathf.Clamp01(0.5f + Mathf.Sin(t * 0.6f) * 0.4f),
                AudioDirection = new Vector3(Mathf.Cos(t), 0f, Mathf.Sin(t)).normalized,
                ObjectMemory = "Mug, Keyboard, Notebook"
            };
        }
    }
}
