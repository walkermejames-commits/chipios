using System;
using System.Collections.Generic;
using ChipOS.Panels;
using ChipOS.XR;
using UnityEngine;

namespace ChipOS.Spatial
{
    [Serializable]
    public class PanelAnchorData
    {
        public string PanelId;
        public AnchorZone Zone;
        public Vector3 Position;
        public Vector3 EulerAngles;
        public Vector3 Scale;
        public bool IsPinned;
        public bool FollowEnabled;
    }

    [Serializable]
    public class PanelAnchorCollection
    {
        public string ActiveLayout = "Focus Mode";
        public List<PanelAnchorData> Anchors = new();
    }

    public class SpatialAnchorManager : MonoBehaviour, ISpatialAnchorProvider
    {
        private const string SaveKey = "chipos.spatial.anchors.v1";

        [SerializeField] private List<HUDPanel> trackedPanels = new();
        [SerializeField] private bool autoLoadOnStart = true;

        private readonly PanelAnchorCollection _cache = new();

        public string ActiveLayoutName
        {
            get => _cache.ActiveLayout;
            set => _cache.ActiveLayout = value;
        }

        private void Start()
        {
            if (autoLoadOnStart)
                LoadAnchors();
        }

        public void RegisterPanel(HUDPanel panel)
        {
            if (panel != null && !trackedPanels.Contains(panel))
                trackedPanels.Add(panel);
        }

        public void SaveAnchors()
        {
            _cache.Anchors.Clear();
            foreach (var panel in trackedPanels)
            {
                if (panel == null) continue;
                var follow = panel.GetComponent<FollowBehaviour>();
                _cache.Anchors.Add(new PanelAnchorData
                {
                    PanelId = panel.PanelId,
                    Zone = panel.Zone,
                    Position = panel.transform.position,
                    EulerAngles = panel.transform.rotation.eulerAngles,
                    Scale = panel.transform.localScale,
                    IsPinned = follow != null && follow.IsPinned,
                    FollowEnabled = follow != null && follow.FollowEnabled
                });
            }

            PlayerPrefs.SetString(SaveKey, JsonUtility.ToJson(_cache));
            PlayerPrefs.Save();
        }

        public void LoadAnchors()
        {
            if (!PlayerPrefs.HasKey(SaveKey)) return;

            var payload = JsonUtility.FromJson<PanelAnchorCollection>(PlayerPrefs.GetString(SaveKey));
            if (payload == null) return;

            _cache.ActiveLayout = payload.ActiveLayout;
            foreach (var data in payload.Anchors)
            {
                var panel = trackedPanels.Find(p => p != null && p.PanelId == data.PanelId);
                if (panel == null) continue;

                panel.Zone = data.Zone;
                panel.transform.position = data.Position;
                panel.transform.rotation = Quaternion.Euler(data.EulerAngles);
                panel.transform.localScale = data.Scale;

                var follow = panel.GetComponent<FollowBehaviour>();
                if (follow != null)
                {
                    follow.SetPinned(data.IsPinned);
                    follow.SetFollowEnabled(data.FollowEnabled);
                }
            }
        }

        public void ResetAnchors()
        {
            PlayerPrefs.DeleteKey(SaveKey);
            foreach (var panel in trackedPanels)
            {
                if (panel == null) continue;
                panel.ResetLayout();
                var follow = panel.GetComponent<FollowBehaviour>();
                if (follow != null)
                {
                    follow.SetPinned(false);
                    follow.SetFollowEnabled(panel.Zone == AnchorZone.Follow);
                }
            }
        }

        public bool TryCreateOrUpdateAnchor(string anchorId, Pose pose, AnchorZone zone)
        {
            // local stub provider until XR runtime provider is bound
            return !string.IsNullOrWhiteSpace(anchorId);
        }

        public bool TryGetAnchorPose(string anchorId, out Pose pose)
        {
            pose = Pose.identity;
            return false;
        }

        public bool RemoveAnchor(string anchorId) => true;

        public void ClearAll() => ResetAnchors();
    }
}
