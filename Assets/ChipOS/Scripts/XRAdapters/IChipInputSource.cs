using UnityEngine;

namespace ChipOS.XRAdapters
{
    /// <summary>
    /// Abstraction seam for keyboard mock input today and NRSDK hand/gaze input later.
    /// </summary>
    public interface IChipInputSource
    {
        string DisplayName { get; }
        bool GetActionDown(XRInputAction action);
        Vector3 GetPanelMoveVector();
    }
}
