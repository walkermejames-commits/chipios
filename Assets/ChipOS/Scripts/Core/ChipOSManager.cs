using System.Collections.Generic;
using ChipOS.Persistence;
using ChipOS.Services;
using ChipOS.UI.Notifications;
using ChipOS.UI.Panels;
using ChipOS.UI.Theme;
using TMPro;
using UnityEngine;

namespace ChipOS.Core
{
    /// <summary>
    /// Global coordinator for the ChipOS spatial context layer.
    /// Keeps the runtime loop small while delegating visuals, services, persistence, and input adapters.
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
        [SerializeField] private DebugOverlayPanel debugOverlayPanel;
        [SerializeField] private NotificationPanel notificationPanel;

        [Header("Spatial Context")]
        [SerializeField] private Transform gazeAnchor;
        [SerializeField] private ChipOSThemeSettings themeSettings = new();

        [Header("Status")]
        [SerializeField] private string appStatus = "Status: Running";

        private readonly ContextState _contextState = new();
        private readonly PanelLayoutPersistence _layoutPersistence = new();
        private readonly ChipServiceRegistry _serviceRegistry = new();
        private readonly List<HUDPanel> _managedPanels = new();
        private bool _hudVisible = true;
        private int _focusedPanelIndex;
        private float _layoutFlushTimer;
        private ChipOSThemeController _themeController;

        public ContextState State => _contextState;

        private void Awake()
        {
            InitializeServices();
            InitializeTheme();
        }

        private void Start()
        {
            ResolveGazeAnchor();
            InitializePanels();
            InitializeFocusState();
            SyncStateFlags();
            RenderAll();
            notificationPanel?.ShowMessage("ChipOS v0.2 Spatial Context Layer online");
        }

        private void Update()
        {
            _serviceRegistry.RefreshAll(_contextState);
            FlushPendingLayoutWrites();
            SyncStateFlags();
            RenderAll();
        }

        public void ToggleHUD()
        {
            _hudVisible = !_hudVisible;
            SetAllPanelsVisible(_hudVisible);
            notificationPanel?.ShowMessage(_hudVisible ? "HUD shown" : "HUD hidden");
        }

        public void PanicMode()
        {
            _hudVisible = false;
            SetAllPanelsVisible(false);
            notificationPanel?.ShowMessage("Panic mode engaged");
        }

        public void ResetLayout()
        {
            for (var i = 0; i < _managedPanels.Count; i++)
            {
                _managedPanels[i]?.ResetLayout();
            }

            notificationPanel?.ShowMessage("Panel layout reset");
        }

        public void CycleTaskStatus()
        {
            taskPanel?.CyclePrimaryTaskStatus(_contextState);
            notificationPanel?.ShowMessage("Primary task status updated");
        }

        public void ToggleGazeFollow()
        {
            _contextState.GazeFollowEnabled = !_contextState.GazeFollowEnabled;

            for (var i = 0; i < _managedPanels.Count; i++)
            {
                _managedPanels[i]?.SetGazeFollow(_contextState.GazeFollowEnabled, gazeAnchor);
            }

            notificationPanel?.ShowMessage(_contextState.GazeFollowEnabled ? "Gaze follow enabled" : "Gaze follow disabled");
        }

        public void ToggleContrastMode()
        {
            themeSettings.ToggleContrast();
            ApplyTheme();
            notificationPanel?.ShowMessage(themeSettings.HighContrastEnabled ? "High contrast enabled" : "High contrast disabled");
        }

        public void AdjustFontScale(float delta)
        {
            themeSettings.AdjustFontScale(delta);
            ApplyTheme();
            notificationPanel?.ShowMessage($"Font scale {themeSettings.FontScale:F2}");
        }

        public void MoveFocusedPanel(Vector3 referenceDelta)
        {
            if (_managedPanels.Count == 0 || referenceDelta == Vector3.zero)
            {
                return;
            }

            var reference = gazeAnchor != null ? gazeAnchor : transform;
            var worldDelta =
                (reference.right * referenceDelta.x) +
                (reference.up * referenceDelta.y) +
                (reference.forward * referenceDelta.z);

            _managedPanels[_focusedPanelIndex]?.MoveByWorld(worldDelta);
        }

        public void SelectNextPanel()
        {
            if (_managedPanels.Count == 0)
            {
                return;
            }

            _focusedPanelIndex = (_focusedPanelIndex + 1) % _managedPanels.Count;
            UpdateFocusedPanel();
            notificationPanel?.ShowMessage($"Focused {_contextState.FocusedPanelId}");
        }

