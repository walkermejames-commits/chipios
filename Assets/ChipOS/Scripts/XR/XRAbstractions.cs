using UnityEngine;

namespace ChipOS.XR
{
    public enum AnchorZone
    {
        Left,
        Right,
        Center,
        Follow,
        Desk,
        Ceiling
    }

    public interface IHeadPoseProvider
    {
        Vector3 Position { get; }
        Quaternion Rotation { get; }
        Vector3 Forward { get; }
    }

    public interface ISpatialAnchorProvider
    {
        bool TryCreateOrUpdateAnchor(string anchorId, Pose pose, AnchorZone zone);
        bool TryGetAnchorPose(string anchorId, out Pose pose);
        bool RemoveAnchor(string anchorId);
        void ClearAll();
    }

    public interface IHandTrackingProvider
    {
        bool IsPinching { get; }
        Vector3 PrimaryHandPosition { get; }
    }

    public interface IGestureInputProvider
    {
        bool GetGestureDown(string gestureName);
    }

    public class CameraHeadPoseProvider : MonoBehaviour, IHeadPoseProvider
    {
        [SerializeField] private Transform headTransform;

        public Vector3 Position => headTransform != null ? headTransform.position : transform.position;
        public Quaternion Rotation => headTransform != null ? headTransform.rotation : transform.rotation;
        public Vector3 Forward => headTransform != null ? headTransform.forward : transform.forward;

        private void Reset()
        {
            if (Camera.main != null)
                headTransform = Camera.main.transform;
        }
    }
}
