using System.Collections.Generic;
using ChipOS.UI.Notifications;
using ChipOS.UI.Panels;

namespace ChipOS.UI.Theme
{
    /// <summary>
    /// Applies a single theme definition across the active ChipOS panels and overlays.
    /// </summary>
    public sealed class ChipOSThemeController
    {
        private readonly List<HUDPanel> _panels = new();
        private NotificationPanel _notificationPanel;

        public ChipOSThemeSettings Theme { get; }

        public ChipOSThemeController(ChipOSThemeSettings theme)
        {
            Theme = theme;
        }

        public void RegisterPanel(HUDPanel panel)
        {
            if (panel == null || _panels.Contains(panel))
            {
                return;
            }

            _panels.Add(panel);
            panel.ApplyTheme(Theme);
        }

        public void RegisterNotificationPanel(NotificationPanel notificationPanel)
        {
            _notificationPanel = notificationPanel;
            _notificationPanel?.ApplyTheme(Theme);
        }

        public void Apply()
        {
            for (var i = 0; i < _panels.Count; i++)
            {
                _panels[i]?.ApplyTheme(Theme);
            }

            _notificationPanel?.ApplyTheme(Theme);
        }
    }
}
