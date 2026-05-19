using ChipOS.XR;
using UnityEngine;

namespace ChipOS.Spatial
{
    public class FollowBehaviour : MonoBehaviour
    {
        [Header("Follow")]
        [SerializeField] private bool followEnabled = true;
        [SerializeField] private bool isPinned;
        [SerializeField] private float followDistance = 1.2f;
        [SerializeField] private float verticalOffset = -0.05f;
        [SerializeField] private float positionSmoothing = 8f;
        [SerializeField] private float rotationSmoothing = 6f;
        [SerializeField] private float maxMotionSpeed = 1.8f;

        [Header("References")]
        [SerializeField] private CameraHeadPoseProvider headPoseProvider;

        public bool IsPinned => isPinned;
        public bool FollowEnabled => followEnabled;

        private void Update()
        {
            if (!followEnabled || isPinned || headPoseProvider == null) return;

            var targetPos = headPoseProvider.Position + headPoseProvider.Forward * followDistance + Vector3.up * verticalOffset;
            var move = Vector3.ClampMagnitude(targetPos - transform.position, maxMotionSpeed * Time.deltaTime);
            var desiredPos = transform.position + move;
            transform.position = Vector3.Lerp(transform.position, desiredPos, 1f - Mathf.Exp(-positionSmoothing * Time.deltaTime));

            var lookDir = (transform.position - headPoseProvider.Position).normalized;
            if (lookDir.sqrMagnitude > 0.0001f)
            {
                var targetRot = Quaternion.LookRotation(lookDir, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 1f - Mathf.Exp(-rotationSmoothing * Time.deltaTime));
            }
        }

        public void TogglePin() => isPinned = !isPinned;
        public void ToggleFollow() => followEnabled = !followEnabled;
        public void SetPinned(bool value) => isPinned = value;
        public void SetFollowEnabled(bool value) => followEnabled = value;
    }
}
