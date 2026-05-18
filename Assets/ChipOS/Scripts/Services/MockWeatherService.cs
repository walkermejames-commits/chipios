using ChipOS.Core;

namespace ChipOS.Services
{
    /// <summary>
    /// Mock weather until a real provider or Android bridge is introduced.
    /// </summary>
    public sealed class MockWeatherService : IContextService
    {
        private static readonly string[] Presets =
        {
            "18C Clear",
            "17C Light Cloud",
            "16C Overcast",
            "14C Rain Watch"
        };

        public string Name => "Weather";

        public void Refresh(ContextState state)
        {
            var index = (System.DateTime.Now.Minute / 15) % Presets.Length;
            state.WeatherSummary = $"{Presets[index]} (mock)";
        }

        public string GetDebugValue(ContextState state)
        {
            return state.WeatherSummary;
        }
    }
}
