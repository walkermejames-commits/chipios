# ChipOS Prefab Assembly Notes

These notes are meant to make the current scripts easier to turn into prefabs later. No architecture change is required.

## Recommended prefab candidates

- `ChipOSRoot`
- `ChipPanel`
- `TaskPanel`
- `EnvironmentPanel`

## ChipOSRoot recipe

Create an empty GameObject named `ChipOSRoot` with:

- `ChipOSManager`
- `InputRouter`

Recommended children:

- `ChipPanel`
- `TaskPanel`
- `EnvironmentPanel`
- `DebugOverlayPanel`
- `NotificationPanel`

Inspector notes:

- Keep `ChipOSManager` references local to these child objects.
- Point `gazeAnchor` to the XR camera when available.
- Leave the input backend on keyboard mock for editor testing.

## ChipPanel recipe

Create a world-space Canvas named `ChipPanel` with:

- `Canvas`
- `CanvasGroup`
- `HUDPanel`
- panel background `Image`
- `TimeText`
- `StatusText`
- `AssistantText`

Suggested `HUDPanel` setup:

- `panelId`: `chip`
- background graphics: panel background image
- text targets: all three TMP fields

## TaskPanel recipe

Create a world-space Canvas named `TaskPanel` with:

- `Canvas`
- `CanvasGroup`
- `TaskPanel`
- panel background `Image`
- `TaskBodyText`

Suggested `TaskPanel` setup:

- `panelId`: `tasks`
- background graphics: panel background image
- text targets: `TaskBodyText`
- `taskBodyText`: assign `TaskBodyText`

## EnvironmentPanel recipe

Create a world-space Canvas named `EnvironmentPanel` with:

- `Canvas`
- `CanvasGroup`
- `EnvironmentPanel`
- panel background `Image`
- `EnvironmentBodyText`

Suggested `EnvironmentPanel` setup:

- `panelId`: `environment`
- background graphics: panel background image
- text targets: `EnvironmentBodyText`
- `environmentBodyText`: assign `EnvironmentBodyText`

## Prefab conversion advice

Once one panel looks correct:

1. Drag it into `Assets/ChipOS/Prefabs`.
2. Create variants instead of rebuilding each panel from scratch.
3. Keep `panelId` values unique after duplicating.
4. Recheck all TMP field references after prefab duplication.
