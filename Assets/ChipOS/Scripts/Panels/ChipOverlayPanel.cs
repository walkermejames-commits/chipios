using System;
using ChipOS.Core;
using TMPro;
using UnityEngine;

namespace ChipOS.Panels
{
    public class ChipOverlayPanel : HUDPanel
    {
        [SerializeField] private TMP_Text timeText;
        [SerializeField] private TMP_Text assistantStatusText;
        [SerializeField] private TMP_Text currentTaskText;
        [SerializeField] private TMP_Text notificationsText;
        [SerializeField] private TMP_Text ambientStateText;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private bool compactMode = true;
        [SerializeField] private float idlePulseSpeed = 2f;
        [SerializeField] private float alphaMin = 0.65f;
        [SerializeField] private float alphaMax = 0.92f;

        public void Render(ContextState state)
        {
            if (state == null) return;
            if (timeText != null) timeText.text = DateTime.Now.ToString("HH:mm");
            if (assistantStatusText != null) assistantStatusText.text = state.AssistantMessage;
            if (currentTaskText != null && state.Tasks.Count > 0) currentTaskText.text = state.Tasks[0].Name;
            if (notificationsText != null) notificationsText.text = "No critical alerts";
            if (ambientStateText != null) ambientStateText.text = $"Mode {state.Mode} | Net {state.Network}";
        }

        public void ToggleCompactMode()
        {
            compactMode = !compactMode;
            transform.localScale = compactMode ? Vector3.one : Vector3.one * 1.15f;
        }

        private void Update()
        {
            if (canvasGroup != null)
                canvasGroup.alpha = Mathf.Lerp(alphaMin, alphaMax, (Mathf.Sin(Time.time * idlePulseSpeed) + 1f) * 0.5f);
        }
    }
}
