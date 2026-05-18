# ChipOS Quickstart

## Unity version

Use `Unity 2022 LTS` or newer in the `Unity 2022+` line.

## Open the scene

1. Open the repository in Unity Hub.
2. Let Unity finish importing.
3. Open `Assets/ChipOS/Scenes/ChipOS_Main.unity`.

## Enter Play Mode

1. Open the `Game` view.
2. Press the Play button at the top of the Unity Editor.

## Expected result

You should see a floating futuristic HUD in front of the camera:

- a central `HUDCanvas` with time, status, and assistant text
- a left `TaskPanel`
- a right `EnvironmentPanel`

The panels should appear as dark translucent surfaces with cyan and green text accents.

## Basic controls

- `H`: toggle HUD
- `T`: cycle first task state
- `R`: reset layout
- `P`: panic mode
