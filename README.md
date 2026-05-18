# ChipOS

ChipOS is a Unity-based AR operating system prototype targeting Android head-worn display workflows, specifically XREAL Air 2 Ultra class hardware. The current repository contains the early context layer and spatial panel scaffolding for a world-space HUD, task surface, environment surface, notifications, and input abstraction.

## Target stack

- Unity target: `Unity 2022 LTS or newer`
- Platform target: `Android`
- Device target: `XREAL Air 2 Ultra`
- XR integration status: `abstracted for now`, with NRSDK-facing seams already present in `Assets/ChipOS/Scripts/XRAdapters`

## Project layout

- `Assets/ChipOS/Scripts`: runtime scripts
- `Assets/ChipOS/Scenes`: scene files and scene setup notes
- `Assets/ChipOS/Prefabs`: prefab placeholders and assembly notes
- `Assets/ChipOS/Materials`: material placeholders and notes
- `Assets/ChipOS/README_ChipOS_v0.1.md`: in-project ChipOS scene wiring reference

## How to open the project

1. Open `Unity Hub`.
2. Choose `Add project from disk`.
3. Select the repository root: `walkermejames-commits/chipios`.
4. Open the project using `Unity 2022 LTS` or a newer compatible `Unity 2022+` editor.
5. Let Unity finish importing packages, scripts, and TMP essentials.
6. If Unity prompts you to import `TextMeshPro` resources, accept it.

## First test steps

1. In the Project window, open the checklist at `Assets/ChipOS/Scenes/SCENE_SETUP_CHECKLIST.md`.
2. Create a new scene named `ChipOS_Main` inside `Assets/ChipOS/Scenes`.
3. Assemble the basic hierarchy using `Assets/ChipOS/Prefabs/PREFAB_ASSEMBLY_NOTES.md`.
4. Add a camera or XR rig, then create `ChipOSRoot` and the three core world-space panels.
5. Wire `ChipOSManager`, `InputRouter`, the panel references, and the TMP text references in the Inspector.
6. Enter Play Mode.
7. Verify the clock updates, panels render, and keyboard test input works.

## Basic keyboard test map

- `H`: toggle HUD
- `R`: reset layout
- `P`: panic mode
- `T`: cycle primary task
- `G`: toggle gaze-follow
- `[` and `]`: change focused panel
- `Arrow Keys`: move focused panel
- `PageUp` and `PageDown`: move focused panel forward or back
- `F1`: toggle debug overlay
- `C`: toggle contrast mode
- `-` and `=`: adjust font scale
- `N`: show a test notification

## Current status

This repo is still at scaffold stage. It is meant to be easy to open, assemble, and iterate on in Unity before deeper NRSDK, XREAL, voice, camera, and assistant integrations are added.
