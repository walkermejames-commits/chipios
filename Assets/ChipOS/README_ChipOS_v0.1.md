# ChipOS v0.2 Spatial Context Layer (Unity + Android + XREAL Air 2 Ultra)

## Folder structure

- `Assets/ChipOS/Scripts/Core/ChipOSManager.cs`
- `Assets/ChipOS/Scripts/Core/ContextState.cs`
- `Assets/ChipOS/Scripts/UI/Panels/HUDPanel.cs`
- `Assets/ChipOS/Scripts/UI/Panels/TaskPanel.cs`
- `Assets/ChipOS/Scripts/UI/Panels/EnvironmentPanel.cs`
- `Assets/ChipOS/Scripts/UI/Panels/DebugOverlayPanel.cs`
- `Assets/ChipOS/Scripts/UI/Notifications/NotificationPanel.cs`
- `Assets/ChipOS/Scripts/UI/Theme/ChipOSThemeSettings.cs`
- `Assets/ChipOS/Scripts/UI/Theme/ChipOSThemeController.cs`
- `Assets/ChipOS/Scripts/Services/*`
- `Assets/ChipOS/Scripts/Persistence/PanelLayoutPersistence.cs`
- `Assets/ChipOS/Scripts/Animations/PanelAnimationProfile.cs`
- `Assets/ChipOS/Scripts/XRAdapters/InputRouter.cs`
- `Assets/ChipOS/Scripts/XRAdapters/*`

## Scene setup (exact placement)

1. Create a scene `Assets/ChipOS/Scenes/ChipOS_Main.unity`.
2. Add an `XR Rig` or `NRSDK` camera rig depending on your XREAL stack.
3. Under the camera or a world anchor, create an empty `ChipOSRoot` GameObject.
4. Add `ChipOSManager` and `InputRouter` to `ChipOSRoot`.
5. Assign `gazeAnchor` on `ChipOSManager` to the XR camera transform. If left empty, `Camera.main` is used at runtime.
6. Create five world-space canvases as children of `ChipOSRoot`:
   - `ChipPanel`
   - `TaskPanel`
   - `EnvironmentPanel`
   - `DebugOverlayPanel`
   - `NotificationPanel`
7. Add `HUDPanel` to `ChipPanel`.
8. Add `TaskPanel` to `TaskPanel`.
9. Add `EnvironmentPanel` to `EnvironmentPanel`.
10. Add `DebugOverlayPanel` to `DebugOverlayPanel`.
11. Add `NotificationPanel` to `NotificationPanel`.
12. Add `CanvasGroup` components to each panel root for fade animation.
13. Use TextMeshPro `TMP_Text` labels with large font sizes, typically `36-56`, for AR readability.
14. Wire `ChipOSManager` references for:
   - `chipPanel`
   - `taskPanel`
   - `environmentPanel`
   - `debugOverlayPanel`
   - `notificationPanel`
   - `timeText`
   - `statusText`
   - `assistantText`
15. On each `HUDPanel`-derived panel, assign:
   - a stable `panelId`
   - panel background `Graphic` references
   - panel `TMP_Text` references
   - optional `focusHighlight`

## Chip Panel content

Add three TMP text fields to `ChipPanel` and wire them in `ChipOSManager`:
- `timeText`: live clock
- `statusText`: e.g. `Status: Running | Mon 18 May`
- `assistantText`: should show `ChipOS online`

## Task Panel content

Add one TMP body text field and wire it to `TaskPanel.taskBodyText`.
Expected default tasks:
- Finish Door in Four driver flow `[Now]`
- Test XREAL display mode `[Next]`
- Write one AR OS note `[Parked]`

Press `T` to cycle the first task status.

## Environment Panel content

Add one TMP body text field and wire it to `EnvironmentPanel.environmentBodyText`.
Expected runtime values:
- Mode: Desk
- Tracking: Checking
- Input: Keyboard Mock
- Network: Wi-Fi / Cellular / Offline
- Battery: device battery summary
- Weather: mock weather summary

## Debug Overlay content

Add one TMP body text field and wire it to `DebugOverlayPanel.debugText`.
Expected content:
- focused panel id
- gaze-follow state
- theme values
- time/date
- service summaries

## Notification Panel content

Add one TMP text field and wire it to `NotificationPanel.messageText`.
Recommended usage:
- place it high in the user view
- keep width generous
- use the same background treatment as the rest of the system

## Input mapping (testing)

- `H` = Toggle all HUD panels
- `R` = Reset all panel positions
- `P` = Panic mode
- `T` = Cycle first task status
- `G` = Toggle gaze-follow mode
- `[` / `]` = Focus previous / next panel
- `Arrow Keys` = Move focused panel left / right / up / down
- `PageUp` / `PageDown` = Move focused panel forward / back
- `F1` = Toggle debug overlay
- `C` = Toggle accessibility contrast mode
- `-` / `=` = Decrease / increase font scale
- `N` = Trigger a test notification

## Visual style recommendations

- Use dark translucent panel backgrounds, e.g. RGBA `#10141ACC`
- Keep text high contrast with off-white and cyan accents
- Use rounded corners and a restrained glow
- Keep line spacing generous to reduce eye strain in glasses
- Keep panel opacity around `0.75-0.85` until you validate against the real XREAL optical stack

## XREAL / NRSDK integration notes

- Keep `ChipOSRoot` anchored in front of the camera for initial stability.
- `InputRouter` now targets `IChipInputSource`, so NRSDK hand/gaze input should land in a concrete adapter instead of in manager code.
- `NRSDKInputSourceStub` keeps the seam in place without taking a hard dependency yet.
- `ContextState` is still intentionally lightweight. Replace mock weather and placeholder tracking values with device or Android bridge adapters when that layer is ready.

## First test checklist

1. Enter Play Mode and confirm the main, task, and environment panels are visible.
2. Confirm the clock and date update automatically.
3. Press `]`, move the focused panel with arrow keys, stop Play Mode, restart, and verify the layout persists.
4. Press `G` and confirm the panels smoothly follow the gaze anchor while retaining relative offsets.
5. Press `F1` and verify the debug overlay can be shown and hidden.
6. Press `N` and verify the notification panel fades in and out.
7. Press `C` and `-` / `=` to confirm theme changes propagate across the panels.
8. Press `H`, `P`, and `R` to verify visibility and reset flows still work.
9. Build the Android player with the XR/NRSDK pipeline enabled and verify readable panel size and comfortable panel motion on XREAL Air 2 Ultra.