        public void SelectPreviousPanel()
        {
            if (_managedPanels.Count == 0)
            {
                return;
            }

            _focusedPanelIndex = (_focusedPanelIndex - 1 + _managedPanels.Count) % _managedPanels.Count;
            UpdateFocusedPanel();
            notificationPanel?.ShowMessage($"Focused {_contextState.FocusedPanelId}");
        }

        public void ToggleDebugOverlay()
        {
            debugOverlayPanel?.ToggleVisibility();
        }

        public void PushNotification(string message)
        {
            notificationPanel?.ShowMessage(message);
        }

        public void SetInputLabel(string inputLabel)
        {
            _contextState.Input = inputLabel;
        }

        private void InitializePanels()
        {
            _managedPanels.Clear();
            RegisterPanel(chipPanel);
            RegisterPanel(taskPanel);
            RegisterPanel(environmentPanel);
            RegisterPanel(debugOverlayPanel);

            for (var i = 0; i < _managedPanels.Count; i++)
            {
                _managedPanels[i].Initialize(_layoutPersistence, gazeAnchor);
            }

            debugOverlayPanel?.Hide();
            ApplyTheme();
        }

        private void SetAllPanelsVisible(bool visible)
        {
            for (var i = 0; i < _managedPanels.Count; i++)
            {
                if (_managedPanels[i] == debugOverlayPanel)
                {
                    continue;
                }

                if (visible)
                {
                    _managedPanels[i]?.Show();
                }
                else
                {
                    _managedPanels[i]?.Hide();
                }
            }
        }

        private void RenderAll()
        {
            if (timeText != null)
            {
                timeText.text = _contextState.CurrentTimeText;
            }

            if (statusText != null)
            {
                statusText.text = $"{appStatus} | {_contextState.CurrentDateText}";
            }

            if (assistantText != null)
            {
                assistantText.text = _contextState.AssistantMessage;
            }

            taskPanel?.Render(_contextState);
            environmentPanel?.Render(_contextState);
            debugOverlayPanel?.Render(_contextState, _serviceRegistry.BuildDebugSummary(_contextState));
        }

        private void InitializeServices()
        {
            _serviceRegistry.Register(new TimeDateService());
            _serviceRegistry.Register(new BatteryStateService());
            _serviceRegistry.Register(new NetworkStateService());
            _serviceRegistry.Register(new MockWeatherService());
        }

        private void InitializeTheme()
        {
            _themeController = new ChipOSThemeController(themeSettings);
            _themeController.RegisterNotificationPanel(notificationPanel);
        }

        private void ApplyTheme()
        {
            _themeController?.Apply();
            SyncStateFlags();
        }

        private void RegisterPanel(HUDPanel panel)
        {
            if (panel == null)
            {
                return;
            }

            _managedPanels.Add(panel);
            _themeController?.RegisterPanel(panel);
        }

        private void InitializeFocusState()
        {
            _focusedPanelIndex = 0;
            UpdateFocusedPanel();
        }

        private void UpdateFocusedPanel()
        {
            for (var i = 0; i < _managedPanels.Count; i++)
            {
                var isFocused = i == _focusedPanelIndex;
                _managedPanels[i]?.SetFocused(isFocused);

                if (isFocused && _managedPanels[i] != null)
                {
                    _contextState.FocusedPanelId = _managedPanels[i].PanelId;
                }
            }
        }

        private void ResolveGazeAnchor()
        {
            if (gazeAnchor == null && Camera.main != null)
            {
                gazeAnchor = Camera.main.transform;
            }
        }

        private void SyncStateFlags()
        {
            _contextState.GazeFollowEnabled = _managedPanels.Count > 0 && _managedPanels[0] != null && _managedPanels[0].GazeFollowEnabled;
            _contextState.HighContrastEnabled = themeSettings.HighContrastEnabled;
            _contextState.PanelOpacity = themeSettings.PanelOpacity;
            _contextState.FontScale = themeSettings.FontScale;
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                _layoutPersistence.Flush();
            }
        }

        private void OnApplicationQuit()
        {
            _layoutPersistence.Flush();
        }

        private void FlushPendingLayoutWrites()
        {
            if (!_layoutPersistence.HasPendingWrites)
            {
                _layoutFlushTimer = 0f;
                return;
            }

            _layoutFlushTimer += Time.deltaTime;
            if (_layoutFlushTimer < 1f)
            {
                return;
            }

            _layoutPersistence.Flush();
            _layoutFlushTimer = 0f;
        }
    }
}
