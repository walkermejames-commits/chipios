using System;
using System.Reflection;
using ChipOS.Core;
using ChipOS.UI.Panels;
using ChipOS.XRAdapters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChipOS.Bootstrap
{
    /// <summary>
    /// Lightweight runtime scaffold for the first runnable ChipOS scene and prefab shells.
    /// It assembles readable world-space UI without changing the existing runtime architecture.
    /// </summary>
    [DisallowMultipleComponent]
    public class ChipOSSceneScaffold : MonoBehaviour
    {
        public enum ScaffoldMode
        {
            Root,
            HudCanvas,
            TaskPanel,
            EnvironmentPanel
        }

        [SerializeField] private ScaffoldMode scaffoldMode = ScaffoldMode.Root;
        [SerializeField] private bool buildOnAwake = true;

        private static readonly Color PanelBackgroundColor = new(0.05f, 0.08f, 0.12f, 0.86f);
        private static readonly Color PanelBorderColor = new(0.18f, 0.86f, 0.94f, 0.95f);
        private static readonly Color PrimaryTextColor = new(0.73f, 0.97f, 1f, 1f);
        private static readonly Color AccentTextColor = new(0.57f, 1f, 0.75f, 1f);
        private static readonly Color SecondaryTextColor = new(0.82f, 0.90f, 0.96f, 1f);

        private void Awake()
        {
            if (buildOnAwake)
            {
                Build();
            }
        }

        [ContextMenu("Build Scaffold")]
        public void Build()
        {
            switch (scaffoldMode)
            {
                case ScaffoldMode.Root:
                    BuildRoot();
                    break;
                case ScaffoldMode.HudCanvas:
                    BuildHudCanvas(gameObject);
                    break;
                case ScaffoldMode.TaskPanel:
                    BuildTaskPanel(gameObject);
                    break;
                case ScaffoldMode.EnvironmentPanel:
                    BuildEnvironmentPanel(gameObject);
                    break;
            }
        }

        private void BuildRoot()
        {
            name = "ChipOSRoot";

            var origin = transform.parent;
            var cameraTransform = ResolveCameraTransform(origin);

            if (origin == null)
            {
                transform.position = new Vector3(0f, 1.6f, 0f);
            }
            else if (cameraTransform != null && cameraTransform.parent == origin)
            {
                transform.localPosition = new Vector3(0f, cameraTransform.localPosition.y, 0f);
            }

            var chipOSManager = GetOrAddComponent<ChipOSManager>(gameObject);
            GetOrAddComponent<InputRouter>(gameObject);

            var hudCanvas = FindOrCreateChild("HUDCanvas");
            BuildHudCanvas(hudCanvas);
            hudCanvas.transform.SetParent(transform, false);

            var taskPanel = FindOrCreateChild("TaskPanel");
            BuildTaskPanel(taskPanel);
            taskPanel.transform.SetParent(transform, false);

            var environmentPanel = FindOrCreateChild("EnvironmentPanel");
            BuildEnvironmentPanel(environmentPanel);
            environmentPanel.transform.SetParent(transform, false);

            AssignManagerFields(chipOSManager, hudCanvas, taskPanel, environmentPanel, cameraTransform);
        }

        private void BuildHudCanvas(GameObject panelRoot)
        {
            panelRoot.name = "HUDCanvas";
            EnsurePanelTransform(panelRoot, new Vector2(720f, 300f), new Vector3(0f, 0.14f, 1.2f), Vector3.zero, new Vector3(0.0018f, 0.0018f, 0.0018f));

            var hudPanel = GetOrAddComponent<HUDPanel>(panelRoot);
            ConfigureHudPanelDefaults(hudPanel, "chip", new Vector3(0f, 0.14f, 1.2f));

            var background = EnsureBackground(panelRoot.transform, "PanelBackground", PanelBackgroundColor, new Vector2(706f, 286f));
            var border = EnsureBorder(panelRoot.transform, "PanelBorder", PanelBorderColor, new Vector2(720f, 300f), 5f);
            var header = EnsureBackground(panelRoot.transform, "HeaderStrip", new Color(0.08f, 0.24f, 0.28f, 0.95f), new Vector2(720f, 48f));
            SetTopStripRect(header.rectTransform, 48f);

            var timeText = EnsureText(panelRoot.transform, "TimeText", "00:00:00", PrimaryTextColor, 52f, FontStyles.Bold);
            SetRect(timeText.rectTransform, new Vector2(28f, -38f), new Vector2(320f, 84f), TextAlignmentOptions.Left);

            var statusText = EnsureText(panelRoot.transform, "StatusText", "Status: Running", AccentTextColor, 28f, FontStyles.Normal);
            SetRect(statusText.rectTransform, new Vector2(28f, -102f), new Vector2(600f, 50f), TextAlignmentOptions.Left);

            var assistantText = EnsureText(panelRoot.transform, "AssistantText", "ChipOS online", SecondaryTextColor, 24f, FontStyles.Normal);
            SetRect(assistantText.rectTransform, new Vector2(28f, -154f), new Vector2(640f, 96f), TextAlignmentOptions.TopLeft);

            AssignPanelVisualFields(hudPanel, background, timeText, statusText, assistantText, border);
        }

        private void BuildTaskPanel(GameObject panelRoot)
        {
            panelRoot.name = "TaskPanel";
            EnsurePanelTransform(panelRoot, new Vector2(520f, 360f), new Vector3(-0.42f, -0.08f, 1.28f), new Vector3(0f, 7f, 0f), new Vector3(0.0017f, 0.0017f, 0.0017f));

            var taskPanel = GetOrAddComponent<TaskPanel>(panelRoot);
            ConfigureHudPanelDefaults(taskPanel, "tasks", new Vector3(-0.42f, -0.08f, 1.28f), new Vector3(0f, 7f, 0f));

            var background = EnsureBackground(panelRoot.transform, "PanelBackground", PanelBackgroundColor, new Vector2(508f, 348f));
            var border = EnsureBorder(panelRoot.transform, "PanelBorder", PanelBorderColor, new Vector2(520f, 360f), 4f);
            var title = EnsureText(panelRoot.transform, "TaskTitle", "TASK STACK", AccentTextColor, 26f, FontStyles.Bold);
            SetRect(title.rectTransform, new Vector2(26f, -24f), new Vector2(440f, 40f), TextAlignmentOptions.Left);

            var taskBodyText = EnsureText(
                panelRoot.transform,
                "TaskBodyText",
                "• Finish Door in Four driver flow [Now]\n• Test XREAL display mode [Next]\n• Write one AR OS note [Parked]",
                PrimaryTextColor,
                24f,
                FontStyles.Normal);
            SetRect(taskBodyText.rectTransform, new Vector2(26f, -72f), new Vector2(460f, 240f), TextAlignmentOptions.TopLeft);

            AssignPanelVisualFields(taskPanel, background, title, taskBodyText, border);
            SetPrivateField(taskPanel, "taskBodyText", taskBodyText);
        }

        private void BuildEnvironmentPanel(GameObject panelRoot)
        {
            panelRoot.name = "EnvironmentPanel";
            EnsurePanelTransform(panelRoot, new Vector2(520f, 360f), new Vector3(0.42f, -0.08f, 1.28f), new Vector3(0f, -7f, 0f), new Vector3(0.0017f, 0.0017f, 0.0017f));

            var environmentPanel = GetOrAddComponent<EnvironmentPanel>(panelRoot);
            ConfigureHudPanelDefaults(environmentPanel, "environment", new Vector3(0.42f, -0.08f, 1.28f), new Vector3(0f, -7f, 0f));

            var background = EnsureBackground(panelRoot.transform, "PanelBackground", PanelBackgroundColor, new Vector2(508f, 348f));
            var border = EnsureBorder(panelRoot.transform, "PanelBorder", new Color(0.23f, 0.94f, 0.70f, 0.95f), new Vector2(520f, 360f), 4f);
            var title = EnsureText(panelRoot.transform, "EnvironmentTitle", "ENVIRONMENT", AccentTextColor, 26f, FontStyles.Bold);
            SetRect(title.rectTransform, new Vector2(26f, -24f), new Vector2(440f, 40f), TextAlignmentOptions.Left);

            var environmentBodyText = EnsureText(
                panelRoot.transform,
                "EnvironmentBodyText",
                "Mode: Desk\nTracking: Checking\nInput: Keyboard Mock\nNetwork: Unknown",
                SecondaryTextColor,
                24f,
                FontStyles.Normal);
            SetRect(environmentBodyText.rectTransform, new Vector2(26f, -72f), new Vector2(460f, 240f), TextAlignmentOptions.TopLeft);

            AssignPanelVisualFields(environmentPanel, background, title, environmentBodyText, border);
            SetPrivateField(environmentPanel, "environmentBodyText", environmentBodyText);
        }

        private void AssignManagerFields(
            ChipOSManager chipOSManager,
            GameObject hudCanvas,
            GameObject taskPanel,
            GameObject environmentPanel,
            Transform gazeAnchor)
        {
            SetPrivateField(chipOSManager, "chipPanel", hudCanvas.GetComponent<HUDPanel>());
            SetPrivateField(chipOSManager, "taskPanel", taskPanel.GetComponent<TaskPanel>());
            SetPrivateField(chipOSManager, "environmentPanel", environmentPanel.GetComponent<EnvironmentPanel>());
            SetPrivateField(chipOSManager, "timeText", FindRequiredText(hudCanvas.transform, "TimeText"));
            SetPrivateField(chipOSManager, "statusText", FindRequiredText(hudCanvas.transform, "StatusText"));
            SetPrivateField(chipOSManager, "assistantText", FindRequiredText(hudCanvas.transform, "AssistantText"));
            SetPrivateField(chipOSManager, "gazeAnchor", gazeAnchor);
        }

        private static Transform ResolveCameraTransform(Transform origin)
        {
            if (Camera.main != null)
            {
                return Camera.main.transform;
            }

            if (origin != null)
            {
                var namedCamera = origin.Find("Main Camera");
                if (namedCamera != null)
                {
                    return namedCamera;
                }
            }

            return null;
        }

        private static TMP_Text FindRequiredText(Transform parent, string name)
        {
            var child = parent.Find(name);
            return child != null ? child.GetComponent<TMP_Text>() : null;
        }

        private static void AssignPanelVisualFields(HUDPanel hudPanel, Graphic background, TMP_Text title, TMP_Text body, Graphic focusHighlight)
        {
            SetPrivateField(hudPanel, "backgroundGraphics", new[] { background });
            SetPrivateField(hudPanel, "textTargets", new[] { title, body });
            SetPrivateField(hudPanel, "focusHighlight", focusHighlight);
        }

        private static void AssignPanelVisualFields(HUDPanel hudPanel, Graphic background, TMP_Text first, TMP_Text second, TMP_Text third, Graphic focusHighlight)
        {
            SetPrivateField(hudPanel, "backgroundGraphics", new[] { background });
            SetPrivateField(hudPanel, "textTargets", new[] { first, second, third });
            SetPrivateField(hudPanel, "focusHighlight", focusHighlight);
        }

        private static void ConfigureHudPanelDefaults(HUDPanel hudPanel, string panelId, Vector3 localPosition)
        {
            ConfigureHudPanelDefaults(hudPanel, panelId, localPosition, Vector3.zero);
        }

        private static void ConfigureHudPanelDefaults(HUDPanel hudPanel, string panelId, Vector3 localPosition, Vector3 localEulerAngles)
        {
            SetPrivateField(hudPanel, "panelId", panelId);
            SetPrivateField(hudPanel, "defaultLocalPosition", localPosition);
            SetPrivateField(hudPanel, "defaultLocalEulerAngles", localEulerAngles);
        }

        private static void EnsurePanelTransform(GameObject panelRoot, Vector2 size, Vector3 localPosition, Vector3 localEulerAngles, Vector3 localScale)
        {
            var canvas = GetOrAddComponent<Canvas>(panelRoot);
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
            canvas.pixelPerfect = false;

            GetOrAddComponent<CanvasGroup>(panelRoot);
            GetOrAddComponent<GraphicRaycaster>(panelRoot);

            var scaler = GetOrAddComponent<CanvasScaler>(panelRoot);
            scaler.dynamicPixelsPerUnit = 10f;
            scaler.referencePixelsPerUnit = 100f;

            var rectTransform = panelRoot.GetComponent<RectTransform>();

            rectTransform.sizeDelta = size;
            rectTransform.localPosition = localPosition;
            rectTransform.localRotation = Quaternion.Euler(localEulerAngles);
            rectTransform.localScale = localScale;
        }

        private static void SetTopStripRect(RectTransform rectTransform, float height)
        {
            rectTransform.anchorMin = new Vector2(0f, 1f);
            rectTransform.anchorMax = new Vector2(1f, 1f);
            rectTransform.pivot = new Vector2(0.5f, 1f);
            rectTransform.offsetMin = new Vector2(0f, -height);
            rectTransform.offsetMax = Vector2.zero;
        }

        private static void SetRect(RectTransform rectTransform, Vector2 anchoredPosition, Vector2 size, TextAlignmentOptions alignment)
        {
            rectTransform.anchorMin = new Vector2(0f, 1f);
            rectTransform.anchorMax = new Vector2(0f, 1f);
            rectTransform.pivot = new Vector2(0f, 1f);
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.sizeDelta = size;

            if (rectTransform.TryGetComponent<TextMeshProUGUI>(out var tmp))
            {
                tmp.alignment = alignment;
            }
        }

        private static Image EnsureBackground(Transform parent, string name, Color color, Vector2 size)
        {
            var go = FindOrCreateChild(parent, name);
            var image = GetOrAddComponent<Image>(go);
            image.color = color;

            var rectTransform = go.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.sizeDelta = size;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.localScale = Vector3.one;

            return image;
        }

        private static Image EnsureBorder(Transform parent, string name, Color color, Vector2 size, float thickness)
        {
            var border = EnsureBackground(parent, name, color, size);
            border.raycastTarget = false;

            var inner = FindOrCreateChild(border.transform, "InnerMask");
            var innerImage = GetOrAddComponent<Image>(inner);
            innerImage.color = Color.clear;

            var innerRect = inner.GetComponent<RectTransform>();
            innerRect.anchorMin = new Vector2(0.5f, 0.5f);
            innerRect.anchorMax = new Vector2(0.5f, 0.5f);
            innerRect.pivot = new Vector2(0.5f, 0.5f);
            innerRect.sizeDelta = new Vector2(size.x - (thickness * 2f), size.y - (thickness * 2f));
            innerRect.anchoredPosition = Vector2.zero;
            innerRect.localScale = Vector3.one;

            return border;
        }

        private static TextMeshProUGUI EnsureText(
            Transform parent,
            string name,
            string defaultText,
            Color color,
            float fontSize,
            FontStyles fontStyle)
        {
            var go = FindOrCreateChild(parent, name);
            var text = GetOrAddComponent<TextMeshProUGUI>(go);
            text.text = defaultText;
            text.color = color;
            text.fontSize = fontSize;
            text.fontStyle = fontStyle;
            text.enableWordWrapping = true;
            text.richText = false;
            text.raycastTarget = false;
            text.margin = new Vector4(0f, 0f, 0f, 0f);

            if (TMP_Settings.defaultFontAsset != null)
            {
                text.font = TMP_Settings.defaultFontAsset;
            }

            return text;
        }

        private GameObject FindOrCreateChild(string childName)
        {
            return FindOrCreateChild(transform, childName);
        }

        private static GameObject FindOrCreateChild(Transform parent, string childName)
        {
            var child = parent.Find(childName);
            if (child != null)
            {
                return child.gameObject;
            }

            var go = new GameObject(childName, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            return go;
        }

        private static T GetOrAddComponent<T>(GameObject target) where T : Component
        {
            var component = target.GetComponent<T>();
            return component != null ? component : target.AddComponent<T>();
        }

        private static void SetPrivateField<TTarget, TValue>(TTarget target, string fieldName, TValue value)
        {
            if (target == null)
            {
                return;
            }

            var searchType = target.GetType();
            FieldInfo field = null;

            while (searchType != null && field == null)
            {
                field = searchType.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
                searchType = searchType.BaseType;
            }

            field?.SetValue(target, value);
        }
    }
}
