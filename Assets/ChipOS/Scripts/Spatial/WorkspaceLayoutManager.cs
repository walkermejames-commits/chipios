using System.Collections;
using System.Collections.Generic;
using ChipOS.Panels;
using ChipOS.XR;
using UnityEngine;

namespace ChipOS.Spatial
{
    public enum WorkspaceLayout
    {
        FocusMode,
        WalkingMode,
        DeskMode,
        MinimalMode,
        PanicMode
    }

    public class WorkspaceLayoutManager : MonoBehaviour
    {
        [SerializeField] private SpatialAnchorManager anchorManager;
        [SerializeField] private List<HUDPanel> panels = new();
        [SerializeField] private float transitionDuration = 0.35f;

        private readonly Dictionary<WorkspaceLayout, Dictionary<AnchorZone, Vector3>> _layoutOffsets = new();

        private void Awake()
        {
            _layoutOffsets[WorkspaceLayout.FocusMode] = BuildOffsets(1.0f, 1.15f);
            _layoutOffsets[WorkspaceLayout.WalkingMode] = BuildOffsets(0.75f, 1.0f);
            _layoutOffsets[WorkspaceLayout.DeskMode] = BuildOffsets(1.2f, 0.85f);
            _layoutOffsets[WorkspaceLayout.MinimalMode] = BuildOffsets(1.35f, 1.3f);
            _layoutOffsets[WorkspaceLayout.PanicMode] = BuildOffsets(3.5f, -2.0f);
        }

        public void ApplyLayout(WorkspaceLayout layout)
        {
            StopAllCoroutines();
            StartCoroutine(AnimateLayout(layout));
            if (anchorManager != null)
            {
                anchorManager.ActiveLayoutName = layout.ToString();
                anchorManager.SaveAnchors();
            }
        }

        private IEnumerator AnimateLayout(WorkspaceLayout layout)
        {
            var start = new Dictionary<HUDPanel, Vector3>();
            foreach (var panel in panels)
                if (panel != null) start[panel] = panel.transform.position;

            var t = 0f;
            while (t < transitionDuration)
            {
                t += Time.deltaTime;
                var a = Mathf.SmoothStep(0f, 1f, t / transitionDuration);
                foreach (var panel in panels)
                {
                    if (panel == null || !start.ContainsKey(panel)) continue;
                    var target = ResolvePositionForZone(panel.Zone, layout);
                    panel.transform.position = Vector3.Lerp(start[panel], target, a);
                }
                yield return null;
            }
        }

        private Vector3 ResolvePositionForZone(AnchorZone zone, WorkspaceLayout layout)
        {
            if (!_layoutOffsets.TryGetValue(layout, out var map) || !map.TryGetValue(zone, out var pos))
                return transform.position + transform.forward;
            return transform.TransformPoint(pos);
        }

        private static Dictionary<AnchorZone, Vector3> BuildOffsets(float depth, float height)
        {
            return new Dictionary<AnchorZone, Vector3>
            {
                [AnchorZone.Left] = new Vector3(-0.55f, height, depth),
                [AnchorZone.Right] = new Vector3(0.55f, height, depth),
                [AnchorZone.Center] = new Vector3(0f, height, depth),
                [AnchorZone.Follow] = new Vector3(0f, height, depth),
                [AnchorZone.Desk] = new Vector3(0f, 0.65f, depth + 0.2f),
                [AnchorZone.Ceiling] = new Vector3(0f, 1.8f, depth)
            };
        }
    }
}
