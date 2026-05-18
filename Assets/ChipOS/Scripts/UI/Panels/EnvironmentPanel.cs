using ChipOS.Core;
using TMPro;
using UnityEngine;

namespace ChipOS.UI.Panels
{
    public class EnvironmentPanel : HUDPanel
    {
        [SerializeField] private TMP_Text environmentBodyText;

        public void Render(ContextState state)
        {
            if (environmentBodyText == null || state == null) return;

            environmentBodyText.text =
                $"Mode: {state.Mode}\n" +
                $"Tracking: {state.Tracking}\n" +
                $"Input: {state.Input}\n" +
                $"Network: {state.Network}\n" +
                $"Battery: {state.BatterySummary}\n" +
                $"Weather: {state.WeatherSummary}";
        }
    }
}
