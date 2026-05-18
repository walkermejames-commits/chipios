using System;
using ChipOS.Core;

namespace ChipOS.Services
{
    public sealed class TimeDateService : IContextService
    {
        public string Name => "Time";

        public void Refresh(ContextState state)
        {
            var now = DateTime.Now;
            state.CurrentTimeText = now.ToString("HH:mm:ss");
            state.CurrentDateText = now.ToString("ddd dd MMM");
        }

        public string GetDebugValue(ContextState state)
        {
            return $"{state.CurrentTimeText} {state.CurrentDateText}";
        }
    }
}
