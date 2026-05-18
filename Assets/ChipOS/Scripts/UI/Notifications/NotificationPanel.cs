using ChipOS.UI.Theme;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChipOS.UI.Notifications
{
    /// <summary>
    /// Simple transient notification presenter for lightweight AR system feedback.
    /// </summary>
    public class NotificationPanel : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TMP_Text messageText;
        [SerializeField] private Graphic[] backgroundGraphics;
        [SerializeField] private float fadeInSpeed = 8f;
        [SerializeField] private float fadeOutSpeed = 4f;
        [SerializeField] private float defaultDuration = 2.25f;

        private float _targetAlpha;
        private float _remainingDuration;
        private float _baseFontSize;

        public void ShowMessage(string message, float duration = -1f)
        {
            if (canvasGroup == null || messageText == null)
            {
                return;
            }

            messageText.text = message;
            _remainingDuration = duration > 0f ? duration : defaultDuration;
            _targetAlpha = 1f;
            gameObject.SetActive(true);
        }

        public void ApplyTheme(ChipOSThemeSettings themeSettings)
        {
            if (themeSettings == null)
            {
                return;
            }

            if (_baseFontSize <= 0f && messageText != null)
            {
                _baseFontSize = messageText.fontSize;
            }

            if (backgroundGraphics != null)
            {
                for (var i = 0; i < backgroundGraphics.Length; i++)
                {
                    if (backgroundGraphics[i] != null)
                    {
                        backgroundGraphics[i].color = themeSettings.GetPanelColor();
                    }
                }
            }

            if (messageText != null)
            {
                messageText.color = themeSettings.GetTextColor();
                messageText.fontSize = Mathf.Max(24f, _baseFontSize * themeSettings.FontScale);
            }
        }

        private void Awake()
        {
            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
            }

            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
            }

            if (messageText != null)
            {
                _baseFontSize = messageText.fontSize;
            }
        }

        private void Update()
        {
            if (canvasGroup == null)
            {
                return;
            }

            if (_remainingDuration > 0f)
            {
                _remainingDuration -= Time.deltaTime;
                _targetAlpha = 1f;
            }
            else
            {
                _targetAlpha = 0f;
            }

            var speed = _targetAlpha > canvasGroup.alpha ? fadeInSpeed : fadeOutSpeed;
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, _targetAlpha, speed * Time.deltaTime);

            if (_targetAlpha <= 0f && canvasGroup.alpha <= 0.001f)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
