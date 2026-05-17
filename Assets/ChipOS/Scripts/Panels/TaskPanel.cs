using System.Text;
using ChipOS.Core;
using TMPro;
using UnityEngine;

namespace ChipOS.Panels
{
    public class TaskPanel : HUDPanel
    {
        [SerializeField] private TMP_Text taskBodyText;

        public void Render(ContextState state)
        {
            if (taskBodyText == null || state == null) return;

            var sb = new StringBuilder();
            for (var i = 0; i < state.Tasks.Count; i++)
            {
                var task = state.Tasks[i];
                sb.Append("• ").Append(task.Name).Append(" [").Append(task.Status).AppendLine("]");
            }

            taskBodyText.text = sb.ToString().TrimEnd();
        }

        public void CyclePrimaryTaskStatus(ContextState state)
        {
            if (state == null || state.Tasks.Count == 0) return;
            state.Tasks[0].CycleStatus();
            Render(state);
        }
    }
}
