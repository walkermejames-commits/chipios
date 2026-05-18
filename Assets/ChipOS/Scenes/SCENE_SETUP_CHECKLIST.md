# ChipOS Scene Setup Checklist

This checklist is written for non-experts. Follow it in order.

## Before you start

1. Open the project in `Unity 2022 LTS` or newer.
2. Wait for Unity to finish importing everything.
3. Make sure `TextMeshPro` resources are imported if Unity asks.

## Create the scene

1. In the Project window, open `Assets/ChipOS/Scenes`.
2. Right-click and create a new Scene named `ChipOS_Main`.
3. Open that scene.
4. Save the scene immediately.

## Add the camera or XR rig

1. If you already use an XR rig prefab in your project, place it in the scene now.
2. If not, add a standard `Camera` so you can test basic world-space UI in Editor Play Mode.
3. Place the camera at a comfortable default position, for example:
   - Position: `0, 1.6, 0`
   - Rotation: `0, 0, 0`

## Create the ChipOS root

1. Create an empty GameObject.
2. Rename it to `ChipOSRoot`.
3. Add these components:
   - `ChipOSManager`
   - `InputRouter`
4. Keep `ChipOSRoot` near the camera for early testing.

## Create the core panels

Create these child objects under `ChipOSRoot`:

1. `ChipPanel`
2. `TaskPanel`
3. `EnvironmentPanel`

For each one:

1. Add a `Canvas`.
2. Set `Render Mode` to `World Space`.
3. Add a `CanvasGroup`.
4. Add a background `Image`.
5. Size the panel so text is readable in AR.

## Add the required panel scripts

- Add `HUDPanel` to `ChipPanel`
- Add `TaskPanel` to `TaskPanel`
- Add `EnvironmentPanel` to `EnvironmentPanel`

## Add the text content

### ChipPanel

Create three TextMeshPro text objects:

1. `TimeText`
2. `StatusText`
3. `AssistantText`

### TaskPanel

Create one larger TextMeshPro body text object:

1. `TaskBodyText`

### EnvironmentPanel

Create one larger TextMeshPro body text object:

1. `EnvironmentBodyText`

## Optional but recommended

Create these extra panels under `ChipOSRoot`:

1. `DebugOverlayPanel`
2. `NotificationPanel`

Add:

- `DebugOverlayPanel` script to `DebugOverlayPanel`
- `NotificationPanel` script to `NotificationPanel`
- `CanvasGroup` to both

## Wire the manager in Inspector

On `ChipOSRoot`, assign these fields on `ChipOSManager`:

1. `chipPanel` -> `ChipPanel`
2. `taskPanel` -> `TaskPanel`
3. `environmentPanel` -> `EnvironmentPanel`
4. `debugOverlayPanel` -> optional `DebugOverlayPanel`
5. `notificationPanel` -> optional `NotificationPanel`
6. `timeText` -> `TimeText`
7. `statusText` -> `StatusText`
8. `assistantText` -> `AssistantText`
9. `gazeAnchor` -> your XR camera transform, or leave blank to use `Camera.main`

## Wire panel fields

On each HUD-derived panel:

1. Set a unique `panelId`
2. Assign `backgroundGraphics`
3. Assign `textTargets`
4. Assign `focusHighlight` if you created one

Also wire:

- `TaskPanel.taskBodyText`
- `EnvironmentPanel.environmentBodyText`
- `DebugOverlayPanel.debugText` if used
- `NotificationPanel.messageText` if used

## First Play Mode test

1. Press Play.
2. Confirm the main panel clock updates.
3. Confirm task and environment text appears.
4. Press `H` to hide and show the HUD.
5. Press `T` to change the first task state.
6. Press `]` and move a panel with arrow keys.
7. Press `R` to reset layout.
8. Press `N` to show a notification.

## If something does not appear

1. Check that the Canvas is set to `World Space`.
2. Check that text objects are actually assigned in Inspector.
3. Check that the panel is in front of the camera.
4. Check the Console for script compile errors.
5. Check that the object has a `CanvasGroup`.
