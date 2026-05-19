using System;
using ChipOS.Panels;
using ChipOS.Spatial;
using TMPro;
using UnityEngine;

namespace ChipOS.Core
{
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
        [SerializeField] private ChipOverlayPanel overlayPanel;

        [Header("Spatial")]
        [SerializeField] private SpatialAnchorManager spatialAnchorManager;

        [Header("Status")]
        [SerializeField] private string appStatus = "Status: Running";

        private readonly ContextState _contextState = new();
        private bool _hudVisible = true;

        public ContextState State => _contextState;

        private void Start()
        {
            InitializePanels();
            RenderAll();
            spatialAnchorManager?.LoadAnchors();
        }

        private void Update() => UpdateClock();

        public void ToggleHUD() { _hudVisible = !_hudVisible; SetAllPanelsVisible(_hudVisible); }

        public void PanicMode() { _hudVisible = false; SetAllPanelsVisible(false); }

        public void ResetLayout()
        {
            chipPanel?.ResetLayout();
            taskPanel?.ResetLayout();
            environmentPanel?.ResetLayout();
            overlayPanel?.ResetLayout();
            spatialAnchorManager?.ResetAnchors();
        }

        public void CycleTaskStatus() => taskPanel?.CyclePrimaryTaskStatus(_contextState);
        public void SaveLayout() => spatialAnchorManager?.SaveAnchors();

        private void InitializePanels()
        {
            chipPanel?.Initialize();
            taskPanel?.Initialize();
            environmentPanel?.Initialize();
            overlayPanel?.Initialize();
        }

        private void SetAllPanelsVisible(bool visible)
        {
            if (visible) { chipPanel?.Show(); taskPanel?.Show(); environmentPanel?.Show(); overlayPanel?.Show(); }
            else { chipPanel?.Hide(); taskPanel?.Hide(); environmentPanel?.Hide(); overlayPanel?.Hide(); }
        }

        private void RenderAll()
        {
            UpdateClock();
            if (statusText != null) statusText.text = appStatus;
            if (assistantText != null) assistantText.text = _contextState.AssistantMessage;
            taskPanel?.Render(_contextState);
            environmentPanel?.Render(_contextState);
            overlayPanel?.Render(_contextState);
        }

        private void UpdateClock()
        {
            if (timeText != null) timeText.text = DateTime.Now.ToString("HH:mm:ss");
            overlayPanel?.Render(_contextState);
        }
    }
}
