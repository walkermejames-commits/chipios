using ChipOS.Core;
using UnityEngine;

namespace ChipOS.Services
{
    public sealed class BatteryStateService : IContextService
    {
        public string Name => "Battery";

        public void Refresh(ContextState state)
        {
            var batteryLevel = SystemInfo.batteryLevel;
            var batteryPercent = batteryLevel < 0f ? -1 : Mathf.RoundToInt(batteryLevel * 100f);
            var batteryStatus = SystemInfo.batteryStatus;

            state.BatteryPercent = batteryPercent;
            state.BatteryCharging = batteryStatus == BatteryStatus.Charging || batteryStatus == BatteryStatus.Full;
            state.BatterySummary = batteryPercent < 0
                ? $"{batteryStatus}"
                : $"{batteryPercent}% {(state.BatteryCharging ? "(Charging)" : string.Empty)}".Trim();
        }

        public string GetDebugValue(ContextState state)
        {
            return state.BatterySummary;
        }
    }
}
