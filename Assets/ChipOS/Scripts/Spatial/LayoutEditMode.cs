using ChipOS.Panels;
using UnityEngine;

namespace ChipOS.Spatial
{
    public class LayoutEditMode : MonoBehaviour
    {
        [SerializeField] private SpatialAnchorManager anchorManager;
        [SerializeField] private Camera sceneCamera;
        [SerializeField] private bool snapToGrid = true;
        [SerializeField] private float gridStep = 0.05f;
        [SerializeField] private KeyCode toggleKey = KeyCode.E;

        private bool _isEditMode;
        private HUDPanel _selected;

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(toggleKey))
                _isEditMode = !_isEditMode;

            if (!_isEditMode) return;

            if (UnityEngine.Input.GetMouseButtonDown(0))
                TrySelectPanel();

            if (_selected == null) return;

            var move = new Vector3(UnityEngine.Input.GetAxis("Horizontal"), UnityEngine.Input.GetAxis("Vertical"), 0f) * Time.deltaTime;
            _selected.transform.position += move;

            if (UnityEngine.Input.GetKey(KeyCode.Q)) _selected.transform.Rotate(Vector3.up, -35f * Time.deltaTime, Space.World);
            if (UnityEngine.Input.GetKey(KeyCode.E)) _selected.transform.Rotate(Vector3.up, 35f * Time.deltaTime, Space.World);

            var scaleDelta = UnityEngine.Input.mouseScrollDelta.y * 0.02f;
            if (Mathf.Abs(scaleDelta) > 0.0001f)
                _selected.transform.localScale += Vector3.one * scaleDelta;

            if (snapToGrid)
                _selected.transform.position = Snap(_selected.transform.position);

            anchorManager?.SaveAnchors();
        }

        private void TrySelectPanel()
        {
            if (sceneCamera == null) sceneCamera = Camera.main;
            var ray = sceneCamera.ScreenPointToRay(UnityEngine.Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
                _selected = hit.collider.GetComponentInParent<HUDPanel>();
        }

        private Vector3 Snap(Vector3 value)
        {
            return new Vector3(
                Mathf.Round(value.x / gridStep) * gridStep,
                Mathf.Round(value.y / gridStep) * gridStep,
                Mathf.Round(value.z / gridStep) * gridStep);
        }

        private void OnDrawGizmos()
        {
            if (!_isEditMode || _selected == null) return;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(_selected.transform.position, Vector3.one * 0.2f);
        }
    }
}
