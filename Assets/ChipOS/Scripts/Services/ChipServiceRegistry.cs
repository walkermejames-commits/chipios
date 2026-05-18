using System.Collections.Generic;
using System.Text;
using ChipOS.Core;

namespace ChipOS.Services
{
    /// <summary>
    /// Keeps the v0.2 service layer small while still separating data providers from the manager.
    /// </summary>
    public sealed class ChipServiceRegistry
    {
        private readonly List<IContextService> _services = new();

        public void Register(IContextService service)
        {
            if (service != null)
            {
                _services.Add(service);
            }
        }

        public void RefreshAll(ContextState state)
        {
            for (var i = 0; i < _services.Count; i++)
            {
                _services[i].Refresh(state);
            }
        }

        public string BuildDebugSummary(ContextState state)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < _services.Count; i++)
            {
                var service = _services[i];
                sb.Append(service.Name).Append(": ").AppendLine(service.GetDebugValue(state));
            }

            return sb.ToString().TrimEnd();
        }
    }
}
