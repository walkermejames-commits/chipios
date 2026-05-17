# ChipOS v0.1 Context Layer (Unity + Android + XREAL Air 2 Ultra)

## Folder structure

- `Assets/ChipOS/Scripts/Core/ChipOSManager.cs`
- `Assets/ChipOS/Scripts/Core/ContextState.cs`
- `Assets/ChipOS/Scripts/Panels/HUDPanel.cs`
- `Assets/ChipOS/Scripts/Panels/TaskPanel.cs`
- `Assets/ChipOS/Scripts/Panels/EnvironmentPanel.cs`
- `Assets/ChipOS/Scripts/Input/InputRouter.cs`

## Scene setup (exact placement)

1. Create a scene `Assets/ChipOS/Scenes/ChipOS_Main.unity`.
2. Add an `XR Rig` or `NRSDK` camera rig (depending on your XREAL stack).
3. Under the camera (or a world anchor), create an empty `ChipOSRoot` GameObject.
4. Add `ChipOSManager` and `InputRouter` to `ChipOSRoot`.
5. Create three world-space canvases as children of `ChipOSRoot`:
   - `ChipPanel` (main)
   - `TaskPanel`
   - `EnvironmentPanel`
6. Add `HUDPanel` to `ChipPanel`.
7. Add `TaskPanel` script to `TaskPanel` object.
8. Add `EnvironmentPanel` script to `EnvironmentPanel` object.
9. Use TextMeshPro (`TMP_Text`) labels with large font sizes (36-56 recommended for AR readability).

## Chip Panel content

Add three TMP text fields to ChipPanel and wire in `ChipOSManager`:
- `timeText`: live clock
- `statusText`: e.g. "Status: Running"
- `assistantText`: should show "ChipOS online"

## Task Panel content

Add one TMP body text field and wire to `TaskPanel.taskBodyText`.
Expected default tasks:
- Finish Door in Four driver flow [Now]
- Test XREAL display mode [Next]
- Write one AR OS note [Parked]

Press `T` to cycle the first task status.

## Environment Panel content

Add one TMP body text field and wire to `EnvironmentPanel.environmentBodyText`.
Expected defaults:
- Mode: Desk
- Tracking: Checking
- Input: Basic
- Network: Unknown

## Input mapping (testing)

- `H` = Toggle all HUD panels
- `R` = Reset all panel positions
- `P` = Panic mode (hide everything instantly)
- `T` = Cycle first task status

## Visual style recommendations

- Use dark translucent panel backgrounds (e.g. RGBA `#10141ACC`)
- High contrast text (off-white/cyan accents)
- Rounded corners and subtle glow
- Spacing between lines to reduce eye strain in glasses

## XREAL / NRSDK integration notes

- Keep `ChipOSRoot` anchored in front of camera for initial stability.
- Replace keyboard input in `InputRouter` with NRSDK hand/gaze events later.
- Replace `ContextState` placeholder values with device/services adapters when ready.

## First test checklist

1. Enter Play Mode and confirm all 3 panels are visible.
2. Confirm Chip Panel clock updates every second.
3. Press `H`, verify all panels hide/show.
4. Press `P`, verify instant hide.
5. Press `R`, verify panel transforms return to defaults.
6. Press `T`, verify first task cycles Now -> Next -> Parked.
7. Build Android player with XR/NRSDK pipeline enabled and verify readable panel size on XREAL Air 2 Ultra.
