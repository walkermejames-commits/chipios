using UnityEngine;

namespace ChipOS.Panels
{
    /// <summary>
    /// Shared panel behaviors: show/hide, reset position, and basic move support.
    /// Attach this to each world-space panel root.
    /// </summary>
    public class HUDPanel : MonoBehaviour
    {
        [Header("Layout")]
        [SerializeField] private Vector3 defaultLocalPosition = new(0f, 0f, 1.2f);
        [SerializeField] private Vector3 defaultLocalEulerAngles = Vector3.zero;

        [Header("Optional Test Reposition")]
        [SerializeField] private bool allowKeyboardNudge = false;
        [SerializeField] private float nudgeSpeed = 0.2f;

        private bool _isVisible = true;

        public virtual void Initialize()
        {
            ResetLayout();
            Show();
        }

        public virtual void Show()
        {
            _isVisible = true;
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            _isVisible = false;
            gameObject.SetActive(false);
        }

        public virtual void ToggleVisibility()
        {
            if (_isVisible)
                Hide();
            else
                Show();
        }

        public virtual void ResetLayout()
        {
            transform.localPosition = defaultLocalPosition;
            transform.localRotation = Quaternion.Euler(defaultLocalEulerAngles);
        }

        private void Update()
        {
            if (!allowKeyboardNudge) return;

            var move = new Vector3(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical"),
                0f
            ) * (nudgeSpeed * Time.deltaTime);

            transform.localPosition += move;
        }
    }
}
