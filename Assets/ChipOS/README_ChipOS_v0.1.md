# ChipOS v0.1 Spatial Workspace Foundation

## Architecture additions

Existing systems are preserved and extended:
- `Core`: `ChipOSManager`, `ContextState`
- `Panels`: `HUDPanel`, `TaskPanel`, `EnvironmentPanel`, `ChipOverlayPanel`
- `Input`: `InputRouter`
- `Spatial`: `SpatialAnchorManager`, `FollowBehaviour`, `WorkspaceLayoutManager`, `LayoutEditMode`
- `Environment`: `EnvironmentScanner` (mock/stub sensor layer)
- `XR`: abstraction interfaces for head pose, anchors, hand tracking, gesture input

## Scene hierarchy (exact setup)

Create scene: `Assets/ChipOS/Scenes/ChipOS_Main.unity`

- `XR Rig` (or camera rig root)
  - `Main Camera`
- `ChipOSRoot`
  - `ChipOSManager` (component)
  - `InputRouter` (component)
  - `SpatialAnchorManager` (component)
  - `WorkspaceLayoutManager` (component)
  - `LayoutEditMode` (component)
  - `EnvironmentScanner` (component)
  - `CameraHeadPoseProvider` (component, point to Main Camera transform)
  - `ChipPanel` (World Space Canvas + `HUDPanel` + optional `FollowBehaviour`)
  - `TaskPanel` (World Space Canvas + `TaskPanel` + optional `FollowBehaviour`)
  - `EnvironmentPanel` (World Space Canvas + `EnvironmentPanel` + optional `FollowBehaviour`)
  - `ChipOverlayPanel` (World Space Canvas + `ChipOverlayPanel` + `FollowBehaviour`)

## TMP wiring

### ChipOSManager
- `timeText`, `statusText`, `assistantText`
- `taskPanel`, `environmentPanel`, `overlayPanel`
- `spatialAnchorManager`

### TaskPanel
- `taskBodyText`

### EnvironmentPanel
- `environmentBodyText`

### ChipOverlayPanel
- `timeText`
- `assistantStatusText`
- `currentTaskText`
- `notificationsText`
- `ambientStateText`
- `canvasGroup`

## Required canvases + object setup

For each panel canvas:
- Render Mode: `World Space`
- Add a `CanvasGroup` for fade/opacity
- Add collider on panel root if you want edit-mode click selection
- Set unique `HUDPanel.panelId` (e.g. `chip.main`, `chip.tasks`, `chip.environment`, `chip.overlay`)
- Pick `HUDPanel.zone`: Left/Right/Center/Follow/Desk/Ceiling

## Spatial features and behavior

- Persistent anchors are saved in local `PlayerPrefs` as JSON by `SpatialAnchorManager`
- Anchor payload stores panel position / rotation / scale, anchor zone, pin state, follow state
- `SaveAnchors()`, `LoadAnchors()`, `ResetAnchors()` exposed for runtime control
- `FollowBehaviour` provides soft head-follow with smoothing and max speed clamp
- Pinning locks panel in physical world; follow can be toggled independently

## Workspace presets

`WorkspaceLayoutManager` supports:
- Focus Mode
- Walking Mode
- Desk Mode
- Minimal Mode
- Panic Mode

Transitions are eased/animated and active layout is persisted through `SpatialAnchorManager.ActiveLayoutName`.

## HUD edit mode

`LayoutEditMode`:
- Toggle edit mode: `E`
- Click panel collider to select
- Move selected panel: Arrow/WASD axes
- Rotate selected panel: hold `Q` / `E`
- Scale selected panel: mouse wheel
- Optional snap-to-grid
- Draws cyan gizmo around selected panel
- Auto-saves anchors during editing

## Keyboard controls

- `H` Toggle HUD visibility
- `R` Reset panel layouts + anchors
- `P` Panic mode
- `T` Cycle primary task
- `K` Save anchor layout
- `F1` Focus Mode
- `F2` Walking Mode
- `F3` Desk Mode
- `F4` Minimal Mode
- `F5` Panic Mode layout

## Prefab structure recommendation

Create reusable prefabs under `Assets/ChipOS/Prefabs/`:
- `PF_ChipPanel`
- `PF_TaskPanel`
- `PF_EnvironmentPanel`
- `PF_ChipOverlayPanel`

Each prefab should include:
- World-space canvas root
- Assigned panel script
- `HUDPanel` identity + zone
- Optional `FollowBehaviour`
- `CanvasGroup`
- TMP child labels

## XR / NRSDK future expansion points

No direct NRSDK dependency yet. Interfaces are prepared:
- `IHeadPoseProvider`
- `ISpatialAnchorProvider`
- `IHandTrackingProvider`
- `IGestureInputProvider`

Swap mock/default providers with XREAL/NRSDK adapters later without replacing panel/core architecture.

## Test checklist

1. Press Play and verify all panels initialize and render.
2. Move panels, press `K`, restart Play, verify anchors restore.
3. Toggle follow/pin on a panel with `FollowBehaviour` and verify smooth motion.
4. Press `F1-F5` and verify animated layout transitions.
5. Enable edit mode (`E`) and verify move/rotate/scale + snap behavior.
6. Verify overlay pulse alpha and compact/expanded toggling hook.
7. Verify mock environment values update each frame.
