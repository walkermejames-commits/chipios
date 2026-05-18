using System;
using UnityEngine;

namespace ChipOS.UI.Theme
{
    /// <summary>
    /// Centralized visual settings shared across panels.
    /// </summary>
    [Serializable]
    public class ChipOSThemeSettings
    {
        [SerializeField] [Range(0.15f, 1f)] private float panelOpacity = 0.8f;
        [SerializeField] [Range(0.75f, 1.75f)] private float fontScale = 1f;
        [SerializeField] private bool highContrastEnabled;
        [SerializeField] private Color panelColor = new(0.07f, 0.09f, 0.11f, 0.8f);
        [SerializeField] private Color focusedPanelColor = new(0.12f, 0.16f, 0.20f, 0.88f);
        [SerializeField] private Color textColor = new(0.93f, 0.96f, 1f, 1f);
        [SerializeField] private Color accentColor = new(0.33f, 0.90f, 0.96f, 1f);
        [SerializeField] private Color contrastPanelColor = new(0f, 0f, 0f, 0.94f);
        [SerializeField] private Color contrastFocusedPanelColor = new(0.08f, 0.08f, 0.08f, 1f);
        [SerializeField] private Color contrastTextColor = Color.white;

        public float PanelOpacity
        {
            get => panelOpacity;
            set => panelOpacity = Mathf.Clamp(value, 0.15f, 1f);
        }

        public float FontScale
        {
            get => fontScale;
            set => fontScale = Mathf.Clamp(value, 0.75f, 1.75f);
        }

        public bool HighContrastEnabled
        {
            get => highContrastEnabled;
            set => highContrastEnabled = value;
        }

        public Color AccentColor => accentColor;

        public Color GetPanelColor()
        {
            var source = highContrastEnabled ? contrastPanelColor : panelColor;
            source.a = PanelOpacity;
            return source;
        }

        public Color GetFocusedPanelColor()
        {
            var source = highContrastEnabled ? contrastFocusedPanelColor : focusedPanelColor;
            source.a = Mathf.Clamp01(PanelOpacity + 0.08f);
            return source;
        }

        public Color GetTextColor()
        {
            return highContrastEnabled ? contrastTextColor : textColor;
        }

        public void AdjustFontScale(float delta)
        {
            FontScale += delta;
        }

        public void ToggleContrast()
        {
            HighContrastEnabled = !HighContrastEnabled;
        }
    }
}
