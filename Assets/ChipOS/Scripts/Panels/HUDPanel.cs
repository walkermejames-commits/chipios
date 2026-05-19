using ChipOS.XR;
using UnityEngine;

namespace ChipOS.Panels
{
    public class HUDPanel : MonoBehaviour
    {
        [Header("Identity")]
        [SerializeField] private string panelId = "panel";
        [SerializeField] private AnchorZone zone = AnchorZone.Center;

        [Header("Layout")]
        [SerializeField] private Vector3 defaultLocalPosition = new(0f, 0f, 1.2f);
        [SerializeField] private Vector3 defaultLocalEulerAngles = Vector3.zero;
        [SerializeField] private Vector3 defaultScale = Vector3.one;

        [Header("Optional Test Reposition")]
        [SerializeField] private bool allowKeyboardNudge;
        [SerializeField] private float nudgeSpeed = 0.2f;

        [Header("Polish")]
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float fadeDuration = 0.25f;

        private bool _isVisible = true;
        public string PanelId => panelId;
        public AnchorZone Zone { get => zone; set => zone = value; }

        public virtual void Initialize()
        {
            ResetLayout();
            Show();
        }

        public virtual void Show()
        {
            _isVisible = true;
            gameObject.SetActive(true);
            SetAlpha(1f);
        }

        public virtual void Hide()
        {
            _isVisible = false;
            SetAlpha(0f);
            gameObject.SetActive(false);
        }

        public virtual void ToggleVisibility()
        {
            if (_isVisible) Hide(); else Show();
        }

        public virtual void ResetLayout()
        {
            transform.localPosition = defaultLocalPosition;
            transform.localRotation = Quaternion.Euler(defaultLocalEulerAngles);
            transform.localScale = defaultScale;
        }

        protected virtual void Update()
        {
            if (!allowKeyboardNudge) return;
            var move = new Vector3(UnityEngine.Input.GetAxisRaw("Horizontal"), UnityEngine.Input.GetAxisRaw("Vertical"), 0f) * (nudgeSpeed * Time.deltaTime);
            transform.localPosition += move;
        }

        private void SetAlpha(float target)
        {
            if (canvasGroup == null) return;
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / Mathf.Max(0.0001f, fadeDuration));
        }
    }
}
