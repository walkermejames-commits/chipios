using System;
using UnityEngine;

namespace ChipOS.Animations
{
    /// <summary>
    /// Small animation profile shared by world-space panels.
    /// </summary>
    [Serializable]
    public class PanelAnimationProfile
    {
        [Range(1f, 20f)] public float VisibilityLerpSpeed = 8f;
        [Range(1f, 20f)] public float LayoutLerpSpeed = 8f;
        [Range(0f, 0.05f)] public float FloatingAmplitude = 0.01f;
        [Range(0f, 3f)] public float FloatingFrequency = 0.65f;
        [Range(1f, 12f)] public float GazeFollowLerpSpeed = 4f;
    }
}
