using System;
using ChipOS.Panels;
using TMPro;
using UnityEngine;

namespace ChipOS.Core
{
    /// <summary>
    /// Global HUD manager for v0.1 Context Layer.
    /// Coordinates panel rendering, visibility, panic mode, and layout reset.
    /// </summary>
    public class ChipOSManager : MonoBehaviour
    {
        [Header("Main Chip Panel")]
        [SerializeField] private HUDPanel chipPanel;
        [SerializeField] private TMP_Text timeText;
        [SerializeField] private TMP_Text statusText;
        [SerializeField] private TMP_Text assistantText;

        [Header("Feature Panels")]
        [SerializeField] private TaskPanel taskPanel;
        [SerializeField] private EnvironmentPanel environmentPanel;

        [Header("Status")]
        [SerializeField] private string appStatus = "Status: Running";

        private readonly ContextState _contextState = new();
        private bool _hudVisible = true;

        public ContextState State => _contextState;

        private void Start()
        {
            InitializePanels();
            RenderAll();
        }

        private void Update()
        {
            UpdateClock();
        }

        public void ToggleHUD()
        {
            _hudVisible = !_hudVisible;
            SetAllPanelsVisible(_hudVisible);
        }

        public void PanicMode()
        {
            _hudVisible = false;
            SetAllPanelsVisible(false);
        }

        public void ResetLayout()
        {
            chipPanel?.ResetLayout();
            taskPanel?.ResetLayout();
            environmentPanel?.ResetLayout();
        }

        public void CycleTaskStatus()
        {
            taskPanel?.CyclePrimaryTaskStatus(_contextState);
        }

        private void InitializePanels()
        {
            chipPanel?.Initialize();
            taskPanel?.Initialize();
            environmentPanel?.Initialize();
        }

        private void SetAllPanelsVisible(bool visible)
        {
            if (visible)
            {
                chipPanel?.Show();
                taskPanel?.Show();
                environmentPanel?.Show();
            }
            else
            {
                chipPanel?.Hide();
                taskPanel?.Hide();
                environmentPanel?.Hide();
            }
        }

        private void RenderAll()
        {
            UpdateClock();
            if (statusText != null) statusText.text = appStatus;
            if (assistantText != null) assistantText.text = _contextState.AssistantMessage;

            taskPanel?.Render(_contextState);
            environmentPanel?.Render(_contextState);
        }

        private void UpdateClock()
        {
            if (timeText != null)
                timeText.text = DateTime.Now.ToString("HH:mm:ss");
        }
    }
}
