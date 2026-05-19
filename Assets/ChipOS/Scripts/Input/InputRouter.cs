using ChipOS.Core;
using ChipOS.Spatial;
using UnityEngine;

namespace ChipOS.Input
{
    public class InputRouter : MonoBehaviour
    {
        [SerializeField] private ChipOSManager chipOSManager;
        [SerializeField] private WorkspaceLayoutManager workspaceLayoutManager;

        private void Update()
        {
            if (chipOSManager == null) return;

            if (UnityEngine.Input.GetKeyDown(KeyCode.H)) chipOSManager.ToggleHUD();
            if (UnityEngine.Input.GetKeyDown(KeyCode.R)) chipOSManager.ResetLayout();
            if (UnityEngine.Input.GetKeyDown(KeyCode.P)) chipOSManager.PanicMode();
            if (UnityEngine.Input.GetKeyDown(KeyCode.T)) chipOSManager.CycleTaskStatus();
            if (UnityEngine.Input.GetKeyDown(KeyCode.K)) chipOSManager.SaveLayout();

            if (workspaceLayoutManager == null) return;
            if (UnityEngine.Input.GetKeyDown(KeyCode.F1)) workspaceLayoutManager.ApplyLayout(WorkspaceLayout.FocusMode);
            if (UnityEngine.Input.GetKeyDown(KeyCode.F2)) workspaceLayoutManager.ApplyLayout(WorkspaceLayout.WalkingMode);
            if (UnityEngine.Input.GetKeyDown(KeyCode.F3)) workspaceLayoutManager.ApplyLayout(WorkspaceLayout.DeskMode);
            if (UnityEngine.Input.GetKeyDown(KeyCode.F4)) workspaceLayoutManager.ApplyLayout(WorkspaceLayout.MinimalMode);
            if (UnityEngine.Input.GetKeyDown(KeyCode.F5)) workspaceLayoutManager.ApplyLayout(WorkspaceLayout.PanicMode);
        }
    }
}
