using UnityEngine;

namespace ChipOS.Persistence
{
    /// <summary>
    /// Lightweight PlayerPrefs-backed layout storage for panel transforms.
    /// </summary>
    public sealed class PanelLayoutPersistence
    {
        private const string Prefix = "ChipOS.PanelLayout";
        private bool _hasPendingWrites;

        public bool HasPendingWrites => _hasPendingWrites;

        public bool TryLoad(string panelId, out PanelLayoutData layoutData)
        {
            var positionKey = BuildKey(panelId, "Position");
            var rotationKey = BuildKey(panelId, "Rotation");

            if (!PlayerPrefs.HasKey(positionKey) || !PlayerPrefs.HasKey(rotationKey))
            {
                layoutData = default;
                return false;
            }

            layoutData = new PanelLayoutData(
                JsonUtility.FromJson<Vector3Data>(PlayerPrefs.GetString(positionKey)).ToVector3(),
                JsonUtility.FromJson<Vector3Data>(PlayerPrefs.GetString(rotationKey)).ToVector3());
            return true;
        }

        public void Save(string panelId, Vector3 localPosition, Vector3 localEulerAngles)
        {
            PlayerPrefs.SetString(BuildKey(panelId, "Position"), JsonUtility.ToJson(new Vector3Data(localPosition)));
            PlayerPrefs.SetString(BuildKey(panelId, "Rotation"), JsonUtility.ToJson(new Vector3Data(localEulerAngles)));
            _hasPendingWrites = true;
        }

        public void Clear(string panelId)
        {
            PlayerPrefs.DeleteKey(BuildKey(panelId, "Position"));
            PlayerPrefs.DeleteKey(BuildKey(panelId, "Rotation"));
            _hasPendingWrites = true;
        }

        public void Flush()
        {
            if (!_hasPendingWrites)
            {
                return;
            }

            PlayerPrefs.Save();
            _hasPendingWrites = false;
        }

        private static string BuildKey(string panelId, string suffix)
        {
            return $"{Prefix}.{panelId}.{suffix}";
        }
    }

    public readonly struct PanelLayoutData
    {
        public readonly Vector3 LocalPosition;
        public readonly Vector3 LocalEulerAngles;

        public PanelLayoutData(Vector3 localPosition, Vector3 localEulerAngles)
        {
            LocalPosition = localPosition;
            LocalEulerAngles = localEulerAngles;
        }
    }

    [System.Serializable]
    public struct Vector3Data
    {
        public float x;
        public float y;
        public float z;

        public Vector3Data(Vector3 value)
        {
            x = value.x;
            y = value.y;
            z = value.z;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }
    }
}
