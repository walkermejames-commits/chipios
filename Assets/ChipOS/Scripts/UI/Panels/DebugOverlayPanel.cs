using ChipOS.Core;
using TMPro;
using UnityEngine;

namespace ChipOS.UI.Panels
{
    /// <summary>
    /// Lightweight runtime overlay for field diagnostics while the interaction stack is still evolving.
    /// </summary>
    public class DebugOverlayPanel : HUDPanel
    {
        [SerializeField] private TMP_Text debugText;

        public void Render(ContextState state, string serviceSummary)
        {
            if (debugText == null || state == null)
            {
                return;
            }

            debugText.text =
                $"Focused: {state.FocusedPanelId}\n" +
                $"Gaze Follow: {(state.GazeFollowEnabled ? "On" : "Off")}\n" +
                $"Theme: Opacity {state.PanelOpacity:F2} | Font {state.FontScale:F2} | Contrast {(state.HighContrastEnabled ? "High" : "Default")}\n" +
                $"Input: {state.Input}\n" +
                $"Time: {state.CurrentTimeText} {state.CurrentDateText}\n" +
                $"{serviceSummary}";
        }
    }
}
