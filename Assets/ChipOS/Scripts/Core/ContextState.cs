using System;
using System.Collections.Generic;

namespace ChipOS.Core
{
    /// <summary>
    /// Runtime model for AR context data. Keep this simple and replaceable.
    /// Future integrations (NRSDK tracking, network checks, sensors) should update this state.
    /// </summary>
    [Serializable]
    public class ContextState
    {
        public enum TaskStatus
        {
            Now,
            Next,
            Parked
        }

        [Serializable]
        public class TaskItem
        {
            public string Name;
            public TaskStatus Status;

            public TaskItem(string name, TaskStatus status)
            {
                Name = name;
                Status = status;
            }

            public void CycleStatus()
            {
                Status = Status switch
                {
                    TaskStatus.Now => TaskStatus.Next,
                    TaskStatus.Next => TaskStatus.Parked,
                    _ => TaskStatus.Now
                };
            }
        }

        public string AssistantMessage = "ChipOS online";
        public string Mode = "Desk";
        public string Tracking = "Checking";
        public string Input = "Keyboard Mock";
        public string Network = "Unknown";
        public string BatterySummary = "Unknown";
        public string WeatherSummary = "Unavailable";
        public string CurrentTimeText = "--:--:--";
        public string CurrentDateText = "--- -- ---";
        public string FocusedPanelId = "chip";

        public int BatteryPercent = -1;
        public bool BatteryCharging;
        public bool GazeFollowEnabled;
        public bool HighContrastEnabled;
        public float PanelOpacity = 0.8f;
        public float FontScale = 1f;

        public readonly List<TaskItem> Tasks = new()
        {
            new TaskItem("Finish Door in Four driver flow", TaskStatus.Now),
            new TaskItem("Test XREAL display mode", TaskStatus.Next),
            new TaskItem("Write one AR OS note", TaskStatus.Parked)
        };
    }
}
