using ChipOS.Core;
using UnityEngine;

namespace ChipOS.Input
{
    /// <summary>
    /// Temporary test input mapping. Later replace with NRSDK hand/gaze/voice adapters.
    /// </summary>
    public class InputRouter : MonoBehaviour
    {
        [SerializeField] private ChipOSManager chipOSManager;

        private void Update()
        {
            if (chipOSManager == null) return;

            if (UnityEngine.Input.GetKeyDown(KeyCode.H))
                chipOSManager.ToggleHUD();

            if (UnityEngine.Input.GetKeyDown(KeyCode.R))
                chipOSManager.ResetLayout();

            if (UnityEngine.Input.GetKeyDown(KeyCode.P))
                chipOSManager.PanicMode();

            if (UnityEngine.Input.GetKeyDown(KeyCode.T))
                chipOSManager.CycleTaskStatus();
        }
    }
}
