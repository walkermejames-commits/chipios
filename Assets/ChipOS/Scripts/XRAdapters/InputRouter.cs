using ChipOS.Core;
using UnityEngine;

namespace ChipOS.XRAdapters
{
    /// <summary>
    /// Routes abstract ChipOS actions into the manager.
    /// Keyboard is retained as a mock backend until NRSDK input adapters are ready.
    /// </summary>
    public class InputRouter : MonoBehaviour
    {
        private enum InputBackend
        {
            KeyboardMock,
            NRSDKStub
        }

        [SerializeField] private ChipOSManager chipOSManager;
        [SerializeField] private InputBackend backend = InputBackend.KeyboardMock;
        [SerializeField] private float panelMoveSpeed = 0.25f;

        private IChipInputSource _inputSource;

        private void Awake()
        {
            if (chipOSManager == null)
            {
                chipOSManager = GetComponent<ChipOSManager>();
            }

            _inputSource = backend switch
            {
                InputBackend.NRSDKStub => new NRSDKInputSourceStub(),
                _ => new KeyboardInputSource()
            };

            chipOSManager?.SetInputLabel(_inputSource.DisplayName);
        }

        private void Update()
        {
            if (chipOSManager == null || _inputSource == null)
            {
                return;
            }

            if (_inputSource.GetActionDown(XRInputAction.ToggleHud))
                chipOSManager.ToggleHUD();

            if (_inputSource.GetActionDown(XRInputAction.ResetLayout))
                chipOSManager.ResetLayout();

            if (_inputSource.GetActionDown(XRInputAction.PanicMode))
                chipOSManager.PanicMode();

            if (_inputSource.GetActionDown(XRInputAction.CyclePrimaryTask))
                chipOSManager.CycleTaskStatus();

            if (_inputSource.GetActionDown(XRInputAction.ToggleGazeFollow))
                chipOSManager.ToggleGazeFollow();

            if (_inputSource.GetActionDown(XRInputAction.ToggleDebugOverlay))
                chipOSManager.ToggleDebugOverlay();

            if (_inputSource.GetActionDown(XRInputAction.NextPanel))
                chipOSManager.SelectNextPanel();

            if (_inputSource.GetActionDown(XRInputAction.PreviousPanel))
                chipOSManager.SelectPreviousPanel();

            if (_inputSource.GetActionDown(XRInputAction.ToggleContrastMode))
                chipOSManager.ToggleContrastMode();

            if (_inputSource.GetActionDown(XRInputAction.IncreaseFontScale))
                chipOSManager.AdjustFontScale(0.1f);

            if (_inputSource.GetActionDown(XRInputAction.DecreaseFontScale))
                chipOSManager.AdjustFontScale(-0.1f);

            if (_inputSource.GetActionDown(XRInputAction.ShowTestNotification))
                chipOSManager.PushNotification("Spatial context notification");

            var panelMove = _inputSource.GetPanelMoveVector();
            if (panelMove != Vector3.zero)
            {
                chipOSManager.MoveFocusedPanel(panelMoveSpeed * Time.deltaTime * panelMove);
            }
        }
    }
}
