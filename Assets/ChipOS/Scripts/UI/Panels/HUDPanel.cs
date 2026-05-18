using ChipOS.Animations;
using ChipOS.Persistence;
using ChipOS.UI.Theme;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChipOS.UI.Panels
{
    /// <summary>
    /// Shared world-space panel behavior for ChipOS.
    /// Handles saved layout, focus state, theme application, smooth visibility, and optional gaze follow.
    /// </summary>
    public class HUDPanel : MonoBehaviour
    {
        [Header("Layout")]
        [SerializeField] private string panelId = "panel";
        [SerializeField] private Vector3 defaultLocalPosition = new(0f, 0f, 1.2f);
        [SerializeField] private Vector3 defaultLocalEulerAngles = Vector3.zero;

        [Header("Theme")]
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Graphic[] backgroundGraphics;
        [SerializeField] private TMP_Text[] textTargets;
        [SerializeField] private Graphic focusHighlight;

        [Header("Animation")]
        [SerializeField] private PanelAnimationProfile animationProfile = new();

        private PanelLayoutPersistence _layoutPersistence;
        private Transform _gazeAnchor;
        private bool _isInitialized;
        private bool _isVisible = true;
        private bool _isFocused;
        private bool _gazeFollowEnabled;
        private float _currentVisibility = 1f;
        private float _targetVisibility = 1f;
        private Vector3 _layoutLocalPosition;
        private Vector3 _layoutLocalEulerAngles;
        private Vector3 _gazeFollowOffset;
        private float[] _baseFontSizes;
        private ChipOSThemeSettings _themeSettings;

        public string PanelId => string.IsNullOrWhiteSpace(panelId) ? name : panelId;
        public bool IsVisible => _isVisible;
        public bool IsFocused => _isFocused;
        public bool GazeFollowEnabled => _gazeFollowEnabled;

        public virtual void Initialize(PanelLayoutPersistence layoutPersistence, Transform gazeAnchor)
        {
            _layoutPersistence = layoutPersistence;
            _gazeAnchor = gazeAnchor;

            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    canvasGroup = gameObject.AddComponent<CanvasGroup>();
                }
            }

            CacheBaseFontSizes();
            LoadLayout();
            ApplyLayoutImmediate();
            ShowImmediate();
            _isInitialized = true;
        }

        public virtual void Show()
        {
            _isVisible = true;
            _targetVisibility = 1f;
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            _isVisible = false;
            _targetVisibility = 0f;
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
            _layoutLocalPosition = defaultLocalPosition;
            _layoutLocalEulerAngles = defaultLocalEulerAngles;
            ApplyLayoutImmediate();

            if (_gazeFollowEnabled && _gazeAnchor != null)
            {
                _gazeFollowOffset = _gazeAnchor.InverseTransformPoint(transform.position);
            }

            SaveLayout();
        }

        public virtual void MoveByWorld(Vector3 worldDelta)
        {
            if (_gazeFollowEnabled && _gazeAnchor != null)
            {
                var nextWorldPosition = transform.position + worldDelta;
                _gazeFollowOffset = _gazeAnchor.InverseTransformPoint(nextWorldPosition);
                transform.position = nextWorldPosition;
                SyncLayoutFromTransform();
                SaveLayout();
                return;
            }

            var localDelta = transform.parent != null
                ? transform.parent.InverseTransformVector(worldDelta)
                : worldDelta;

            _layoutLocalPosition += localDelta;
            ApplyLayoutImmediate();
            SaveLayout();
        }

        public virtual void SetFocused(bool isFocused)
        {
            _isFocused = isFocused;
            ApplyTheme(_themeSettings);
        }

        public virtual void SetGazeFollow(bool enabled, Transform gazeAnchor)
        {
            _gazeAnchor = gazeAnchor;
            _gazeFollowEnabled = enabled;

            if (_gazeFollowEnabled && _gazeAnchor != null)
            {
                _gazeFollowOffset = _gazeAnchor.InverseTransformPoint(transform.position);
            }
            else
            {
                SyncLayoutFromTransform();
                SaveLayout();
            }
        }

        public virtual void ApplyTheme(ChipOSThemeSettings themeSettings)
        {
            _themeSettings = themeSettings;
            if (themeSettings == null)
            {
                return;
            }

            var backgroundColor = _isFocused
                ? themeSettings.GetFocusedPanelColor()
                : themeSettings.GetPanelColor();

            if (backgroundGraphics != null)
            {
                for (var i = 0; i < backgroundGraphics.Length; i++)
                {
                    if (backgroundGraphics[i] != null)
                    {
                        backgroundGraphics[i].color = backgroundColor;
                    }
                }
            }

            if (focusHighlight != null)
            {
                focusHighlight.color = _isFocused
                    ? themeSettings.AccentColor
                    : new Color(themeSettings.AccentColor.r, themeSettings.AccentColor.g, themeSettings.AccentColor.b, 0f);
            }

            if (textTargets != null && _baseFontSizes != null)
            {
                for (var i = 0; i < textTargets.Length; i++)
                {
                    if (textTargets[i] == null)
                    {
                        continue;
                    }

                    textTargets[i].fontSize = _baseFontSizes[i] * themeSettings.FontScale;
                    textTargets[i].color = themeSettings.GetTextColor();
                }
            }
        }

        protected virtual void Update()
        {
            if (!_isInitialized)
            {
                return;
            }

            UpdateVisibility();
            UpdatePose();
        }

        private void ShowImmediate()
        {
            _isVisible = true;
            _currentVisibility = 1f;
            _targetVisibility = 1f;
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            gameObject.SetActive(true);
        }

        private void UpdateVisibility()
        {
            _currentVisibility = Mathf.MoveTowards(
                _currentVisibility,
                _targetVisibility,
                animationProfile.VisibilityLerpSpeed * Time.deltaTime);

            canvasGroup.alpha = _currentVisibility;
            canvasGroup.interactable = _currentVisibility > 0.95f;
            canvasGroup.blocksRaycasts = _currentVisibility > 0.95f;

            if (!_isVisible && _currentVisibility <= 0.001f)
            {
                gameObject.SetActive(false);
            }
            else if (_isVisible && !gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
        }

        private void UpdatePose()
        {
            var floatingOffset = Mathf.Sin(Time.time * animationProfile.FloatingFrequency + transform.GetInstanceID())
                * animationProfile.FloatingAmplitude;

            if (_gazeFollowEnabled && _gazeAnchor != null)
            {
                var targetWorldPosition = _gazeAnchor.TransformPoint(_gazeFollowOffset) + (_gazeAnchor.up * floatingOffset);
                transform.position = Vector3.Lerp(
                    transform.position,
                    targetWorldPosition,
                    animationProfile.GazeFollowLerpSpeed * Time.deltaTime);
                return;
            }

            transform.localPosition = Vector3.Lerp(
                transform.localPosition,
                _layoutLocalPosition + (Vector3.up * floatingOffset),
                animationProfile.LayoutLerpSpeed * Time.deltaTime);

            transform.localRotation = Quaternion.Slerp(
                transform.localRotation,
                Quaternion.Euler(_layoutLocalEulerAngles),
                animationProfile.LayoutLerpSpeed * Time.deltaTime);
        }

        private void LoadLayout()
        {
            if (_layoutPersistence != null && _layoutPersistence.TryLoad(PanelId, out var layoutData))
            {
                _layoutLocalPosition = layoutData.LocalPosition;
                _layoutLocalEulerAngles = layoutData.LocalEulerAngles;
                return;
            }

            _layoutLocalPosition = defaultLocalPosition;
            _layoutLocalEulerAngles = defaultLocalEulerAngles;
        }

        private void SaveLayout()
        {
            _layoutPersistence?.Save(PanelId, _layoutLocalPosition, _layoutLocalEulerAngles);
        }

        private void SyncLayoutFromTransform()
        {
            _layoutLocalPosition = transform.localPosition;
            _layoutLocalEulerAngles = transform.localEulerAngles;
        }

        private void ApplyLayoutImmediate()
        {
            transform.localPosition = _layoutLocalPosition;
            transform.localRotation = Quaternion.Euler(_layoutLocalEulerAngles);
        }

        private void CacheBaseFontSizes()
        {
            if (textTargets == null)
            {
                _baseFontSizes = System.Array.Empty<float>();
                return;
            }

            _baseFontSizes = new float[textTargets.Length];
            for (var i = 0; i < textTargets.Length; i++)
            {
                _baseFontSizes[i] = textTargets[i] != null ? textTargets[i].fontSize : 0f;
            }
        }
    }
}
