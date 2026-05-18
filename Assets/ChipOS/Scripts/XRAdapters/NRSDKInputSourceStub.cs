using UnityEngine;

namespace ChipOS.XRAdapters
{
    /// <summary>
    /// Placeholder backend that preserves the routing contract without binding to NRSDK yet.
    /// </summary>
    public sealed class NRSDKInputSourceStub : IChipInputSource
    {
        public string DisplayName => "NRSDK Stub";

        public bool GetActionDown(XRInputAction action)
        {
            return false;
        }

        public Vector3 GetPanelMoveVector()
        {
            return Vector3.zero;
        }
    }
}
