using UnityEngine;

namespace ChipOS.XRAdapters
{
    public sealed class KeyboardInputSource : IChipInputSource
    {
        public string DisplayName => "Keyboard Mock";

        public bool GetActionDown(XRInputAction action)
        {
            return action switch
            {
                XRInputAction.ToggleHud => Input.GetKeyDown(KeyCode.H),
                XRInputAction.ResetLayout => Input.GetKeyDown(KeyCode.R),
                XRInputAction.PanicMode => Input.GetKeyDown(KeyCode.P),
                XRInputAction.CyclePrimaryTask => Input.GetKeyDown(KeyCode.T),
                XRInputAction.ToggleGazeFollow => Input.GetKeyDown(KeyCode.G),
                XRInputAction.ToggleDebugOverlay => Input.GetKeyDown(KeyCode.F1),
                XRInputAction.NextPanel => Input.GetKeyDown(KeyCode.RightBracket),
                XRInputAction.PreviousPanel => Input.GetKeyDown(KeyCode.LeftBracket),
                XRInputAction.ToggleContrastMode => Input.GetKeyDown(KeyCode.C),
                XRInputAction.IncreaseFontScale => Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.KeypadPlus),
                XRInputAction.DecreaseFontScale => Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus),
                XRInputAction.ShowTestNotification => Input.GetKeyDown(KeyCode.N),
                _ => false
            };
        }

        public Vector3 GetPanelMoveVector()
        {
            var horizontal = 0f;
            var vertical = 0f;
            var depth = 0f;

            if (Input.GetKey(KeyCode.LeftArrow)) horizontal -= 1f;
            if (Input.GetKey(KeyCode.RightArrow)) horizontal += 1f;
            if (Input.GetKey(KeyCode.DownArrow)) vertical -= 1f;
            if (Input.GetKey(KeyCode.UpArrow)) vertical += 1f;
            if (Input.GetKey(KeyCode.PageDown)) depth -= 1f;
            if (Input.GetKey(KeyCode.PageUp)) depth += 1f;

            return new Vector3(horizontal, vertical, depth).normalized;
        }
    }
}
