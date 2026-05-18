using ChipOS.Core;

namespace ChipOS.Services
{
    public interface IContextService
    {
        string Name { get; }
        void Refresh(ContextState state);
        string GetDebugValue(ContextState state);
    }
}
