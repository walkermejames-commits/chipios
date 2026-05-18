using ChipOS.Core;
using UnityEngine;

namespace ChipOS.Services
{
    public sealed class NetworkStateService : IContextService
    {
        public string Name => "Network";

        public void Refresh(ContextState state)
        {
            state.Network = Application.internetReachability switch
            {
                NetworkReachability.ReachableViaCarrierDataNetwork => "Cellular",
                NetworkReachability.ReachableViaLocalAreaNetwork => "Wi-Fi",
                _ => "Offline"
            };
        }

        public string GetDebugValue(ContextState state)
        {
            return state.Network;
        }
    }
}
